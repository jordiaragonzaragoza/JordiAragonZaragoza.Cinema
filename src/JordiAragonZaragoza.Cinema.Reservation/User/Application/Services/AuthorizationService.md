# 🔐 Authorization System

## 📌 Purpose

The authorization system controls **what authenticated users can do**
within a specific business scope. It answers two distinct questions:

| Question | Mechanism |
|----------|-----------|
| **Can this user access this scope?** | `ValidateScopeAsync` — scope membership check |
| **Can this user perform this action?** | `AuthorizeAsync` — roles and permissions check |

The system is intentionally **custom** — it does not use
`Microsoft.AspNetCore.Authorization` or `IAuthorizationService` from ASP.NET
Core. This keeps authorization logic in the application layer, decoupled from
HTTP infrastructure, testable in isolation, and aware of the multi-tenant
business context that ASP.NET Core policies cannot natively express.

---

## 🧩 Key concepts

### 1. User aggregate

The `User` aggregate is the source of truth for authorization data. It is
persisted in KurrentDB as an event-sourced aggregate. Its state is the
accumulated result of all authorization-related events.

```
User
└── Assignments[]
      └── Assignment
            ├── Scope (TenantId, PartitionId?, CinemaId?)
            ├── Roles[]       e.g. "Admin", "Viewer"
            └── Permissions[] e.g. "cancel:showtime", "reserveSeats:showtime"
```

A user has **zero or more Assignments**. Each Assignment binds a set of
roles and permissions to a specific Scope. A user can have different roles
in different scopes — for example, `Admin` in one cinema but `Viewer` in
another.

---

### 2. Scope — hierarchical access levels

`Scope` is a value object that represents the business context in which
an assignment applies. It has three levels, from broadest to narrowest:

```
Tenant  (company / organization)
  └── Partition  (regional subdivision or area)
        └── Cinema  (specific domain instance)
```

The `Scope.Matches` method applies a **specificity rule**: the narrowest
scope level defined on the assignment wins.

```csharp
public bool Matches(CinemaId? cinemaId, PartitionId? partitionId, TenantId tenantId)
{
    if (this.CinemaId is not null)   return this.CinemaId == cinemaId;
    if (this.PartitionId is not null) return this.PartitionId == partitionId;
    return this.TenantId == tenantId;
}
```

This means:
- An assignment at **Tenant** level grants access to all partitions and
  cinemas within that tenant.
- An assignment at **Partition** level only applies to that partition,
  regardless of tenant.
- An assignment at **Cinema** level only applies to that specific cinema.

#### Scope matching examples

| Assignment scope              | Request scope                        | Matches? |
|-------------------------------|--------------------------------------|----------|
| Tenant=A                      | Tenant=A, Partition=any, Cinema=any  | ✅        |
| Tenant=A, Partition=P1        | Tenant=A, Partition=P1, Cinema=any   | ✅        |
| Tenant=A, Partition=P1        | Tenant=A, Partition=P2               | ❌        |
| Tenant=A, Cinema=C1           | Tenant=A, Partition=any, Cinema=C1   | ✅        |
| Tenant=A, Cinema=C1           | Tenant=A, Cinema=C2                  | ❌        |

---

### 3. Assignment

An `Assignment` is an entity that binds a `Scope` to a set of `Role`s
and `Permission`s. It is owned by the `User` aggregate.

Business rules enforced at domain level:
- An assignment must have **at least one role or permission**
  (`AssignmentMustHaveAtLeastOneRoleOrPermissionRule`).
- A role can only be assigned **once per scope**
  (`OnlyPossibleToAssignRoleOnceRule`).
- A permission can only be assigned **once per scope**
  (`OnlyPossibleToAssignPermissionOnceRule`).
- Calling `GrantUser` on an existing scope must introduce **at least one
  new role or permission** (`GrantUserMustIntroduceNewRolesOrPermissionsRule`).

---

### 4. Role

A role is a named grouping that semantically describes a level of access.
Roles are plain strings wrapped in a value object.

Current roles defined in `Roles`:

| Role    | Intended meaning                  |
|---------|-----------------------------------|
| `Admin` | Full management access            |
| `Viewer`| Read-only access                  |

