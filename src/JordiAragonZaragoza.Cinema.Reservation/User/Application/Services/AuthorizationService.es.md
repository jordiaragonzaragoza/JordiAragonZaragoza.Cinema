# 🔐 Sistema de Autorización

## 📌 Propósito

El sistema de autorización controla **lo que los usuarios autenticados pueden hacer**
dentro de un alcance de negocio específico. Responde a dos preguntas distintas:

| Pregunta | Mecanismo |
|----------|-----------|
| **¿Puede este usuario acceder a este alcance?** | `ValidateScopeAsync` — comprobación de membresía del alcance |
| **¿Puede este usuario realizar esta acción?** | `AuthorizeAsync` — comprobación de roles y permisos |

El sistema es intencionadamente **personalizado** — no utiliza
`Microsoft.AspNetCore.Authorization` ni `IAuthorizationService` de ASP.NET
Core. Esto mantiene la lógica de autorización en la capa de aplicación, desacoplada de
la infraestructura HTTP, aislada para pruebas y consciente del contexto de negocio multi-inquilino
(multi-tenant) que las políticas de ASP.NET Core no pueden expresar de forma nativa.

---

## 🧩 Conceptos clave

### 1. Agregado de Usuario (User aggregate)

El agregado `User` es la fuente de verdad para los datos de autorización. Se
persiste en KurrentDB como un agregado basado en eventos (event-sourced). Su estado es el
resultado acumulado de todos los eventos relacionados con la autorización.


```

User
└── Assignments[]
└── Assignment
├── Scope (TenantId, PartitionId?, CinemaId?)
├── Roles[]       ej. "Admin", "Viewer"
└── Permissions[] ej. "cancel:showtime", "reserveSeats:showtime"

```

Un usuario tiene **cero o más Asignaciones (Assignments)**. Cada Asignación vincula un conjunto de
roles y permisos a un Alcance (Scope) específico. Un usuario puede tener diferentes roles
en diferentes alcances — por ejemplo, `Admin` en un cine pero `Viewer` en
otro.

---

### 2. Alcance (Scope) — niveles de acceso jerárquicos

`Scope` es un objeto de valor (value object) que representa el contexto de negocio en el que
se aplica una asignación. Tiene tres niveles, desde el más amplio al más estrecho:


```

Tenant  (empresa / organización)
└── Partition  (subdivisión regional o área)
└── Cinema  (instancia de dominio específica)

```

El método `Scope.Matches` aplica una **regla de especificidad**: el nivel de alcance más estrecho
definido en la asignación es el que prevalece.

```csharp
public bool Matches(CinemaId? cinemaId, PartitionId? partitionId, TenantId tenantId)
{
    if (this.CinemaId is not null)   return this.CinemaId == cinemaId;
    if (this.PartitionId is not null) return this.PartitionId == partitionId;
    return this.TenantId == tenantId;
}

```

Esto significa:

* Una asignación a nivel de **Tenant** otorga acceso a todas las particiones y
cines dentro de ese tenant.
* Una asignación a nivel de **Partition** solo se aplica a esa partición,
independientemente del tenant.
* Una asignación a nivel de **Cinema** solo se aplica a ese cine específico.

#### Ejemplos de coincidencia de Alcance (Scope matching)

| Alcance de la asignación | Alcance de la solicitud | ¿Coincide? |
| --- | --- | --- |
| Tenant=A | Tenant=A, Partition=cualquiera, Cinema=cualquiera | ✅ |
| Tenant=A, Partition=P1 | Tenant=A, Partition=P1, Cinema=cualquiera | ✅ |
| Tenant=A, Partition=P1 | Tenant=A, Partition=P2 | ❌ |
| Tenant=A, Cinema=C1 | Tenant=A, Partition=cualquiera, Cinema=C1 | ✅ |
| Tenant=A, Cinema=C1 | Tenant=A, Cinema=C2 | ❌ |

---

### 3. Asignación (Assignment)

Una `Assignment` es una entidad que vincula un `Scope` a un conjunto de `Role`s
y `Permission`s. Es propiedad del agregado `User`.

Reglas de negocio aplicadas a nivel de dominio:

