# System Architecture — Travel Itinerary Planner

## 1. Layered Architecture (Backend)

Controller → Service → Repository → DbContext → SQL Server

| Layer               | Responsibility                                                                                                                         |
| ------------------- | -------------------------------------------------------------------------------------------------------------------------------------- |
| Controller          | Receives HTTP requests, validates model state, calls the Service, and returns HTTP responses. No business logic.                       |
| Service             | Contains business logic, orchestration, and authorization checks beyond role checks, such as verifying whether a Traveler owns a trip. |
| Repository          | Handles data access only — CRUD operations through DbContext. No business logic.                                                       |
| DbContext (EF Core) | ORM bridge between application entities and SQL Server.                                                                                |
| DTOs                | Define the data sent to and received from the API. Entities are never exposed directly.                                                |
| AutoMapper          | Maps Entity ↔ DTO.                                                                                                                     |
| Middleware          | Handles cross-cutting concerns such as JWT authentication, global exception handling, and logging.                                     |

**Architecture Rule:**

Controllers never communicate directly with Repositories.

Repositories never contain authorization or business logic.

The Service layer is responsible for business rules and orchestration.

This separation keeps each layer independently testable and maintainable.

## 3. Authentication & Token Flow

### Login Flow

Angular Login Form
│
▼
POST /api/auth/login
│
▼
AuthController
│
▼
AuthService
│
├── Find user by email
│
├── Verify password using BCrypt
│
├── Generate JWT Access Token
│
└── Generate Refresh Token
│
▼
Store Refresh Token in RefreshToken table
│
▼
Store tokens using httpOnly Secure cookies
│
▼
Return authentication success + user information
│
▼
Angular Application

### Access Token

- Access token is short-lived (approximately 15 minutes).
- The access token contains the user's identity and role claim.
- The access token is stored in an httpOnly cookie.
- JavaScript cannot directly read the httpOnly cookie.
- The browser automatically sends the cookie with requests.

### Subsequent API Requests

Angular Component
│
▼
Angular Service
│
▼
HttpClient
│
▼
Browser automatically sends httpOnly authentication cookie
│
▼
ASP.NET Core Authentication Middleware
│
├── Validate token signature
├── Check token expiration
└── Extract user claims
│
▼
Authorization
│
├── Correct role → Continue
│
└── Incorrect role → 403 Forbidden
│
▼
Controller
│
▼
Service
│
▼
Repository
│
▼
Database

### Token Refresh Flow

When the access token expires:

Angular
│
▼
API request returns 401 Unauthorized
│
▼
Angular Interceptor detects 401
│
▼
POST /api/auth/refresh
│
▼
AuthService
│
├── Validate refresh token
├── Check expiration
├── Check revocation status
└── Identify user
│
▼
Generate new access token
│
▼
Rotate refresh token
│
▼
Update RefreshToken table
│
▼
Set new httpOnly cookies
│
▼
Angular retries the original request

## 4. Token Storage Decision: httpOnly Cookie

**Decision:** JWT access tokens and refresh tokens will be stored in
httpOnly, Secure cookies rather than localStorage or JavaScript-accessible
memory.

### Why httpOnly Cookies?

- JavaScript cannot directly access an httpOnly cookie.
- This reduces the risk of JWT theft through XSS attacks.
- The browser automatically sends the cookie with requests.
- Tokens are not exposed through JavaScript or localStorage.
- This provides a stronger security posture for the application.

### Backend Implications

The ASP.NET Core backend will:

- Set the authentication cookie during login.
- Use `HttpOnly = true`.
- Use `Secure = true` in HTTPS environments.
- Use an appropriate `SameSite` policy.
- Never return the JWT access token in the normal login response body.
- Validate the JWT from the authentication cookie.
- Store refresh-token information server-side.
- Revoke and rotate refresh tokens when required.

Example cookie configuration:

    Response.Cookies.Append("access_token", token, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict
    });

### CORS Implications

Because the Angular frontend and ASP.NET Core API may run on different
origins during development:

- CORS must allow credentials.
- The backend must specify the exact frontend origin.
- `AllowAnyOrigin()` cannot be combined with `AllowCredentials()`.

Example:

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Frontend", policy =>
        {
            policy
                .WithOrigins("https://frontend.example.com")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

The exact development origin will be configured later.

### Angular Implications

Angular HTTP requests must include credentials so the browser sends
authentication cookies.

The application will use:

    withCredentials: true

The HttpInterceptor will **not** manually attach:

    Authorization: Bearer <token>

Instead, the browser automatically sends the authentication cookie.

The interceptor will primarily handle:

- 401 responses
- Token refresh
- Retrying the failed request
- Global HTTP error handling

### CSRF Protection

Because authentication uses cookies, Cross-Site Request Forgery (CSRF)
must also be considered.

The application will implement an appropriate CSRF protection strategy
during the authentication implementation phase.

The exact approach will be finalized in Phase 7.

### Refresh Token

The refresh token will also be protected using an httpOnly cookie.

The backend will:

1. Validate the refresh token.
2. Check whether it is expired.
3. Check whether it has been revoked.
4. Identify the associated user.
5. Generate a new access token.
6. Rotate the refresh token.
7. Update the stored refresh-token record.
8. Set the new cookies.

### Security Decision Summary

| Option            | Decision                       | Reason                                         |
| ----------------- | ------------------------------ | ---------------------------------------------- | --------------------------------------------- |
| localStorage      | ❌ Not used                    | JavaScript can access the token                |
| JavaScript memory | ❌ Not used as primary storage | Token is lost on refresh                       |
| httpOnly cookie   | ✅ Selected                    | Browser-managed and inaccessible to JavaScript | ## 4. Token Storage Decision: httpOnly Cookie |

**Decision:** JWT access tokens and refresh tokens will be stored in
httpOnly, Secure cookies rather than localStorage or JavaScript-accessible
memory.

### Why httpOnly Cookies?

- JavaScript cannot directly access an httpOnly cookie.
- This reduces the risk of JWT theft through XSS attacks.
- The browser automatically sends the cookie with requests.
- Tokens are not exposed through JavaScript or localStorage.
- This provides a stronger security posture for the application.

### Backend Implications

The ASP.NET Core backend will:

- Set the authentication cookie during login.
- Use `HttpOnly = true`.
- Use `Secure = true` in HTTPS environments.
- Use an appropriate `SameSite` policy.
- Never return the JWT access token in the normal login response body.
- Validate the JWT from the authentication cookie.
- Store refresh-token information server-side.
- Revoke and rotate refresh tokens when required.

Example cookie configuration:

    Response.Cookies.Append("access_token", token, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict
    });

