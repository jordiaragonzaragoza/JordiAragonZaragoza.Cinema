# 🔐 Sistema de Autorización

## 📌 Propósito

El sistema de autorización controla **lo que los actores pueden hacer**
dentro de un alcance de negocio específico. Responde a dos preguntas distintas:

| Pregunta | Mecanismo |
|----------|-----------|
| **¿Puede este usuario acceder a este alcance?** | `ValidateScopeAsync` — comprobación de membresía del alcance |
| **¿Puede este usuario realizar esta acción?** | `AuthorizeAsync` — comprobación de roles, permisos y políticas |

El sistema es intencionadamente **personalizado** — no utiliza
`Microsoft.AspNetCore.Authorization` ni `IAuthorizationService` de ASP.NET
Core. Esto mantiene la lógica de autorización en la capa de aplicación, desacoplada
de la infraestructura HTTP, aislada para pruebas y consciente del contexto de negocio
multi-inquilino (multi-tenant) que las políticas de ASP.NET Core no pueden expresar
de forma nativa.

---

## 🧩 Conceptos clave

### 1. Agregado de Usuario (User aggregate)

El agregado `User` es la fuente de verdad para los datos de autorización. Se
persiste en KurrentDB como un agregado basado en eventos (event-sourced). Su estado
es el resultado acumulado de todos los eventos relacionados con la autorización.

```
User
└── Assignments[]
      └── Assignment
            ├── Scope (TenantId, PartitionId?, CinemaId?)
            ├── Roles[]       ej. "Admin", "Viewer"
            └── Permissions[] ej. "cancel:showtime", "reserveSeats:showtime"
```

Un usuario tiene **cero o más Asignaciones (Assignments)**. Cada Asignación vincula
un conjunto de roles y permisos a un Alcance (Scope) específico. Un usuario puede
tener diferentes roles en diferentes alcances — por ejemplo, `Admin` a nivel de
Tenant pero también `Viewer` para un cine concreto dentro de ese mismo tenant.

---

### 2. Alcance (Scope) — niveles de acceso jerárquicos

`Scope` es un objeto de valor (value object) que representa el contexto de negocio
en el que se aplica una asignación. Tiene tres niveles, desde el más amplio al
más estrecho:

```
Tenant  (empresa / organización)
  └── Partition  (subdivisión regional o área)
        └── Cinema  (instancia de dominio específica)
```

El método `Scope.Matches` (en el dominio) aplica una **regla de especificidad**:
el nivel de alcance más estrecho definido en la asignación es el que prevalece.

```csharp
public bool Matches(CinemaId? cinemaId, PartitionId? partitionId, TenantId tenantId)
{
    if (this.CinemaId is not null)    return this.CinemaId == cinemaId;
    if (this.PartitionId is not null) return this.PartitionId == partitionId;
    return this.TenantId == tenantId;
}
```

El read model `UserAuthorizationReadModel` replica la misma regla con `Matches`,
para que la lógica de especificidad sea idéntica tanto en memoria (agregado) como
en la capa de lectura proyectada:

```csharp
public bool Matches(Guid tenantId, Guid? partitionId, Guid? domainId)
{
    if (this.CinemaId is not null)    return this.CinemaId == domainId;
    if (this.PartitionId is not null) return this.PartitionId == partitionId;
    return this.TenantId == tenantId;
}
```

Esto significa:

* Una asignación a nivel de **Tenant** otorga acceso a todas las particiones y
  cines dentro de ese tenant.
* Una asignación a nivel de **Partition** solo se aplica a esa partición.
* Una asignación a nivel de **Cinema** solo se aplica a ese cine específico.

#### Ejemplos de coincidencia de Alcance

| Alcance de la asignación | Alcance de la solicitud | ¿Coincide? |
|---|---|---|
| Tenant=A | Tenant=A, Partition=cualquiera, Cinema=cualquiera | ✅ |
| Tenant=A, Partition=P1 | Tenant=A, Partition=P1, Cinema=cualquiera | ✅ |
| Tenant=A, Partition=P1 | Tenant=A, Partition=P2 | ❌ |
| Tenant=A, Cinema=C1 | Tenant=A, Partition=cualquiera, Cinema=C1 | ✅ |
| Tenant=A, Cinema=C1 | Tenant=A, Cinema=C2 | ❌ |