Roles are coarse-grained. They are checked by the `AuthorizationBehavior`
when a command or query declares `[Authorize(Roles = "Admin")]`.

---

### 5. Permission

A permission is a fine-grained capability string. The convention is:

```
{action}:{resource}
```

Examples from `ShowtimePermissions`:

| Permission                    | Meaning                              |
|-------------------------------|--------------------------------------|
| `cancel:showtime`             | Cancel a scheduled showtime          |
| `reserveSeats:showtime`       | Reserve seats for a showtime         |
| `scheduleShowtime:showtime`   | Create a new showtime                |
| `getAvailableSeats:showtime`  | Query available seats                |

Permissions are fine-grained. They are checked when a command or query
declares `[Authorize(Permissions = "cancel:showtime")]`.

---

### 6. Policy

Policies are named authorization rules that go beyond role and permission
checks — they can encode ABAC (Attribute-Based Access Control) logic such
as "can only cancel a showtime if you are the owner" or "can only act on
resources within your region".

> **Current status:** Policy evaluation is declared in `AuthorizeAttribute`
> and parsed by `RequestAuthorizationService`, but the enforcement loop in
> `AuthorizationService.AuthorizeAsync` is not yet implemented (marked TODO).
> Policies are reserved for future ABAC scenarios.

---

### 7. `[Authorize]` attribute

Applied to commands and queries to declare their authorization requirements:

```csharp
[Authorize(Permissions = "cancel:showtime")]
public sealed record CancelShowtimeCommand(Guid ShowtimeId) : ICommand;

[Authorize(Roles = "Admin")]
public sealed record ScheduleShowtimeCommand(...) : ICommand;

// Combined — user must have BOTH
[Authorize(Roles = "Admin", Permissions = "scheduleShowtime:showtime")]
public sealed record ...

// Multiple attributes — user must satisfy ALL attributes
[Authorize(Roles = "Admin")]
[Authorize(Permissions = "cancel:showtime")]
public sealed record ...
```

When multiple `[Authorize]` attributes are present, all of them must be
satisfied (AND semantics). Within a single attribute, all declared roles
and permissions must be satisfied (also AND semantics).

---

## ⚙️ Execution flow

### Full request lifecycle

```
HTTP Request
    │
    ▼
ExecutionContextMiddleware
    ├── Resolves Actor (JWT → user:{guid})
    ├── Validates x-tenant-id → ScopeContext
    └── ValidateScopeAsync ──────────────────────────────────────────┐
           Checks UserAuthorizationReadModel exists for              │
           (userId, tenantId, partitionId?, cinemaId?)              │
           → 403 if user has no assignment in this scope            │
    │                                                               │
    ▼                                                               │
Sets ExecutionContext (AsyncLocal)                                   │
    │                                                               │
    ▼                                                               │
MediatR Pipeline                                                    │
    │                                                               │
    ├── ValidationBehavior (FluentValidation)                       │
    │                                                               │
    ├── AuthorizationBehavior ──────────────────────────────────────┘
    │       RequestAuthorizationService.TryAuthorizeAsync
    │         ├── Reads [Authorize] attributes from command/query
    │         └── AuthorizationService.AuthorizeAsync
    │               ├── Loads UserAuthorizationReadModel (cached)
    │               ├── Checks required permissions ⊆ user permissions
    │               ├── Checks required roles ⊆ user roles
    │               └── (TODO) Evaluates policies
    │
    └── CommandHandler / QueryHandler
```

### Step 1 — Scope validation (middleware)

`ValidateScopeAsync` verifies that the user has **any assignment** in the
requested scope. It does not check roles or permissions — it only answers:
"Does this user belong to this tenant/partition/cinema at all?"

```csharp
// AuthorizationService.ValidateScopeAsync
var userAuthorization = await repository.SingleOrDefaultAsync(
    new GetUserAuthorizationCachedSpecification(query), ct);

if (userAuthorization is null)
    return Result.NotFound(...);

return Result.Success();
```

This is the **gate** — if the user is not in the scope, the request is
rejected at 403 before the command even reaches the pipeline.

### Step 2 — Command/query authorization (pipeline behavior)

`AuthorizationBehavior` runs for every command and query. If the request
has no `[Authorize]` attribute, it passes through immediately (public
within-scope action).

For decorated requests:

```csharp
// RequestAuthorizationService
var authorizationAttributes = request.GetType()
    .GetCustomAttributes<AuthorizeAttribute>().ToList();

if (authorizationAttributes.Count == 0)
    return Result.Success();   // ← no authorization required

var requiredPermissions = authorizationAttributes
    .SelectMany(a => a.Permissions?.Split(',') ?? [])
    .ToList().AsReadOnly();

var requiredRoles = authorizationAttributes
    .SelectMany(a => a.Roles?.Split(',') ?? [])
    .ToList().AsReadOnly();

return await authorizationService.AuthorizeAsync(
    requiredRoles, requiredPermissions, requiredPolicies, ct);
```

`AuthorizationService.AuthorizeAsync` then:
1. Reads `ExecutionContext` from `IExecutionContextService`.
2. Skips authorization for non-User actors (service-to-service calls
   are trusted at this layer — see corner cases).
3. Loads `UserAuthorizationReadModel` from the cached read model.
4. Performs set-difference checks:

```csharp
// Any required permission not in the user's set → Forbidden
if (requiredPermissions.Except(userAuthorization.Permissions
    .Select(p => p.Value)).Any())
    return Result.Forbidden(...);

// Any required role not in the user's set → Forbidden
if (requiredRoles.Except(userAuthorization.Roles
    .Select(r => r.Value)).Any())
    return Result.Forbidden(...);
```

### Step 3 — Read model: `UserAuthorizationReadModel`

The authorization check reads from a **projected read model**, not from
the `User` aggregate directly. This read model is maintained by projectors
that react to `User` domain events (`UserGrantedEvent`,
`RoleAssignedToScopeEvent`, etc.) via the KurrentDB subscription.

```
User aggregate (KurrentDB)
    │  UserGrantedEvent
    │  RoleAssignedToScopeEvent
    │  PermissionAssignedToScopeEvent
    ▼
UserAuthorizationProjector
    ▼
UserAuthorizationReadModel (PostgreSQL, cached in Redis)
    ▼
AuthorizationService.AuthorizeAsync
```

The read model is cached using a specification cache key:
```
GetUserAuthorizationCachedSpecification_{userId}_{tenantId}_{partitionId}_{cinemaId}
```

This means authorization checks are served from Redis after the first
lookup, making them extremely fast for subsequent requests.

---

## 🏗️ Managing user access

### Grant a user access to a scope with roles and permissions

```csharp
user.GrantUser(
    scope: Scope.Create(tenantId, partitionId, cinemaId),
    roles: [new Role(Roles.Admin)],
    permissions: [new Permission(ShowtimePermissions.Cancel)]);
```

This emits `UserGrantedEvent` (new scope) or `RoleAssignedToScopeEvent` /
`PermissionAssignedToScopeEvent` (existing scope).

### Revoke all access for a scope

```csharp
user.RevokeUser(scope);
// Emits: UserRevokedEvent → removes the entire Assignment
```

### Add a single role to an existing scope

```csharp
user.AssignRole(scope, new Role(Roles.Viewer));
// Emits: RoleAssignedToScopeEvent
```

### Remove a single permission

```csharp
user.RemovePermission(scope, new Permission(ShowtimePermissions.Cancel));
// Emits: PermissionRevokedFromScopeEvent
```

---

## 🚫 Design decisions

### ❌ No `Microsoft.AspNetCore.Authorization`

ASP.NET Core's authorization is HTTP-bound and policy-centric. It cannot
natively express multi-tenant scope hierarchies. The custom
`IAuthorizationService` lives in the application layer and is callable
from any handler — not just controllers.

### ❌ No role/permission checks in `ValidateScopeAsync`

Scope validation is intentionally coarse. It answers only "does this user
belong here?" — not "what can they do?". This separation keeps the
middleware fast and allows the pipeline behavior to enforce fine-grained
access per operation.

### ✅ Read model for authorization, not aggregate

Loading the full `User` aggregate for every authorization check would
require replaying potentially hundreds of events. The `UserAuthorizationReadModel`
is a pre-projected, cached snapshot of exactly the data needed for access
control — scoped roles and permissions.

### ✅ Cache key includes full scope

