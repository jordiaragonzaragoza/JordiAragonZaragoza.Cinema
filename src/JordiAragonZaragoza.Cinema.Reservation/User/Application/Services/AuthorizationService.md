# 🔐 Authorization System

## 📌 Purpose

The authorization system controls **what actors can do** within a specific business scope. It answers two distinct questions:

| Question | Mechanism |
| --- | --- |
| **Can this user access this scope?** | `ValidateScopeAsync` — scope membership check |
| **Can this user perform this action?** | `AuthorizeAsync` — roles, permissions, and policies check |

The system is intentionally **custom-built** — it does not use ASP.NET Core's `Microsoft.AspNetCore.Authorization` or `IAuthorizationService`. This keeps the authorization logic in the application layer, decoupled from the HTTP infrastructure, isolated for testing, and aware of the multi-tenant business context that native ASP.NET Core policies cannot natively express.

---

## 🧩 Key Concepts

### 1. User Aggregate

The `User` aggregate is the source of truth for authorization data. It is persisted in KurrentDB as an event-sourced aggregate. Its state is the accumulated result of all authorization-related events.

```
User
└── Assignments[]
      └── Assignment
            ├── Scope (TenantId, PartitionId?, CinemaId?)
            ├── Roles[]       e.g., "Admin", "Viewer"
            └── Permissions[] e.g., "cancel:showtime", "reserveSeats:showtime"

```

A user has **zero or more Assignments**. Each Assignment binds a set of roles and permissions to a specific Scope. A user can have different roles in different scopes — for example, `Admin` at the Tenant level but also `Viewer` for a specific cinema within that same tenant.

---

### 2. Scope — Hierarchical Access Levels

`Scope` is a value object representing the business context where an assignment applies. It has three levels, from broadest to narrowest:

```
Tenant  (company / organization)
  └── Partition  (regional subdivision or area)
        └── Cinema  (specific domain instance)

```

The `Scope.Matches` method (in the domain) applies a **specificity rule**: the narrowest scope level defined in the assignment is the one that prevails.

```csharp
public bool Matches(CinemaId? cinemaId, PartitionId? partitionId, TenantId tenantId)
{
    if (this.CinemaId is not null)    return this.CinemaId == cinemaId;
    if (this.PartitionId is not null) return this.PartitionId == partitionId;
    return this.TenantId == tenantId;
}

```

The read model `UserAuthorizationReadModel` replicates the exact same rule with `Matches`, ensuring that specificity logic is identical both in memory (aggregate) and in the projected read layer:

```csharp
public bool Matches(Guid tenantId, Guid? partitionId, Guid? domainId)
{
    if (this.CinemaId is not null)    return this.CinemaId == domainId;
    if (this.PartitionId is not null) return this.PartitionId == partitionId;
    return this.TenantId == tenantId;
}

```

This means:

* An assignment at the **Tenant** level grants access to all partitions and cinemas within that tenant.
* An assignment at the **Partition** level only applies to that specific partition.
* An assignment at the **Cinema** level only applies to that specific cinema.

#### Scope Matching Examples

| Assignment Scope | Request Scope | Matches? |
| --- | --- | --- |
| Tenant=A | Tenant=A, Partition=any, Cinema=any | ✅ |
| Tenant=A, Partition=P1 | Tenant=A, Partition=P1, Cinema=any | ✅ |
| Tenant=A, Partition=P1 | Tenant=A, Partition=P2 | ❌ |
| Tenant=A, Cinema=C1 | Tenant=A, Partition=any, Cinema=C1 | ✅ |
| Tenant=A, Cinema=C1 | Tenant=A, Cinema=C2 | ❌ |

---

### 3. Assignment

An `Assignment` binds a `Scope` to a set of `Role`s and `Permission`s. It is owned by the `User` aggregate.

Business rules applied at the domain level:

* It must have **at least one role or permission** (`AssignmentMustHaveAtLeastOneRoleOrPermissionRule`).
* A role can only be assigned **once per scope** (`OnlyPossibleToAssignRoleOnceRule`).
* A permission can only be assigned **once per scope** (`OnlyPossibleToAssignPermissionOnceRule`).
* `GrantUser` on an existing scope must introduce **at least one new role or permission** (`GrantUserMustIntroduceNewRolesOrPermissionsRule`).