---

### 3. Asignación (Assignment)

Una `Assignment` vincula un `Scope` a un conjunto de `Role`s y `Permission`s.
Es propiedad del agregado `User`.

Reglas de negocio aplicadas a nivel de dominio:

* Debe tener **al menos un rol o permiso**
  (`AssignmentMustHaveAtLeastOneRoleOrPermissionRule`).
* Un rol solo se puede asignar **una vez por alcance**
  (`OnlyPossibleToAssignRoleOnceRule`).
* Un permiso solo se puede asignar **una vez por alcance**
  (`OnlyPossibleToAssignPermissionOnceRule`).
* `GrantUser` sobre un alcance existente debe introducir **al menos un nuevo
  rol o permiso** (`GrantUserMustIntroduceNewRolesOrPermissionsRule`).

---

### 4. Rol (Role)

Un rol es una agrupación con nombre que describe semánticamente un nivel de acceso.
Los roles son cadenas de texto planas envueltas en un objeto de valor.

Roles actuales definidos en `Roles` (Application.Contracts):

| Rol | Significado previsto |
|---|---|
| `Admin` | Acceso completo de gestión |
| `Viewer` | Acceso de solo lectura |

> 📐 **Decisión de diseño**: `Roles` (Application.Contracts) y `Role` (Domain) son
> intencionadamente independientes. `Roles` es la clase de constantes públicas que
> cualquier bounded context usa para construir `[Authorize(Roles = Roles.Admin)]`.
> `Role` es el value object interno del agregado `User`. Si `Roles` referenciara
> `Role` del dominio, cualquier consumidor de `[Authorize]` arrastraría una
> dependencia transitiva al dominio de `User` — invirtiendo la dirección de
> dependencias de Clean Architecture. La consistencia entre ambas se garantiza
> mediante un test de arquitectura.

---

### 5. Permiso (Permission)

Un permiso es una cadena de texto de capacidad de grano fino. La convención es:

```
{acción}:{recurso}
```

Ejemplos de `ShowtimePermissions`:

| Permiso | Significado |
|---|---|
| `cancel:showtime` | Cancelar una función programada |
| `reserveSeats:showtime` | Reservar asientos para una función |
| `scheduleShowtime:showtime` | Crear una nueva función |
| `getAvailableSeats:showtime` | Consultar asientos disponibles |

---

### 6. Política (Policy)

Las políticas son reglas de autorización con nombre que van más allá de las
comprobaciones estáticas de roles y permisos — codifican lógica ABAC
(Control de Acceso Basado en Atributos) como "solo puede cancelar una reserva
si es el propietario o un administrador".

**Estado actual:** implementado y operativo. La evaluación se realiza en
`AuthorizationService.AuthorizeAsync` → `IPolicyEnforcer` → `IAuthorizationPolicy`.

Políticas disponibles:

| Constante | Significado |
|---|---|
| `Policies.SelfOrAdmin` | Pasa si el usuario es el propietario del recurso (`resourceId`) O tiene el rol `Admin` en el alcance activo |

#### Cómo funciona `SelfOrAdmin`

1. Si el usuario tiene el rol `Admin` → **pasa** inmediatamente (sin consultar el recurso).
2. Si no es Admin y no se proporcionó `resourceId` → **Forbidden** (fail-closed, nunca
   fail-open).
3. Si no es Admin y hay `resourceId` → carga el `ReservationReadModel` y compara
   `reservation.UserId == currentUserId`.

> ⚠️ **Fail-closed garantizado**: una policy que no puede evaluarse por falta de datos
> devuelve siempre `Forbidden`, nunca `Success`. Una policy declarada en `[Authorize]`
> pero sin implementación registrada lanza `InvalidOperationException` al arranque,
> evitando el riesgo de no-op silencioso.

#### Cómo declarar una policy en un command