* Una asignación debe tener **al menos un rol o permiso**
(`AssignmentMustHaveAtLeastOneRoleOrPermissionRule`).
* Un rol solo se puede asignar **una vez por alcance**
(`OnlyPossibleToAssignRoleOnceRule`).
* A permiso solo se puede asignar **una vez por alcance**
(`OnlyPossibleToAssignPermissionOnceRule`).
* Invocar `GrantUser` en un alcance existente debe introducir **al menos un
nuevo rol o permiso** (`GrantUserMustIntroduceNewRolesOrPermissionsRule`).

---

### 4. Rol (Role)

Un rol es una agrupación con nombre que describe semánticamente un nivel de acceso.
Los roles son cadenas de texto planas envueltas en un objeto de valor.

Roles actuales definidos en `Roles`:

| Rol | Significado previsto |
| --- | --- |
| `Admin` | Acceso completo de gestión |
| `Viewer` | Acceso de solo lectura |

Los roles son de grano grueso (coarse-grained). Son validados por el `AuthorizationBehavior`
cuando un comando o consulta declara `[Authorize(Roles = "Admin")]`.

---

### 5. Permiso (Permission)

Un permiso es una cadena de texto de capacidad de grano fino (fine-grained). La convención es:

```
{acción}:{recurso}

```

Ejemplos de `ShowtimePermissions`:

| Permiso | Significado |
| --- | --- |
| `cancel:showtime` | Cancelar una función programada |
| `reserveSeats:showtime` | Reservar asientos para una función |
| `scheduleShowtime:showtime` | Crear una nueva función |
| `getAvailableSeats:showtime` | Consultar asientos disponibles |

Los permisos son de grano fino. Se validan cuando un comando o consulta
declara `[Authorize(Permissions = "cancel:showtime")]`.

---

### 6. Política (Policy)

Las políticas son reglas de autorización con nombre que van más allá de las comprobaciones de roles y
permisos — pueden codificar lógica ABAC (Control de Acceso Basado en Atributos) como
"solo puede cancelar una función si es el propietario" o "solo puede actuar sobre
recursos dentro de su región".

> **Estado actual:** La evaluación de políticas está declarada en `AuthorizeAttribute`
> y es analizada por `RequestAuthorizationService`, pero el bucle de ejecución en
> `AuthorizationService.AuthorizeAsync` aún no está implementado (marcado como TODO).
> Las políticas están reservadas para futuros escenarios ABAC.

---

### 7. Atributo `[Authorize]`

Se aplica a comandos y consultas para declarar sus requisitos de autorización:

```csharp
[Authorize(Permissions = "cancel:showtime")]
public sealed record CancelShowtimeCommand(Guid ShowtimeId) : ICommand;

[Authorize(Roles = "Admin")]
public sealed record ScheduleShowtimeCommand(...) : ICommand;

// Combinado — el usuario debe tener AMBOS
[Authorize(Roles = "Admin", Permissions = "scheduleShowtime:showtime")]
public sealed record ...

// Múltiples atributos — el usuario debe cumplir con TODOS los atributos
[Authorize(Roles = "Admin")]
[Authorize(Permissions = "cancel:showtime")]
public sealed record ...

```

Cuando hay múltiples atributos `[Authorize]` presentes, se deben cumplir todos
(semántica AND). Dentro de un solo atributo, se deben cumplir todos los roles y
permisos declarados (también semántica AND).

---

## ⚙️ Flujo de ejecución

### Ciclo de vida completo de la solicitud

```
Solicitud HTTP
    │
    ▼
ExecutionContextMiddleware
    ├── Resuelve el Actor (JWT → user:{guid})
    ├── Valida x-tenant-id → ScopeContext
    └── ValidateScopeAsync ──────────────────────────────────────────┐
           Comprueba si existe UserAuthorizationReadModel para         │
           (userId, tenantId, partitionId?, cinemaId?)              │
           → 403 si el usuario no tiene asignación en este alcance    │
    │                                                               │
    ▼                                                               │
Establece ExecutionContext (AsyncLocal)                             │
    │                                                               │
    ▼                                                               │
Canalización de MediatR (MediatR Pipeline)                          │
    │                                                               │
    ├── ValidationBehavior (FluentValidation)                       │
    │                                                               │
    ├── AuthorizationBehavior ──────────────────────────────────────┘
    │       RequestAuthorizationService.TryAuthorizeAsync
    │         ├── Lee los atributos [Authorize] del comando/consulta
    │         └── AuthorizationService.AuthorizeAsync
    │               ├── Carga UserAuthorizationReadModel (en caché)
    │               ├── Comprueba permisos requeridos ⊆ permisos usuario
    │               ├── Comprueba roles requeridos ⊆ roles usuario
    │               └── (TODO) Evalúa políticas
    │
    └── CommandHandler / QueryHandler

```