---

### 4. Role

A role is a named grouping that semantically describes an access level. Roles are flat text strings wrapped in a value object.

Current roles defined in `Roles` (Application.Contracts):

| Role | Intended Meaning |
| --- | --- |
| `Admin` | Full management access |
| `Viewer` | Read-only access |

> 📐 **Design Decision**: `Roles` (Application.Contracts) and `Role` (Domain) are intentionally decoupled. `Roles` is the public constants class that any bounded context uses to build `[Authorize(Roles = Roles.Admin)]`. `Role` is the internal value object of the `User` aggregate. If `Roles` referenced the domain `Role`, any consumer of `[Authorize]` would drag a transitive dependency to the `User` domain — inverting the dependency direction of Clean Architecture. Consistency between both is guaranteed via an architecture test.

---

### 5. Permission

A permission is a fine-grained capability text string. The convention is:

```
{action}:{resource}

```

Examples of `ShowtimePermissions`:

| Permission | Meaning |
| --- | --- |
| `cancel:showtime` | Cancel a scheduled showtime |
| `reserveSeats:showtime` | Reserve seats for a showtime |
| `scheduleShowtime:showtime` | Create a new showtime |
| `getAvailableSeats:showtime` | Query available seats |

---

### 6. Policy

Policies are named authorization rules that go beyond static role and permission checks — they encode ABAC (Attribute-Based Access Control) logic such as "you can only cancel a reservation if you are the owner or an administrator".

**Current Status:** Implemented and operational. Evaluation is performed in `AuthorizationService.AuthorizeAsync` → `IPolicyEnforcer` → `IAuthorizationPolicy`.

Available policies:

| Constant | Meaning |
| --- | --- |
| `Policies.SelfOrAdmin` | Passes if the user is the owner of the resource (`resourceId`) OR has the `Admin` role in the active scope |

#### How `SelfOrAdmin` Works

1. If the user has the `Admin` role → **passes** immediately (without querying the resource).
2. If not an Admin and no `resourceId` is provided → **Forbidden** (fail-closed, never fail-open).
3. If not an Admin and there is a `resourceId` → loads the `ReservationReadModel` and compares `reservation.UserId == currentUserId`.

> ⚠️ **Guaranteed Fail-closed**: A policy that cannot be evaluated due to missing data always returns `Forbidden`, never `Success`. A policy declared in `[Authorize]` but without a registered implementation throws an `InvalidOperationException` at startup, preventing the risk of a silent no-op.

#### How to Declare a Policy in a Command

The command implements `IPolicyResourceRequest` to expose the `resourceId` that the policy needs to resolve ownership — without exposing the full shape of the command to the authorization system:

```csharp
[Authorize(Permissions = ShowtimePermisions.Cancel, Policies = Policies.SelfOrAdmin)]
public sealed record CancelReservationCommand(
    Guid ShowtimeId,
    Guid ReservationId) : ICommand, IPolicyResourceRequest
{
    // Only the resource ID crosses into the enforcer — no further details.
    public Guid ResourceId => this.ReservationId;
}

```

#### Adding a New Policy

1. Add the constant in `Policies` (Application.Common).
2. Implement `IAuthorizationPolicy` in the application layer of the corresponding bounded context.
3. Register with `services.AddScoped<IAuthorizationPolicy, MyNewPolicy>()`.
4. The `PolicyEnforcer` automatically discovers it via DI.

---

### 7. `[Authorize]` Attribute

It is applied to commands and queries to declare their authorization requirements:

```csharp
// Permission only
[Authorize(Permissions = "cancel:showtime")]
public sealed record CancelShowtimeCommand(Guid ShowtimeId) : ICommand;

// Role only
[Authorize(Roles = "Admin")]
public sealed record ScheduleShowtimeCommand(...) : ICommand;

// Combined (AND — user must meet BOTH)
[Authorize(Roles = "Admin", Permissions = "scheduleShowtime:showtime")]
public sealed record ...

// Multiple attributes (AND — user must meet ALL)
[Authorize(Roles = "Admin")]
[Authorize(Permissions = "cancel:showtime")]
public sealed record ...

// With ownership policy
[Authorize(Permissions = ShowtimePermisions.Cancel, Policies = Policies.SelfOrAdmin)]
public sealed record CancelReservationCommand(...) : ICommand, IPolicyResourceRequest { ... }

// Without attribute = accessible to any authorized actor within the scope
public sealed record GetShowtimesQuery(...) : IQuery<...>;

```

AND semantics in all cases: all attributes, all roles declared within an attribute, all permissions declared, and all policies must be satisfied.

---

## ⚙️ Execution Flow

### Complete Request Lifecycle

```
HTTP Request
    │
    ▼
ExecutionContextMiddleware
    ├── Resolves Actor based on type:
    │     User     → JWT with valid 'oid' claim
    │     External → [AllowAnonymous] without JWT
    │     null     → 401 Unauthorized
    ├── Validates x-tenant-id → ScopeContext
    │     User without tenant → 400 Bad Request
    │     External without tenant → SystemTenantId as fallback
    └── ValidateScopeAsync (only for ActorType.User)
           Hierarchical resolution: Is there any Assignment covering the scope?
           → 403 Forbidden if none exists
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
              ├── Reads [Authorize] from the command/query
              ├── Extracts resourceId if implementing IPolicyResourceRequest
              └── AuthorizationService.AuthorizeAsync
                    ├── ActorType.User  → AuthorizeUserAsync
                    │     ├── Resolves UserAuthorizationReadModel (hierarchical)
                    │     ├── requiredPermissions ⊆ user permissions
                    │     ├── requiredRoles ⊆ user roles
                    │     └── For each policy → IPolicyEnforcer → IAuthorizationPolicy
                    ├── ActorType.System → Result.Success() (trusted, audited)
                    └── ActorType.External → Result.Forbidden() (configuration error)
    │
    ▼
CommandHandler / QueryHandler

```

### Authorization Behavior by Actor Type

| ActorType | `ValidateScopeAsync` | `AuthorizeAsync` | Reason |
| --- | --- | --- | --- |
| `User` | ✅ Executed | ✅ Full (roles + permissions + policies) | Authenticated actor with system identity |
| `System` | ❌ N/A (bypasses HTTP middleware) | `Result.Success()` | Trusted internal system, action audited in KurrentDB |
| `External` | ❌ Omitted in middleware | `Result.Forbidden()` — config error | Should never reach a command decorated with `[Authorize]` |

> **`ActorType.External` in `AuthorizeAsync**`: If an `External` actor reaches a command decorated with `[Authorize]`, the system returns `Forbidden` instead of `Success` — this indicates an endpoint configuration error (it should either be behind authentication or not have `[Authorize]`). This is a deliberate fail-closed behavior: `External` must never silently inherit permissions.

> **`ActorType.System` and Ownership Policies**: `System` actors bypass policy evaluation (including `SelfOrAdmin`). If a worker/consumer invokes a command with an ownership policy, that policy is not evaluated — `System` is trusted by design. If the integration flow needs to respect ownership, propagate the original user's `ActorId` in the integration event so the consumer can reconstruct the context as `ActorType.User`.

---

### Step 1 — Scope Validation (Middleware)

`ValidateScopeAsync` verifies that the user has **some assignment that hierarchically covers** the requested scope. It does not check roles or permissions — it only answers: "Does this user belong to this tenant/partition/cinema?"

### Step 2 — Command/Query Authorization (Pipeline)

`AuthorizationService.AuthorizeAsync` delegates to `AuthorizeUserAsync` for `User` actors, executing sequentially: permissions → roles → policies. A failure at any step returns immediately without evaluating subsequent ones.

### Step 3 — Read Model: `UserAuthorizationReadModel`