El command implementa `IPolicyResourceRequest` para exponer el `resourceId` que
la policy necesita para resolver el ownership — sin exponer el shape completo del
command al sistema de autorización:

```csharp
[Authorize(Permissions = ShowtimePermisions.Cancel, Policies = Policies.SelfOrAdmin)]
public sealed record CancelReservationCommand(
    Guid ShowtimeId,
    Guid ReservationId) : ICommand, IPolicyResourceRequest
{
    // Solo el id del recurso cruza hacia el enforcer — ningún detalle más.
    public Guid ResourceId => this.ReservationId;
}
```

#### Añadir una nueva policy

1. Añadir la constante en `Policies` (Application.Common).
2. Implementar `IAuthorizationPolicy` en la capa de aplicación del bounded
   context correspondiente.
3. Registrar con `services.AddScoped<IAuthorizationPolicy, MiNuevaPolicy>()`.
4. El `PolicyEnforcer` la descubre automáticamente por DI.

---

### 7. Atributo `[Authorize]`

Se aplica a comandos y consultas para declarar sus requisitos de autorización:

```csharp
// Solo permiso
[Authorize(Permissions = "cancel:showtime")]
public sealed record CancelShowtimeCommand(Guid ShowtimeId) : ICommand;

// Solo rol
[Authorize(Roles = "Admin")]
public sealed record ScheduleShowtimeCommand(...) : ICommand;

// Combinado (AND — el usuario debe tener AMBOS)
[Authorize(Roles = "Admin", Permissions = "scheduleShowtime:showtime")]
public sealed record ...

// Múltiples atributos (AND — el usuario debe cumplir TODOS)
[Authorize(Roles = "Admin")]
[Authorize(Permissions = "cancel:showtime")]
public sealed record ...

// Con policy de ownership
[Authorize(Permissions = ShowtimePermisions.Cancel, Policies = Policies.SelfOrAdmin)]
public sealed record CancelReservationCommand(...) : ICommand, IPolicyResourceRequest { ... }

// Sin atributo = accesible para cualquier actor autorizado dentro del alcance
public sealed record GetShowtimesQuery(...) : IQuery<...>;
```

Semántica AND en todos los casos: se deben cumplir todos los atributos, todos
los roles declarados en un atributo, todos los permisos declarados, y todas las
políticas.

---

## ⚙️ Flujo de ejecución

### Ciclo de vida completo de la solicitud

```
Solicitud HTTP
    │
    ▼
ExecutionContextMiddleware
    ├── Resuelve el Actor según tipo:
    │     User     → JWT con claim 'oid' válido
    │     External → [AllowAnonymous] sin JWT
    │     null     → 401 Unauthorized
    ├── Valida x-tenant-id → ScopeContext
    │     User sin tenant → 400 Bad Request
    │     External sin tenant → SystemTenantId como fallback
    └── ValidateScopeAsync (solo para ActorType.User)
           Resolución jerárquica: ¿existe algún Assignment que cubra el scope?
           → 403 Forbidden si no hay ninguno
    │
    ▼
SetExecutionContext (AsyncLocal)
    │
    ▼
MediatR Pipeline
    │
    ├── ValidationBehavior (FluentValidation)
    │
    └── AuthorizationBehavior
            RequestAuthorizationService.TryAuthorizeAsync
              ├── Lee [Authorize] del command/query
              ├── Extrae resourceId si implementa IPolicyResourceRequest
              └── AuthorizationService.AuthorizeAsync
                    ├── ActorType.User  → AuthorizeUserAsync
                    │     ├── Resuelve UserAuthorizationReadModel (jerárquico)
                    │     ├── requiredPermissions ⊆ user permissions
                    │     ├── requiredRoles ⊆ user roles
                    │     └── Para cada policy → IPolicyEnforcer → IAuthorizationPolicy
                    ├── ActorType.System → Result.Success() (trusted, auditado)
                    └── ActorType.External → Result.Forbidden() (error de configuración)
    │
    └── CommandHandler / QueryHandler
```

### Comportamiento de autorización por tipo de actor