### Paso 1 — Validación de alcance (middleware)

`ValidateScopeAsync` verifica que el usuario tenga **cualquier asignación** en el
alcance solicitado. No comprueba roles ni permisos — solo responde a:
"¿Pertenece este usuario a este tenant/partición/cine en absoluto?"

```csharp
// AuthorizationService.ValidateScopeAsync
var userAuthorization = await repository.SingleOrDefaultAsync(
    new GetUserAuthorizationCachedSpecification(query), ct);

if (userAuthorization is null)
    return Result.NotFound(...);

return Result.Success();

```

Esta es la **puerta de acceso** — si el usuario no está en el alcance, la solicitud es
rechazada con un 403 antes de que el comando llegue a la canalización.

### Paso 2 — Autorización de comando/consulta (comportamiento de la canalización)

`AuthorizationBehavior` se ejecuta para cada comando y consulta. Si la solicitud
no tiene el atributo `[Authorize]`, pasa inmediatamente (acción pública
dentro del alcance).

Para solicitudes decoradas:

```csharp
// RequestAuthorizationService
var authorizationAttributes = request.GetType()
    .GetCustomAttributes<AuthorizeAttribute>().ToList();

if (authorizationAttributes.Count == 0)
    return Result.Success();   // ← no se requiere autorización

var requiredPermissions = authorizationAttributes
    .SelectMany(a => a.Permissions?.Split(',') ?? [])
    .ToList().AsReadOnly();

var requiredRoles = authorizationAttributes
    .SelectMany(a => a.Roles?.Split(',') ?? [])
    .ToList().AsReadOnly();

return await authorizationService.AuthorizeAsync(
    requiredRoles, requiredPermissions, requiredPolicies, ct);

```

Luego, `AuthorizationService.AuthorizeAsync`:

1. Lee el `ExecutionContext` desde `IExecutionContextService`.
2. Omite la autorización para actores que no sean de tipo Usuario (las llamadas de servicio
a servicio son confiables en esta capa — ver casos esquina).
3. Carga `UserAuthorizationReadModel` desde el modelo de lectura en caché.
4. Realiza comprobaciones de diferencia de conjuntos:

```csharp
// Cualquier permiso requerido que no esté en el conjunto del usuario → Prohibido (Forbidden)
if (requiredPermissions.Except(userAuthorization.Permissions
    .Select(p => p.Value)).Any())
    return Result.Forbidden(...);

// Cualquier rol requerido que no esté en el conjunto del usuario → Prohibido (Forbidden)
if (requiredRoles.Except(userAuthorization.Roles
    .Select(r => r.Value)).Any())
    return Result.Forbidden(...);

```

### Paso 3 — Modelo de lectura: `UserAuthorizationReadModel`

La comprobación de autorización lee desde un **modelo de lectura proyectado**, no directamente
del agregado `User`. Este modelo de lectura es mantenido por proyectores
que reaccionan a los eventos de dominio de `User` (`UserGrantedEvent`,
`RoleAssignedToScopeEvent`, etc.) a través de la suscripción de KurrentDB.

```
Agregado User (KurrentDB)
    │  UserGrantedEvent
    │  RoleAssignedToScopeEvent
    │  PermissionAssignedToScopeEvent
    ▼
UserAuthorizationProjector
    ▼
UserAuthorizationReadModel (PostgreSQL, en caché en Redis)
    ▼
AuthorizationService.AuthorizeAsync

```

El modelo de lectura se almacena en caché utilizando una clave de especificación de caché:

```
GetUserAuthorizationCachedSpecification_{userId}_{tenantId}_{partitionId}_{cinemaId}

```

Esto significa que las comprobaciones de autorización se sirven desde Redis después de la primera
búsqueda, lo que las hace extremadamente rápidas para las solicitudes subsiguientes.