### CORS Implications

Because the Angular frontend and ASP.NET Core API may run on different
origins during development:

- CORS must allow credentials.
- The backend must specify the exact frontend origin.
- `AllowAnyOrigin()` cannot be combined with `AllowCredentials()`.

Example:

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Frontend", policy =>
        {
            policy
                .WithOrigins("https://frontend.example.com")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });

The exact development origin will be configured later.

### Angular Implications

Angular HTTP requests must include credentials so the browser sends
authentication cookies.

The application will use:

    withCredentials: true

The HttpInterceptor will **not** manually attach:

    Authorization: Bearer <token>

Instead, the browser automatically sends the authentication cookie.

The interceptor will primarily handle:

- 401 responses
- Token refresh
- Retrying the failed request
- Global HTTP error handling

### CSRF Protection

Because authentication uses cookies, Cross-Site Request Forgery (CSRF)
must also be considered.

The application will implement an appropriate CSRF protection strategy
during the authentication implementation phase.

The exact approach will be finalized in Phase 7.

### Refresh Token

The refresh token will also be protected using an httpOnly cookie.

The backend will:

1. Validate the refresh token.
2. Check whether it is expired.
3. Check whether it has been revoked.
4. Identify the associated user.
5. Generate a new access token.
6. Rotate the refresh token.
7. Update the stored refresh-token record.
8. Set the new cookies.

### Security Decision Summary

| Option            | Decision                       | Reason                                         |
| ----------------- | ------------------------------ | ---------------------------------------------- |
| localStorage      | ❌ Not used                    | JavaScript can access the token                |
| JavaScript memory | ❌ Not used as primary storage | Token is lost on refresh                       |
| httpOnly cookie   | ✅ Selected                    | Browser-managed and inaccessible to JavaScript |

## 5. Global Error Handling

All unhandled exceptions shall be handled centrally by global exception
handling middleware.

### Error Flow

Exception occurs in any application layer
│
▼
Global Exception Handling Middleware
│
▼
Identify exception type
│
├── ValidationException
│ └── 400 Bad Request
│
├── UnauthorizedException
│ └── 401 Unauthorized
│
├── ForbiddenException
│ └── 403 Forbidden
│
├── NotFoundException
│ └── 404 Not Found
│
├── ConflictException
│ └── 409 Conflict
│
└── Unknown Exception
└── 500 Internal Server Error
│
▼
Return consistent JSON error response

## 6. Backend Solution Structure

The project will use a single ASP.NET Core Web API project:

TravelItineraryPlanner.Api

Separate Class Library projects such as API, Core, and Infrastructure will
not be created for this project.

The architectural separation will instead be maintained through folders
within the single Web API project.

### Backend Structure

TravelItineraryPlanner.Api/
│
├── Controllers/
│
├── Data/
│ └── Configurations/
│
├── Domain/
│ ├── Entities/
│ └── Enums/
│
├── DTOs/
│
├── Helpers/
│
├── Mapping/
│
├── Middleware/
│
├── Repositories/
│ ├── Interfaces/
│ └── Implementations/
│
├── Services/
│ ├── Interfaces/
│ └── Implementations/
│
├── Validators/
│
├── Program.cs
├── appsettings.json
└── TravelItineraryPlanner.Api.csproj

### Why a Single Project?

A single ASP.NET Core project is appropriate for the current scope because
this is a portfolio-scale project being developed by a single developer.

Using multiple Class Library projects would introduce additional project
and dependency-management complexity without providing significant benefits
for the current scope.

Folder-level separation still provides clear architectural boundaries
between:

- HTTP/API handling
- Business logic
- Data access
- Domain entities
- DTOs
- Validation
- Middleware
- Mapping

### Architectural Dependency Direction

The intended dependency flow is:

Controller
↓
Service
↓
Repository
↓
DbContext
↓
SQL Server

Supporting components:

DTOs → API data contracts
Validators → Input validation
AutoMapper → Entity/DTO mapping
Middleware → Cross-cutting concerns
Domain → Core business entities and enums

### Layer Responsibilities

| Component         | Responsibility                                            |
| ----------------- | --------------------------------------------------------- |
| Controllers       | HTTP endpoints and HTTP responses                         |
| Services          | Business logic and orchestration                          |
| Repositories      | Database access                                           |
| Domain            | Entities and enums                                        |
| DTOs              | API reques                                                |
| t/response models |
| Validators        | Input and business-related validation rules               |
| Mapping           | Entity ↔ DTO mapping                                      |
| Middleware        | Exception handling and other cross-cutting concerns       |
| Data              | EF Core configuration and database-related infrastructure |