| ActorType | `ValidateScopeAsync` | `AuthorizeAsync` | Motivo |
|---|---|---|---|
| `User` | ✅ ejecutado | ✅ completo (roles + permisos + políticas) | Actor autenticado con identidad en el sistema |
| `System` | ❌ no aplica (no pasa por middleware HTTP) | `Result.Success()` | Sistema interno de confianza, acción auditada en KurrentDB |
| `External` | ❌ omitido en middleware | `Result.Forbidden()` — error de configuración | Nunca debe llegar a un command con `[Authorize]` |

> **`ActorType.External` en `AuthorizeAsync`**: si un actor External llega a un
> command decorado con `[Authorize]`, el sistema devuelve `Forbidden` en lugar de
> `Success` — es un error de configuración del endpoint (debería estar detrás de
> autenticación o no tener `[Authorize]`). Este comportamiento es fail-closed
> deliberado: External nunca debe heredar permisos de forma silenciosa.

> **`ActorType.System` y políticas de ownership**: los actores System omiten la
> evaluación de políticas (incluyendo `SelfOrAdmin`). Si un worker/consumer invoca
> un command con una policy de ownership, esa policy no se evalúa — System es
> trusted por diseño. Si el command proviene de un flujo de integración que debe
> respetar el ownership, propaga el `ActorId` original del usuario en el evento
> de integración para que el consumer reconstruya el contexto como `ActorType.User`.

---

### Paso 1 — Validación de alcance (middleware)

`ValidateScopeAsync` verifica que el usuario tenga **alguna asignación que cubra
jerárquicamente** el alcance solicitado. No comprueba roles ni permisos — solo
responde a: "¿Pertenece este usuario a este tenant/partición/cine?"

### Paso 2 — Autorización de comando/consulta (pipeline)

`AuthorizationService.AuthorizeAsync` delega en `AuthorizeUserAsync` para actores
`User`, que ejecuta en orden: permisos → roles → políticas. Un fallo en cualquier
paso devuelve inmediatamente sin evaluar los siguientes.

### Paso 3 — Modelo de lectura: `UserAuthorizationReadModel`

```
Agregado User (KurrentDB)
    │  UserGrantedEvent, RoleAssignedToScopeEvent, PermissionAssignedToScopeEvent, ...
    ▼
Proyectores (UserGrantedEventProjector, RoleAssignedToScopeEventProjector, ...)
    ▼
UserAuthorizationReadModel — una fila por nivel de Scope (PostgreSQL + Redis)
    ▼
UserAuthorizationResolver.ResolveAsync
    (GetUserAuthorizationCandidatesSpecification + Matches + especificidad)
    ▼
AuthorizationService.AuthorizeAsync / ValidateScopeAsync
```

---

## 🔍 Resolución jerárquica de scopes en el read model

### El problema (resuelto)

`GetUserAuthorizationCachedSpecification` filtra con igualdad SQL exacta, lo que
impide encontrar un Assignment de nivel Tenant cuando la request incluye un
`CinemaId` específico.

### La solución: dos specifications con responsabilidades distintas

| Specification | Propósito | Usado por |
|---|---|---|
| `GetUserAuthorizationCachedSpecification` | Identidad **exacta** — la fila concreta que un evento de dominio referencia | Proyectores (escritura) |
| `GetUserAuthorizationCandidatesSpecification` | Todas las filas del usuario en un tenant; resolución jerárquica con `Matches` en memoria | `UserAuthorizationResolver` (lectura) |

`UserAuthorizationResolver` encapsula la resolución jerárquica y añade un caché
en memoria por request (campo `last`) para evitar la doble carga del read model
entre `ValidateScopeAsync` (middleware) y `AuthorizeAsync` (pipeline):

```csharp
public async Task<UserAuthorizationReadModel?> ResolveAsync(
    Guid userId, ScopeContext scope, CancellationToken cancellationToken)
{
    if (this.last is { } cached && cached.UserId == userId && cached.Scope == scope)
        return cached.Result;  // ← evita segunda consulta a Redis en la misma request

    var candidates = await this.repository.ListAsync(
        new GetUserAuthorizationCandidatesSpecification(userId, scope.TenantId), ct);

    var result = candidates
        .Where(c => c.Matches(scope.TenantId, scope.PartitionId, scope.DomainId))
        .OrderByDescending(GetSpecificity)
        .FirstOrDefault();

    this.last = (userId, scope, result);
    return result;
}
```