The cache key encodes `(userId, tenantId, partitionId, cinemaId)`. This
means a user operating in different scopes gets independent cache entries —
no risk of leaking permissions across scope boundaries.

### ✅ Service-to-service calls bypass user authorization

When `ActorType != User`, `AuthorizeAsync` returns success unconditionally.
Internal services are trusted at this level — they are already authenticated
via service identity, and their actions are auditable through the
`ExecutionContext` (see corner cases for risks).

### ✅ Permissions and roles use AND semantics

All declared requirements must be satisfied. There is no OR semantics
within a single `[Authorize]` attribute. For OR scenarios, the recommended
approach is to declare the widest permission that covers the operation,
and let the domain enforce finer rules.

---

## 🧠 Mental model

```
"Can the user ACCESS this scope?"          ValidateScopeAsync    (middleware)
       ↓ yes
"Can the user PERFORM this operation?"     AuthorizeAsync        (pipeline)
       ↓ yes
"Is the operation VALID?"                  Domain rules          (aggregate)
```

Three layers, three concerns, three places to fail — each independently
testable and independently evolvable.

```
ExecutionContext
    └── ScopeContext (tenantId, partitionId?, domainId?)
              │
              ▼
    UserAuthorizationReadModel
              │
    ┌─────────┴──────────┐
    │  Roles[]           │  ← coarse-grained: Admin, Viewer
    │  Permissions[]     │  ← fine-grained: cancel:showtime
    └────────────────────┘
              │
    [Authorize] attribute on command/query
              │
    AuthorizationService.AuthorizeAsync
```

---

## ⚠️ Corner cases

### 1. Scope specificity and overlapping assignments

A user can have assignments at multiple scope levels. `Scope.Matches` uses
the **narrowest defined level** of the assignment — not the request.

```
User has:
  Assignment A: Tenant=T1             → roles: [Viewer]
  Assignment B: Tenant=T1, Cinema=C1 → roles: [Admin]

Request scope: Tenant=T1, Cinema=C1

GetRolesFor(tenantId=T1, partitionId=null, cinemaId=C1):
  Assignment A: CinemaId=null, PartitionId=null → matches on TenantId=T1 ✅
  Assignment B: CinemaId=C1 → matches on CinemaId=C1 ✅

Result: [Viewer, Admin]  ← BOTH assignments match
```

`GetRolesFor` and `GetPermissionsFor` use `.Where(a => a.Scope.Matches(...))`
which returns **all matching assignments**, not just the most specific.
A user inherits permissions from broader scopes. This is additive.

### 2. Cache staleness after authorization changes

When a user's roles or permissions change (e.g. `RoleAssignedToScopeEvent`
is processed), the cache entry for that user+scope must be invalidated.
The projector that updates `UserAuthorizationReadModel` must also evict
the Redis cache for the affected key.

> If cache invalidation is not implemented in the projector, users will
> see stale authorization data until the cache TTL expires. This is a
> known risk with async event-sourced projections and cached read models.

### 3. Service-to-service authorization is implicit

When `ActorType != User`, `AuthorizeAsync` returns `Result.Success()`
unconditionally. There is no service permission model yet. A compromised
internal service could perform any action.

Mitigation in the current design: every service action is recorded in
KurrentDB with the full `ExecutionContext` (including `ActorId` of the
service), making it auditable after the fact.

### 4. `ValidateScopeAsync` is skipped for `External` actors

The middleware skips `ValidateScopeAsync` for `ActorType.External`. External
actors (unauthenticated requests like registration) use `SystemTenantId`
as a fallback and bypass scope validation. Any endpoint that allows
`[AllowAnonymous]` and performs meaningful business operations must enforce
its own authorization rules at the command level.

### 5. Policy enforcement is not yet implemented

`[Authorize(Policies = "...")]` is parsed and forwarded to
`AuthorizationService.AuthorizeAsync`, but the evaluation loop is commented
out (TODO). Declaring a policy on a command currently has no effect.
Do not rely on policies for access control until the enforcement is complete.

### 6. `GetUserActorId()` throws for non-User actors

The helper on `ExecutionContext` throws `InvalidOperationException` if called
when `ActorType != User`. Any code path that may handle multiple actor types
must guard with `if (actorType == ActorType.User)` before calling it.
`AuthorizationService` does this correctly.

