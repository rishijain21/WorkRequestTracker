# Architecture Notes — Work Request Tracker

## Backend: Clean Architecture in ASP.NET Core 8

I structured the backend into four layers: **Domain**, **Application**, **Infrastructure**, and **API**. This isn't ceremonial — it enforces a dependency rule that pays off immediately. Domain entities have zero references to EF Core or HTTP concerns. The Application layer owns all business logic through `WorkRequestService`, which handles validation, enum parsing, and Entity-to-DTO mapping. The repository interface lives in Application; the implementation lives in Infrastructure. This means I can swap SQLite for SQL Server by changing one line in `Program.cs` and a NuGet package — no service or controller code changes.

The API layer is deliberately thin. Controllers do exactly one thing: call a service method and map the result to an HTTP status code. The `ExceptionMiddleware` sits first in the pipeline, catches anything unhandled, logs the real error, and returns a clean `ApiResponse` — no stack traces leak to the client.

## Frontend: Next.js 14, No State Library

I chose `useState` + `useEffect` over Redux or Zustand because this app has one page with one data source. The work requests page is the single state owner — it holds the list, filters, pagination, loading flag, and error state. Every child component is presentational: `WorkRequestTable` receives data as props, `WorkRequestFilters` reports changes upward via callbacks. Adding a state library here would be overengineering with no benefit.

The API service layer (`services/api.ts`) uses a shared `request<T>()` helper that handles both our custom `ApiResponse` errors and ASP.NET's `ProblemDetails` validation format. Components never call `fetch` directly.

## Database: SQLite for Dev

SQLite removes all setup friction — no Docker, no connection strings to configure, no server process. The `workrequests.db` file is self-contained and committed alongside the code. Because the repository interface abstracts all data access, migrating to SQL Server or PostgreSQL is a configuration change, not a refactor. The only SQLite-specific workaround was replacing `ToLower().Contains()` with `EF.Functions.Like()` and converting `OrderBy(DateTimeOffset)` to string comparison — both documented inline.

## Tradeoffs Under Constraint

I skipped unit tests, authentication, and request logging. The service layer validates enum values by string parsing rather than a dedicated validator. The 404-vs-400 distinction in the controller relies on message string matching rather than a typed error hierarchy. These are conscious tradeoffs for a time-boxed build — each has a clear upgrade path.

## Demo Strategy

I'd demo the full vertical slice first: create a work request, filter by status, update the status, show the table refresh. Then I'd open Swagger to show the raw API contract and the consistent `ApiResponse<T>` wrapper. Finally, I'd walk through the code — starting from the controller, through the service, into the repository — to demonstrate the clean separation.

## What I'd Add Next

Pagination with URL query sync (so the browser back button works), optimistic UI updates on status changes, a detail page with notes, proper error typing with status codes on `ApiResponse`, integration tests using `WebApplicationFactory`, and a CI pipeline with automated migration checks.

## Known Risks

SQLite's `LIKE` is case-insensitive for ASCII only — Unicode client names won't match correctly. The debounced search fires on every keystroke timer reset, which is fine at this scale but would need cancellation tokens for high-latency APIs. There's no auth — any client can hit any endpoint.