> `UserAuthorizationResolver` debe registrarse como **Scoped** para que el caché
> en memoria sea por request y no cruce entre requests concurrentes.

---

## 🏗️ Gestión del acceso de usuarios

```csharp
// Otorgar acceso con roles y permisos
user.GrantUser(
    scope: Scope.Create(tenantId, partitionId, cinemaId),
    roles: [new Role(Roles.Admin)],
    permissions: [new Permission(ShowtimePermissions.Cancel)]);

// Revocar todo el acceso para un alcance
user.RevokeUser(scope);

// Añadir un único rol a un alcance existente
user.AssignRole(scope, new Role(Roles.Viewer));

// Eliminar un único permiso
user.RemovePermission(scope, new Permission(ShowtimePermissions.Cancel));
```

---

## 🗄️ Caché del read model

`BaseCachedSpecificationRepository` invalida **todas** las entradas de caché bajo
el prefijo `UserAuthorizationReadModel` en cada `AddAsync`/`UpdateAsync`/`DeleteAsync`,
automáticamente para todos los proyectores. No se requiere invalidación manual
por clave en ningún proyector.

El `UserAuthorizationResolver` añade una segunda capa de caché **en memoria por
request** (campo `last`), eliminando la doble consulta a Redis por request HTTP.

---

## 🚫 Decisiones de diseño

### ❌ Sin `Microsoft.AspNetCore.Authorization`

La autorización de ASP.NET Core está ligada a HTTP y centrada en políticas. No
puede expresar de forma nativa jerarquías de alcance multi-inquilino.

### ❌ Sin comprobaciones de roles/permisos en `ValidateScopeAsync`

La validación del alcance responde únicamente "¿pertenece este usuario aquí?" —
no "¿qué puede hacer?". Separación que mantiene el middleware rápido.

### ✅ Modelo de lectura para la autorización, no el agregado

El `UserAuthorizationReadModel` es una proyección con exactamente los datos
necesarios: roles y permisos por nivel de alcance, en caché en Redis.

### ✅ Una fila por nivel de Scope, resolución jerárquica en la consulta

La resolución jerárquica (`Matches`) se aplica en el momento de la lectura,
no en la proyección. Evita divergencia entre el comportamiento del dominio y
el del read model.

### ✅ `IPolicyEnforcer` desacoplado del shape del request

El enforcer recibe solo `(policy, currentUserId, currentUserRoles,
currentUserPermissions, resourceId?)` — nunca el command/query completo.
Esto mantiene el sistema de autorización independiente de los contratos de
la capa de presentación.

### ✅ `IAuthorizationPolicy` por estrategia, descubierta por DI

Cada policy es una clase independiente registrada en DI. `PolicyEnforcer` las
descubre por nombre en tiempo de ejecución. Añadir una nueva policy no requiere
modificar `PolicyEnforcer`.

### ✅ Policies viven en la capa de aplicación, no en el dominio

Las policies de tipo ownership necesitan consultar un repositorio (read model)
para resolver el dueño del recurso — esto es orquestación de application layer,
no una invariante del agregado. El dominio permanece ignorante de roles, permisos
y políticas de autorización.

### ✅ `ActorType.External` en `AuthorizeAsync` → Forbidden explícito

Los actores External solo deberían llegar a endpoints `[AllowAnonymous]` que
no declaran `[Authorize]`. Si llegan a `AuthorizeAsync`, es un error de
configuración que debe fallar de forma ruidosa (Forbidden), no silenciosa
(Success).

### ✅ `ActorType.System` → Success sin evaluación de políticas

Los workers y consumers del sistema son trusted porque su identidad y acción
están completamente auditadas vía `ExecutionContext` en `EventStoreMetadata`.
Si un flujo de integración necesita respetar ownership, debe propagar el
`ActorId` del usuario original en el evento de integración.