---

## 🏗️ Gestión del acceso de usuarios

### Otorgar acceso a un usuario a un alcance con roles y permisos

```csharp
user.GrantUser(
    scope: Scope.Create(tenantId, partitionId, cinemaId),
    roles: [new Role(Roles.Admin)],
    permissions: [new Permission(ShowtimePermissions.Cancel)]);

```

Esto emite `UserGrantedEvent` (nuevo alcance) o `RoleAssignedToScopeEvent` /
`PermissionAssignedToScopeEvent` (alcance existente).

### Revocar todo el acceso para un alcance

```csharp
user.RevokeUser(scope);
// Emite: UserRevokedEvent → elimina la Asignación completa

```

### Añadir un único rol a un alcance existente

```csharp
user.AssignRole(scope, new Role(Roles.Viewer));
// Emite: RoleAssignedToScopeEvent

```

### Eliminar un único permiso

```csharp
user.RemovePermission(scope, new Permission(ShowtimePermissions.Cancel));
// Emite: PermissionRevokedFromScopeEvent

```

---

## 🚫 Decisiones de diseño

### ❌ Sin `Microsoft.AspNetCore.Authorization`

La autorización de ASP.NET Core está ligada a HTTP y centrada en políticas. No puede
expresar de forma nativa jerarquías de alcance multi-inquilino. El servicio personalizado
`IAuthorizationService` reside en la capa de aplicación y se puede invocar
desde cualquier manejador — no solo desde controladores.

### ❌ Sin comprobaciones de roles/permisos en `ValidateScopeAsync`

La validación del alcance es intencionadamente gruesa. Responde únicamente a "¿pertenece este usuario
aquí?" — no a "¿qué puede hacer?". Esta separación mantiene el
middleware rápido y permite que el comportamiento de la canalización aplique un control de acceso
de grano fino por operación.

### ✅ Modelo de lectura para la autorización, no el agregado

Cargar el agregado `User` completo para cada comprobación de autorización
requeriría reproducir potencialmente cientos de eventos. El `UserAuthorizationReadModel`
es una instantánea proyectada de antemano y almacenada en caché con exactamente los datos necesarios para el
control de acceso: roles y permisos con alcance.

### ✅ La clave de caché incluye el alcance completo

La clave de caché codifica `(userId, tenantId, partitionId, cinemaId)`. Esto
significa que un usuario que opera en diferentes alcances obtiene entradas de caché independientes —
sin riesgo de filtrar permisos a través de los límites del alcance.

### ✅ Las llamadas servicio a servicio omiten la autorización de usuario

Cuando `ActorType != User`, `AuthorizeAsync` devuelve éxito incondicionalmente.
Los servicios internos son de confianza en esta capa — ya están autenticados
a través de la identidad del servicio, y sus acciones son audidables a través del
`ExecutionContext` (ver casos esquina para riesgos).

### ✅ Los permisos y roles utilizan la semántica AND

Se deben cumplir todos los requisitos declarados. No existe la semántica OR
dentro de un único atributo `[Authorize]`. Para escenarios OR, el enfoque
recomendado es declarar el permiso más amplio que cubra la operación,
y dejar que el dominio aplique reglas más finas.

---

## 🧠 Modelo mental

```
"¿Puede el usuario ACCEDER a este alcance?"        ValidateScopeAsync    (middleware)
       ↓ sí
"¿Puede el usuario REALIZAR esta operación?"     AuthorizeAsync        (canalización)
       ↓ sí
"¿Es la operación VÁLIDA?"                         Reglas de dominio     (agregado)

```

Tres capas, tres preocupaciones, tres lugares donde fallar — cada uno de ellos
independientemente integrable en pruebas y evolucionable por separado.

```
ExecutionContext
    └── ScopeContext (tenantId, partitionId?, domainId?)
              │
              ▼
    UserAuthorizationReadModel
              │
    ┌─────────┴──────────┐
    │  Roles[]           │  ← grano grueso: Admin, Viewer
    │  Permissions[]     │  ← grano fino: cancel:showtime
    └────────────────────┘
              │
    Atributo [Authorize] en comando/consulta
              │
    AuthorizationService.AuthorizeAsync

```

---