```
User Aggregate (KurrentDB)
    │  UserGrantedEvent, RoleAssignedToScopeEvent, PermissionAssignedToScopeEvent, ...
    ▼
Projectors (UserGrantedEventProjector, RoleAssignedToScopeEventProjector, ...)
    ▼
UserAuthorizationReadModel — one row per Scope level (PostgreSQL + Redis)
    ▼
UserAuthorizationResolver.ResolveAsync
    (GetUserAuthorizationCandidatesSpecification + Matches + specificity)
    ▼
AuthorizationService.AuthorizeAsync / ValidateScopeAsync

```

---

## 🔍 Hierarchical Scope Resolution in the Read Model

### The Problem (Resolved)

`GetUserAuthorizationCachedSpecification` filters using exact SQL equality, which prevents finding a Tenant-level Assignment when the request includes a specific `CinemaId`.

### The Solution: Two Specifications with Distinct Responsibilities

| Specification | Purpose | Used by |
| --- | --- | --- |
| `GetUserAuthorizationCachedSpecification` | **Exact** identity — the specific row referenced by a domain event | Projectors (Write side) |
| `GetUserAuthorizationCandidatesSpecification` | All rows for the user in a tenant; hierarchical resolution using `Matches` in memory | `UserAuthorizationResolver` (Read side) |

`UserAuthorizationResolver` encapsulates the hierarchical resolution and adds an in-memory per-request cache (the `last` field) to avoid querying the read model twice between `ValidateScopeAsync` (middleware) and `AuthorizeAsync` (pipeline):

```csharp
public async Task<UserAuthorizationReadModel?> ResolveAsync(
    Guid userId, ScopeContext scope, CancellationToken cancellationToken)
{
    if (this.last is { } cached && cached.UserId == userId && cached.Scope == scope)
        return cached.Result;  // ← avoids second query to Redis within the same request

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

> `UserAuthorizationResolver` must be registered as **Scoped** so that the in-memory cache is per-request and does not leak across concurrent requests.

---

## 🏗️ User Access Management

```csharp
// Grant access with roles and permissions
user.GrantUser(
    scope: Scope.Create(tenantId, partitionId, cinemaId),
    roles: [new Role(Roles.Admin)],
    permissions: [new Permission(ShowtimePermissions.Cancel)]);

// Revoke all access for a scope
user.RevokeUser(scope);

// Add a single role to an existing scope
user.AssignRole(scope, new Role(Roles.Viewer));

// Remove a single permission
user.RemovePermission(scope, new Permission(ShowtimePermissions.Cancel));

```

---

## 🗄️ Read Model Cache

`BaseCachedSpecificationRepository` invalidates **all** cache entries under the `UserAuthorizationReadModel` prefix on every `AddAsync`/`UpdateAsync`/`DeleteAsync` operation, automatically for all projectors. Manual cache key invalidation is not required in any projector.

The `UserAuthorizationResolver` adds a second layer of **in-memory per-request** caching (the `last` field), eliminating duplicate Redis queries per HTTP request.

---

## 🚫 Design Decisions

### ❌ Without `Microsoft.AspNetCore.Authorization`

ASP.NET Core authorization is bound to HTTP and heavily centered around policies. It cannot natively express multi-tenant scope hierarchies.

### ❌ No Role/Permission Checks in `ValidateScopeAsync`

Scope validation solely answers "does this user belong here?" — not "what can they do?". This separation keeps the middleware fast.

### ✅ Read Model for Authorization, Not the Aggregate

The `UserAuthorizationReadModel` is a projection containing exactly the required data: roles and permissions per scope level, cached in Redis.

### ✅ One Row per Scope Level, Hierarchical Resolution at Query Time

Hierarchical resolution (`Matches`) is applied when reading, not during projection. This prevents behavioral divergence between the domain and the read model.

### ✅ `IPolicyEnforcer` Decoupled from the Request Shape

The enforcer receives only `(policy, currentUserId, currentUserRoles, currentUserPermissions, resourceId?)` — never the full command/query object. This keeps the authorization system independent of presentation layer contracts.

### ✅ `IAuthorizationPolicy` by Strategy, Discovered via DI

Each policy is an independent class registered in DI. `PolicyEnforcer` discovers them by name at runtime. Adding a new policy does not require modifying `PolicyEnforcer`.

### ✅ Policies Live in the Application Layer, Not the Domain

Ownership-type policies need to query a repository (read model) to resolve the resource owner — this is application layer orchestration, not a domain aggregate invariant. The domain remains completely unaware of roles, permissions, and authorization policies.

### ✅ `ActorType.External` in `AuthorizeAsync` → Explicit Forbidden

External actors should only reach `[AllowAnonymous]` endpoints that do not declare `[Authorize]`. If they reach `AuthorizeAsync`, it is a configuration error that must fail loudly (Forbidden), not silently (Success).

### ✅ `ActorType.System` → Success Without Policy Evaluation

System workers and consumers are trusted because their identity and actions are fully audited via `ExecutionContext` in `EventStoreMetadata`. If an integration flow needs to enforce ownership, it must propagate the original user's `ActorId` in the integration event.

### ✅ `Roles` (Application.Contracts) and `Role` (Domain) Decoupled

See §4. Consistency is enforced via an architecture test.

### ❌ No Pagination in Authorization Read Models

A user's set of Assignments within a tenant is small (1-5 rows) and highly repetitive — ideal for non-paginated caching. Other read models with large datasets use pagination without caching.

---

## 🧠 Mental Model

```
"Can the user ACCESS this scope?"               ValidateScopeAsync    (middleware)
       ↓ yes