### ✅ `Roles` (Application.Contracts) y `Role` (Domain) desacoplados

Ver §4. Consistencia garantizada por test de arquitectura.

### ❌ Sin paginación en read models de autorización

El conjunto de Assignments de un usuario en un tenant es pequeño (1-5 filas)
y de alta repetición — ideal para caché sin paginar. Otros read models con
datasets grandes usan paginación sin caché.

---

## 🧠 Modelo mental

```
"¿Puede el usuario ACCEDER a este alcance?"        ValidateScopeAsync    (middleware)
       ↓ sí
"¿Puede el usuario REALIZAR esta operación?"       AuthorizeAsync        (pipeline)
       ↓ roles + permisos ok
"¿Cumple las POLÍTICAS de ownership/ABAC?"         IPolicyEnforcer       (pipeline)
       ↓ sí
"¿Es la operación VÁLIDA?"                          Reglas de dominio     (agregado)
```

```
ExecutionContext.ScopeContext
    │
    ▼
UserAuthorizationResolver.ResolveAsync
    ├── GetUserAuthorizationCandidatesSpecification (Redis → PostgreSQL)
    ├── .Where(Matches) + OrderByDescending(especificidad)
    └── UserAuthorizationReadModel (el más específico que cubre el scope)
              │
    ┌─────────┴──────────┐
    │  Roles[]           │  ← grano grueso: Admin, Viewer
    │  Permissions[]     │  ← grano fino: cancel:showtime
    └────────────────────┘
              │
    [Authorize] en command/query
              │
    AuthorizationService.AuthorizeAsync
              │
    IPolicyEnforcer (si hay Policies declaradas)
              │
    IAuthorizationPolicy.EvaluateAsync
         └── Carga ReservationReadModel para resolver ownership
```

---

## ⚠️ Casos esquina

### 1. Especificidad del alcance y asignaciones solapadas

La resolución en `UserAuthorizationResolver` es **excluyente**: selecciona
solo el Assignment más específico. Esto difiere del comportamiento del
agregado `User.GetRolesFor`, que es **aditivo** (unión de todos los que hacen
match). Esta diferencia es deliberada — ver §1 de decisiones de diseño.

```
El usuario tiene:
  Assignment A: Tenant=T1             → roles: [Viewer]
  Assignment B: Tenant=T1, Cinema=C1  → roles: [Admin]

AuthorizationService para Tenant=T1, Cinema=C1:
  → Solo Assignment B (Cinema, más específico) → [Admin]
  (no hereda [Viewer] de Assignment A)
```

### 2. Invalidación de caché — verificado, no es un problema

`BaseCachedSpecificationRepository` invalida por prefijo de tipo en cada
operación de escritura. Todos los proyectores de revocación pasan por este
repositorio base automáticamente.

### 3. `ActorType.System` omite políticas de ownership

Si un worker invoca un command con `[Authorize(Policies = "SelfOrAdmin")]`,
la policy no se evalúa — System pasa directamente. Para preservar el ownership
en flujos de integración, propaga el `ActorId` del usuario original en los
metadatos del evento de integración.

### 4. `ActorType.External` y `[AllowAnonymous]`

El middleware omite `ValidateScopeAsync` para External. Cualquier endpoint
`[AllowAnonymous]` que mute estado de negocio debe aplicar su propia
autorización a nivel de command — no puede depender del sistema de roles/permisos
porque External no tiene identidad en la plataforma.

### 5. `IPolicyResourceRequest` sin `[Authorize(Policies = ...)]` activo

Un command puede implementar `IPolicyResourceRequest` sin tener una policy
activa (ej. mientras la policy está en desarrollo/comentada). Esto no rompe
nada — `resourceId` simplemente no se usa. Sin embargo es ruido semántico:
documenta el motivo en el command si la policy está temporalmente desactivada.

### 6. Policy registrada en DI pero sin constante en `Policies`

Si se registra una `IAuthorizationPolicy` con un `Name` que no aparece en
ningún `[Authorize(Policies = ...)]`, es código muerto — no causa errores
pero tampoco se invoca nunca. Detectable con un test de arquitectura.

