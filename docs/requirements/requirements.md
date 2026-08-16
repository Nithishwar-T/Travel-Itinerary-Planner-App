# Requirements — Travel Itinerary Planner

## 1. Actors

| Actor          | Description                                                                                                                                                                                     |
| -------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Traveler       | Registered user who creates trips, builds itineraries, invites collaborators, and views budgets. Can only access their own trips and trips they've been invited to.                             |
| ContentCurator | Manages the destination/POI catalog. Cannot access traveler trip data.                                                                                                                          |
| Admin          | Has system-level access for user and destination management and can view all trips for support and troubleshooting. Admin trip access is read-only unless explicitly acting on a user's behalf. |

---

## 2. User Stories

### Traveler

- As a Traveler, I want to register and log in, so that I can access my trips securely.
- As a Traveler, I want to create a trip with start/end dates and destinations, so that I can start planning.
- As a Traveler, I want to add activities to specific days in my itinerary, so that I have a day-wise plan.
- As a Traveler, I want to reorder activities within a day, so that I can adjust my schedule.
- As a Traveler, I want to invite co-travelers by email, so that we can plan together.
- As a Traveler, I want to set edit or view-only permission for invited collaborators, so that I control who can change the plan.
- As a Traveler, I want to see a budget summary of my trip, so that I know estimated costs.
- As a Traveler, I want to see only my own trips and trips shared with me, so that other travelers' plans stay private.
- As a Traveler, I want to browse the destination catalog, so that I can pick places for my trip.
- As a Traveler, I want to add and manage bookings for my trip, so that I can track travel-related costs and include them in my overall budget.

### ContentCurator

- As a ContentCurator, I want to log in to a dedicated area, so that I can manage destinations without seeing traveler data.
- As a ContentCurator, I want to create, edit, and delete destinations and points of interest, so that the catalog stays accurate.
- As a ContentCurator, I want my access blocked from trip and booking data, so that I only operate within my scope.

### Admin

- As an Admin, I want to view and manage all users, so that I can handle account issues.
- As an Admin, I want full CRUD access to destinations, so that I can override or fix curator mistakes.
- As an Admin, I want to view all trips across all travelers, so that I can support and troubleshoot issues.
- As an Admin, I want read access to traveler trip and booking information for support purposes, without modifying it unless explicitly acting on a user's behalf.

---

## 3. Functional Requirements

### Authentication & Authorization

- FR1: System shall allow user registration with email + password (hashed via BCrypt).
- FR2: System shall issue a JWT access token + refresh token on successful login.
- FR3: System shall support token refresh via a dedicated endpoint.
- FR4: System shall reject unauthenticated requests to protected endpoints with 401.
- FR5: System shall reject role-mismatched requests with 403.
- FR6: JWT shall embed the user's role as a claim.

### Trip Management

- FR7: Traveler shall be able to create, view, update, and delete their own trips.
- FR8: Trip shall have start date, end date, and one or more destinations.
- FR9: System shall prevent a Traveler from accessing another Traveler's trip unless invited as a collaborator.

### Itinerary Management

- FR10: Traveler shall be able to add day-wise itinerary entries to a trip.
- FR11: Traveler shall be able to add, edit, remove, and reorder activities within a day.
- FR12: Each activity shall have a name, time, destination/POI reference, and cost.

### Collaboration

- FR13: Trip owner shall be able to invite a collaborator by email.
- FR14: System shall support two permission levels for collaborators: View, Edit.
- FR15: Invited collaborator shall be able to accept/view the trip once registered or logged in.

### Destination/POI Management

- FR16: ContentCurator and Admin shall be able to create, update, and delete destinations/POIs.
- FR17: Traveler shall have read-only access to the destination catalog.

### Budget

- FR18: System shall calculate a budget summary by aggregating costs from all activities and bookings in a trip.

### Admin

- FR19: Admin shall be able to view all users.
- FR20: Admin shall be able to view all trips (read access) for support purposes.
- FR21: Admin shall have full CRUD access to destinations.

### Bookings

- FR22: Traveler shall be able to add a booking (e.g. flight, hotel, transport) to their trip with an associated cost.
- FR23: Traveler shall be able to view, edit, and delete their own bookings within a trip.
- FR24: Bookings shall contribute to the trip's overall budget calculation alongside activity costs.

---

## 4. Non-Functional Requirements

- NFR1: All API responses shall follow a consistent JSON error format (code, message).
- NFR2: All incoming DTOs shall be validated server-side using FluentValidation, in addition to client-side Angular Reactive Forms validation.
- NFR3: Passwords shall never be stored or logged in plain text.
- NFR4: JWT secrets and connection strings shall never be committed to source control (use dotnet user-secrets locally).
- NFR5: API shall return standard HTTP status codes (200/201/400/401/403/404/409) consistently.
- NFR6: System is designed for demonstration/portfolio scale — no specific load/performance targets required, but queries should use proper indexing on foreign keys.
- NFR7: A global exception-handling middleware shall catch unhandled exceptions and return a consistent error response instead of leaking stack traces.
- NFR8: Business-rule validation (e.g. duplicate destination names, invalid date ranges where end date precedes start date, duplicate collaborator invites) shall be enforced at the service layer, separate from field-level DTO validation.

---

## 5. Out of Scope

- Real payment processing (booking cost is stored/estimated only, no payment gateway integration).
- Real email delivery for collaborator invites (can be simulated/logged for now; real email service is a stretch goal, not required).
- Multi-language/i18n support.
- Real-time collaborative editing (e.g. live cursors) — collaborators edit independently, not simultaneously in real time.
- Mobile app — web only, responsive Angular UI.