"Can the user PERFORM this operation?"          AuthorizeAsync        (pipeline)
       ↓ roles + permissions ok
"Does it satisfy ownership/ABAC POLICIES?"      IPolicyEnforcer       (pipeline)
       ↓ yes
"Is the operation VALID?"                        Domain Rules         (aggregate)

```

```
ExecutionContext.ScopeContext
    │
    ▼
UserAuthorizationResolver.ResolveAsync
    ├── GetUserAuthorizationCandidatesSpecification (Redis → PostgreSQL)
    ├── .Where(Matches) + OrderByDescending(specificity)
    └── UserAuthorizationReadModel (the most specific matching the scope)
              │
    ┌─────────┴──────────┐
    │  Roles[]           │  ← coarse-grained: Admin, Viewer
    │  Permissions[]     │  ← fine-grained: cancel:showtime
    └────────────────────┘
              │
    [Authorize] on command/query
              │
    AuthorizationService.AuthorizeAsync
              │
    IPolicyEnforcer (if Policies are declared)
              │
    IAuthorizationPolicy.EvaluateAsync
         └── Loads ReservationReadModel to resolve ownership

```

---

## ⚠️ Corner Cases

### 1. Scope Specificity and Overlapping Assignments

Resolution in `UserAuthorizationResolver` is **exclusive**: it selects only the most specific Assignment. This differs from the `User.GetRolesFor` aggregate behavior, which is **additive** (a union of all matching assignments). This difference is deliberate — see design decisions §1.

```
The user has:
  Assignment A: Tenant=T1             → roles: [Viewer]
  Assignment B: Tenant=T1, Cinema=C1  → roles: [Admin]

AuthorizationService for Tenant=T1, Cinema=C1:
  → Only Assignment B (Cinema, more specific) → [Admin]
  (does not inherit [Viewer] from Assignment A)

```

### 2. Cache Invalidation — Verified, Not an Issue

`BaseCachedSpecificationRepository` invalidates by type prefix on every write operation. All revocation projectors automatically route through this base repository.

### 3. `ActorType.System` Bypasses Ownership Policies

If a worker invokes a command decorated with `[Authorize(Policies = "SelfOrAdmin")]`, the policy is not evaluated — `System` passes directly. To preserve ownership in integration flows, propagate the original user's `ActorId` in the integration event metadata.

### 4. `ActorType.External` and `[AllowAnonymous]`

The middleware skips `ValidateScopeAsync` for `External` actors. Any `[AllowAnonymous]` endpoint that mutates business state must apply its own authorization at the command level — it cannot rely on the role/permission system because `External` actors have no platform identity.

### 5. `IPolicyResourceRequest` Without an Active `[Authorize(Policies = ...)]`

A command can implement `IPolicyResourceRequest` without having an active policy (e.g., while the policy is in development/commented out). This breaks nothing — `resourceId` is simply ignored. However, it creates semantic noise: document the reason in the command if the policy is temporarily disabled.

### 6. Policy Registered in DI but Missing Constant in `Policies`

If an `IAuthorizationPolicy` is registered with a `Name` that does not appear in any `[Authorize(Policies = ...)]`, it becomes dead code — it will not cause errors, but it will never be invoked. This is detectable with an architecture test.

### 7. Asynchronous Read Model Propagation

`UserAuthorizationReadModel` is built from KurrentDB events. There is a delay between a domain change (`GrantUser`) and its reflection in authorization checks. In tests and during subscription catch-up, a user might be authorized in the aggregate but not yet in the read model.

---

## 🧪 Complete Examples

### Example 1 — User Cancels a Showtime (Permissions)

```
User U1: Assignment Tenant=T1, Cinema=C1 → [Admin] + [cancel:showtime]