### 7. Propagación asíncrona del read model

`UserAuthorizationReadModel` se construye a partir de eventos de KurrentDB.
Existe un retraso entre un cambio de dominio (`GrantUser`) y su reflejo en
la autorización. En tests y durante catch-up de suscripción, un usuario puede
estar autorizado en el agregado pero no en el read model.

---

## 🧪 Ejemplos completos

### Ejemplo 1 — Usuario cancela una función (permisos)

```
Usuario U1: Assignment Tenant=T1, Cinema=C1 → [Admin] + [cancel:showtime]

POST /api/v2/showtimes/{id}/cancel
  x-tenant-id: T1, x-domain-id: C1

ValidateScopeAsync → Assignment(T1,null,C1).Matches(T1,null,C1) ✅
AuthorizeAsync:
  requiredPermissions: ["cancel:showtime"] ⊆ ["cancel:showtime"] ✅
  → CancelShowtimeCommandHandler ejecuta
```

### Ejemplo 2 — Usuario cancela su reserva (policy SelfOrAdmin)

```
Usuario U1: Assignment Tenant=T1 → [Viewer] + [cancel:reservation]
Reserva R1: UserId = U1

POST /api/v2/reservations/{R1}/cancel
  [Authorize(Permissions = "cancel:reservation", Policies = "SelfOrAdmin")]

ValidateScopeAsync → Assignment(T1).Matches(T1) ✅
AuthorizeAsync:
  requiredPermissions: ["cancel:reservation"] ⊆ ["cancel:reservation"] ✅
  requiredRoles: [] → ok
  policy "SelfOrAdmin":
    currentUserRoles no contiene "Admin"
    resourceId = R1
    ReservationReadModel(R1).UserId == U1 ✅
  → Autorizado ✅
```

### Ejemplo 3 — Usuario intenta cancelar la reserva de otro (policy falla)

```
Usuario U2: Assignment Tenant=T1 → [Viewer]
Reserva R1: UserId = U1 (no es U2)

policy "SelfOrAdmin":
  currentUserRoles no contiene "Admin"
  ReservationReadModel(R1).UserId (U1) != currentUserId (U2)
  → Result.Forbidden ❌
```

### Ejemplo 4 — Admin cancela reserva de cualquier usuario

```
Usuario U3: Assignment Tenant=T1 → [Admin]
Reserva R1: UserId = U1

policy "SelfOrAdmin":
  currentUserRoles.Contains("Admin") → Result.Success() ✅
  (no se consulta ReservationReadModel)
```

### Ejemplo 5 — Tenant Admin accede a cinema específico (jerarquía)

```
Usuario U4: Assignment Tenant=T1 (PartitionId=null, CinemaId=null) → [Admin]

Request: x-tenant-id: T1, x-domain-id: C5

ValidateScopeAsync:
  Candidatos: [Assignment(T1, null, null)]
  Matches(T1, null, C5): CinemaId=null → PartitionId=null → TenantId==T1 ✅
  → Encontrado ✅
```

---

## 📋 Referencia de contratos

```csharp
// Declarar autorización en un command/query
[Authorize(Permissions = "cancel:showtime")]
[Authorize(Roles = "Admin")]
[Authorize(Policies = Policies.SelfOrAdmin)]

// Exponer resourceId para policies de ownership
public sealed record MiCommand(...) : ICommand, IPolicyResourceRequest
{
    public Guid ResourceId => this.RecursoId;
}

// Implementar una nueva policy
public sealed class MiPolicy : IAuthorizationPolicy
{
    public string Name => Policies.MiNuevaPolicy;

    public async Task<Result> EvaluateAsync(
        Guid currentUserId,
        IReadOnlyCollection<string> currentUserRoles,
        IReadOnlyCollection<string> currentUserPermissions,
        Guid? resourceId,
        CancellationToken cancellationToken = default) { ... }
}

// Registrar la policy en DI
services.AddScoped<IAuthorizationPolicy, MiPolicy>();
```