## ⚠️ Casos esquina (Corner cases)

### 1. Especificidad del alcance y asignaciones superpuestas

Un usuario puede tener asignaciones en múltiples niveles de alcance. `Scope.Matches` utiliza
el **nivel definido más estrecho** de la asignación — no de la solicitud.

```
El usuario tiene:
  Asignación A: Tenant=T1             → roles: [Viewer]
  Asignación B: Tenant=T1, Cinema=C1 → roles: [Admin]

Alcance de la solicitud: Tenant=T1, Cinema=C1

GetRolesFor(tenantId=T1, partitionId=null, cinemaId=C1):
  Asignación A: CinemaId=null, PartitionId=null → coincide en TenantId=T1 ✅
  Asignación B: CinemaId=C1 → coincide en CinemaId=C1 ✅

Resultado: [Viewer, Admin]  ← AMBAS asignaciones coinciden

```

`GetRolesFor` y `GetPermissionsFor` utilizan `.Where(a => a.Scope.Matches(...))`
el cual devuelve **todas las asignaciones que coincidan**, no solo la más específica.
Un usuario hereda permisos de alcances más amplios. Esto es aditivo.

### 2. Caducidad de la caché tras cambios de autorización

Cuando los roles o permisos de un usuario cambian (por ejemplo, se procesa `RoleAssignedToScopeEvent`),
la entrada de caché para ese usuario+alcance debe ser invalidada.
El proyector que actualiza `UserAuthorizationReadModel` también debe desalojar
la clave afectada en la caché de Redis.

> Si la invalidación de la caché no está implementada en el proyector, los usuarios
> verán datos de autorización obsoletos hasta que expire el TTL de la caché. Este es un
> riesgo conocido con las proyecciones asíncronas basadas en eventos y los modelos de lectura en caché.

### 3. La autorización de servicio a servicio es implícita

Cuando `ActorType != User`, `AuthorizeAsync` devuelve `Result.Success()`
incondicionalmente. No existe todavía un modelo de permisos de servicio. Un servicio interno
comprometido podría realizar cualquier acción.

Mitigación en el diseño actual: cada acción del servicio se registra en
KurrentDB con el `ExecutionContext` completo (incluyendo el `ActorId` del
servicio), haciéndolo auditable a posteriori.

### 4. `ValidateScopeAsync` se omite para actores `External`

El middleware omite `ValidateScopeAsync` para `ActorType.External`. Los actores externos
(solicitudes no autenticadas como el registro) utilizan `SystemTenantId`
como respaldo y omiten la validación de alcance. Cualquier endpoint que permita
`[AllowAnonymous]` y realice operaciones comerciales significativas debe aplicar
sus propias reglas de autorización a nivel de comando.

### 5. La aplicación de políticas aún no está implementada

`[Authorize(Policies = "...")]` es analizado y reenviado a
`AuthorizationService.AuthorizeAsync`, pero el bucle de evaluación está comentado
(TODO). Declarar una política en un comando actualmente no tiene ningún efecto.
No confíe en las políticas para el control de acceso hasta que la implementación esté completa.

### 6. `GetUserActorId()` lanza una excepción para actores que no son de tipo Usuario

El asistente en `ExecutionContext` lanza `InvalidOperationException` si es invocado
cuando `ActorType != User`. Cualquier ruta de código que pueda manejar múltiples tipos de actores
debe protegerse con `if (actorType == ActorType.User)` antes de llamarlo.
`AuthorizationService` lo hace correctamente.

### 7. El modelo de lectura refleja una proyección asíncrona — no el agregado

Debido a que `UserAuthorizationReadModel` se construye a partir de eventos de suscripción de KurrentDB,
existe un retraso de propagación inherente entre un cambio de dominio
(por ejemplo, `GrantUser`) y el reflejo de este en la comprobación de autorización. En la práctica,
esto es cuestión de milisegundos, pero en pruebas o durante el procesamiento de suscripciones
de puesta al día (catch-up), un usuario puede estar autorizado a nivel de agregado pero no estarlo
aún a nivel de modelo de lectura.

---

## 🧪 Ejemplos completos

### Ejemplo 1 — Un usuario cancela una función