### 7. Read model reflects async projection — not the aggregate

Because `UserAuthorizationReadModel` is built from KurrentDB subscription
events, there is an inherent propagation delay between a domain change
(e.g. `GrantUser`) and the authorization check reflecting it. In practice
this is milliseconds, but in tests or during catch-up subscription
processing, a user may be authorized at the aggregate level but not yet
authorized at the read model level.

---

## 🧪 Full examples

### Example 1 — User cancels a showtime

```
Preconditions:
  User U1 has Assignment:
    Scope: Tenant=T1, Cinema=C1
    Roles: [Admin]
    Permissions: [cancel:showtime]

Request:
  POST /api/v2/showtimes/{id}/cancel
  Authorization: Bearer {jwt, oid=U1}
  x-tenant-id: T1
  x-domain-id: C1

ExecutionContextMiddleware:
  1. ResolveActor → user:U1, ActorType.User
  2. ResolveTenant → T1
  3. ValidateScopeAsync(U1, T1, null, C1)
     → UserAuthorizationReadModel found ✅
  4. SetExecutionContext(...)

MediatR Pipeline:
  5. AuthorizationBehavior
     → [Authorize(Permissions = "cancel:showtime")]
     → AuthorizeAsync
         requiredPermissions: ["cancel:showtime"]
         userAuthorization.Permissions: ["cancel:showtime"]
         difference: [] → ✅ Authorized

  6. CancelShowtimeCommandHandler executes
```

### Example 2 — Viewer tries to cancel (forbidden)

```
User U2 has Assignment:
  Scope: Tenant=T1
  Roles: [Viewer]
  Permissions: [getShowtime:showtime, getShowtimes:showtime]

Request: POST /api/v2/showtimes/{id}/cancel

AuthorizeAsync:
  requiredPermissions: ["cancel:showtime"]
  userAuthorization.Permissions: ["getShowtime:showtime", "getShowtimes:showtime"]
  difference: ["cancel:showtime"] → not empty → Result.Forbidden ❌
```

### Example 3 — User accesses wrong tenant (forbidden at scope)

```
User U3 has Assignment only for Tenant=T2

Request:
  x-tenant-id: T1   (different tenant)

ValidateScopeAsync(U3, T1, null, null):
  → UserAuthorizationReadModel not found for T1
  → Result.NotFound → 403 Forbidden ❌
  (never reaches the pipeline)
```

### Example 4 — Tenant-level Admin accesses any cinema

```
User U4 has Assignment:
  Scope: Tenant=T1 (no partition, no cinema)
  Roles: [Admin]
  Permissions: [cancel:showtime, scheduleShowtime:showtime, ...]

Request:
  x-tenant-id: T1
  x-domain-id: C5  (any cinema in T1)

ValidateScopeAsync(U4, T1, null, C5):
  Specification: userId=U4, tenantId=T1, partitionId=null, cinemaId=C5
  Query: WHERE partitionId IS NULL AND cinemaId = C5
  → No row found because the assignment has cinemaId=null ⚠️
```

> **This is a potential gap**: `ValidateScopeAsync` uses the specification
> `GetUserAuthorizationCachedSpecification` which filters on the exact
> `(userId, tenantId, partitionId, cinemaId)` tuple. A tenant-level
> assignment (cinemaId=null) will not match a request with a specific
> cinemaId. The read model projector must handle this by either:
> - Storing one row per scope level (tenant-only, partition-only, cinema-specific), or
> - Making `ValidateScopeAsync` perform a fallback hierarchy query
>   (check cinema → then partition → then tenant).
>
> Verify your projector and specification handle this correctly.

---

## 📋 Authorization attribute reference

```csharp
// Permission only
[Authorize(Permissions = "cancel:showtime")]

// Role only
[Authorize(Roles = "Admin")]

// Both (AND — user must have both)
[Authorize(Roles = "Admin", Permissions = "scheduleShowtime:showtime")]

// Multiple attributes (AND — user must satisfy all)
[Authorize(Roles = "Admin")]
[Authorize(Permissions = "cancel:showtime")]

// Policy (not yet enforced)
[Authorize(Policies = "OwnerOnly")]

// No attribute = accessible to any user within the scope
public sealed record GetShowtimesQuery(...) : IQuery<...>;