POST /api/v2/showtimes/{id}/cancel
  x-tenant-id: T1, x-domain-id: C1

ValidateScopeAsync → Assignment(T1,null,C1).Matches(T1,null,C1) ✅
AuthorizeAsync:
  requiredPermissions: ["cancel:showtime"] ⊆ ["cancel:showtime"] ✅
  → CancelShowtimeCommandHandler executes

```

### Example 2 — User Cancels Their Reservation (SelfOrAdmin Policy)

```
User U1: Assignment Tenant=T1 → [Viewer] + [cancel:reservation]
Reservation R1: UserId = U1

POST /api/v2/reservations/{R1}/cancel
  [Authorize(Permissions = "cancel:reservation", Policies = "SelfOrAdmin")]

ValidateScopeAsync → Assignment(T1).Matches(T1) ✅
AuthorizeAsync:
  requiredPermissions: ["cancel:reservation"] ⊆ ["cancel:reservation"] ✅
  requiredRoles: [] → ok
  policy "SelfOrAdmin":
    currentUserRoles does not contain "Admin"
    resourceId = R1
    ReservationReadModel(R1).UserId == U1 ✅
  → Authorized ✅

```

### Example 3 — User Tries to Cancel Someone Else's Reservation (Policy Fails)

```
User U2: Assignment Tenant=T1 → [Viewer]
Reservation R1: UserId = U1 (not U2)

policy "SelfOrAdmin":
  currentUserRoles does not contain "Admin"
  ReservationReadModel(R1).UserId (U1) != currentUserId (U2)
  → Result.Forbidden ❌

```

### Example 4 — Admin Cancels Any User's Reservation

```
User U3: Assignment Tenant=T1 → [Admin]
Reservation R1: UserId = U1

policy "SelfOrAdmin":
  currentUserRoles.Contains("Admin") → Result.Success() ✅
  (ReservationReadModel is not queried)

```

### Example 5 — Tenant Admin Accesses a Specific Cinema (Hierarchy)

```
User U4: Assignment Tenant=T1 (PartitionId=null, CinemaId=null) → [Admin]

Request: x-tenant-id: T1, x-domain-id: C5

ValidateScopeAsync:
  Candidates: [Assignment(T1, null, null)]
  Matches(T1, null, C5): CinemaId=null → PartitionId=null → TenantId==T1 ✅
  → Found ✅

```

---

## 📋 Contracts Reference

```csharp
// Declare authorization on a command/query
[Authorize(Permissions = "cancel:showtime")]
[Authorize(Roles = "Admin")]
[Authorize(Policies = Policies.SelfOrAdmin)]

// Expose resourceId for ownership policies
public sealed record MyCommand(...) : ICommand, IPolicyResourceRequest
{
    public Guid ResourceId => this.ResourceIdField;
}

// Implement a new policy
public sealed class MyPolicy : IAuthorizationPolicy
{
    public string Name => Policies.MyNewPolicy;

    public async Task<Result> EvaluateAsync(
        Guid currentUserId,
        IReadOnlyCollection<string> currentUserRoles,
        IReadOnlyCollection<string> currentUserPermissions,
        Guid? resourceId,
        CancellationToken cancellationToken = default) { ... }
}

// Register the policy in DI
services.AddScoped<IAuthorizationPolicy, MyPolicy>();

```