```
Precondiciones:
  El usuario U1 tiene la Asignación:
    Alcance: Tenant=T1, Cinema=C1
    Roles: [Admin]
    Permisos: [cancel:showtime]

Solicitud:
  POST /api/v2/showtimes/{id}/cancel
  Authorization: Bearer {jwt, oid=U1}
  x-tenant-id: T1
  x-domain-id: C1

ExecutionContextMiddleware:
  1. ResolveActor → user:U1, ActorType.User
  2. ResolveTenant → T1
  3. ValidateScopeAsync(U1, T1, null, C1)
     → Se encontró UserAuthorizationReadModel ✅
  4. SetExecutionContext(...)

Canalización de MediatR:
  5. AuthorizationBehavior
     → [Authorize(Permissions = "cancel:showtime")]
     → AuthorizeAsync
         requiredPermissions: ["cancel:showtime"]
         userAuthorization.Permissions: ["cancel:showtime"]
         difference: [] → ✅ Autorizado

  6. Ejecución de CancelShowtimeCommandHandler

```

### Ejemplo 2 — Un espectador intenta cancelar (prohibido)

```
El usuario U2 tiene la Asignación:
  Alcance: Tenant=T1
  Roles: [Viewer]
  Permisos: [getShowtime:showtime, getShowtimes:showtime]

Solicitud: POST /api/v2/showtimes/{id}/cancel

AuthorizeAsync:
  requiredPermissions: ["cancel:showtime"]
  userAuthorization.Permissions: ["getShowtime:showtime", "getShowtimes:showtime"]
  difference: ["cancel:showtime"] → no está vacío → Result.Forbidden ❌

```

### Ejemplo 3 — Un usuario accede al tenant incorrecto (prohibido en el alcance)

```
El usuario U3 tiene una Asignación únicamente para Tenant=T2

Solicitud:
  x-tenant-id: T1   (tenant diferente)

ValidateScopeAsync(U3, T1, null, null):
  → No se encontró UserAuthorizationReadModel para T1
  → Result.NotFound → 403 Forbidden ❌
  (nunca llega a la canalización)

```

### Ejemplo 4 — Un Administrador a nivel de Tenant accede a cualquier cine

```
El usuario U4 tiene la Asignación:
  Alcance: Tenant=T1 (sin partición, sin cine)
  Roles: [Admin]
  Permisos: [cancel:showtime, scheduleShowtime:showtime, ...]

Solicitud:
  x-tenant-id: T1
  x-domain-id: C5  (cualquier cine en T1)

ValidateScopeAsync(U4, T1, null, C5):
  Especificación: userId=U4, tenantId=T1, partitionId=null, cinemaId=C5
  Consulta (Query): WHERE partitionId IS NULL AND cinemaId = C5
  → No se encontró ninguna fila porque la asignación tiene cinemaId=null ⚠️

```

> **Esta es una brecha potencial**: `ValidateScopeAsync` utiliza la especificación
> `GetUserAuthorizationCachedSpecification` la cual filtra por la tupla exacta
> `(userId, tenantId, partitionId, cinemaId)`. Una asignación a nivel de tenant
> (cinemaId=null) no coincidirá con una solicitud que tenga un cinemaId específico.
> El proyector del modelo de lectura debe manejar esto mediante:
> * El almacenamiento de una fila por nivel de alcance (solo tenant, solo partición, específico de cine), o
> * Haciendo que `ValidateScopeAsync` realice una consulta jerárquica de respaldo (fallback)
> (comprobar cine → luego partición → luego tenant).
> 
> 
> Verifique que su proyector y especificación manejen esto correctamente.

---

## 📋 Referencia del atributo de autorización

```csharp
// Solo permiso
[Authorize(Permissions = "cancel:showtime")]

// Solo rol
[Authorize(Roles = "Admin")]

// Ambos (AND — el usuario debe tener ambos)
[Authorize(Roles = "Admin", Permissions = "scheduleShowtime:showtime")]

// Múltiples atributos (AND — el usuario debe cumplir con todos)
[Authorize(Roles = "Admin")]
[Authorize(Permissions = "cancel:showtime")]

// Política (aún no se ejecuta)
[Authorize(Policies = "OwnerOnly")]

// Sin atributo = accesible para cualquier usuario dentro del alcance
public sealed record GetShowtimesQuery(...) : IQuery<...>;