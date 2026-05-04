# Work Request Tracker

A full-stack work request management application built with **ASP.NET Core 8** (backend) and **Next.js 14** (frontend). Supports creating, filtering, searching, and tracking work requests with status updates and notes.
<img width="1470" height="956" alt="Screenshot 2026-05-04 at 4 43 56 PM" src="https://github.com/user-attachments/assets/949c4c3a-c59e-4a7c-8d72-89a7b87db408" />

## Tech Stack

| Layer      | Technology                          |
|------------|-------------------------------------|
| Backend    | ASP.NET Core 8 Web API              |
| Frontend   | Next.js 14 (App Router, TypeScript) |
| Database   | SQLite (dev) / SQL Server (prod)    |
| ORM        | Entity Framework Core 8             |
| Styling    | Tailwind CSS                        |

## Project Structure

```
workRequestTracker/
├── backend/                     # ASP.NET Core backend
│   ├── API/                     # Controllers, middleware, filters
│   ├── Application/             # Services, DTOs, interfaces
│   ├── Domain/                  # Entities, enums
│   ├── Infrastructure/          # EF Core, repositories, migrations
│   └── Program.cs               # App entry point
│
├── frontend/                    # Next.js frontend
│   ├── app/                     # Pages (App Router)
│   ├── components/              # Reusable UI components
│   ├── services/                # API client
│   └── types/                   # TypeScript type definitions
│
└── ARCHITECTURE.md              # Architecture decisions & tradeoffs
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- npm (comes with Node.js)

## Getting Started

### 1. Clone the repository

```bash
git clone <repo-url>
cd workRequestTracker
```

### 2. Backend setup

```bash
cd backend

# Restore NuGet packages
dotnet restore

# Apply database migrations (creates workrequests.db with seed data)
dotnet ef database update

# Run the API server
dotnet run
```

The API will start at `http://localhost:5074`. Swagger UI is available at `http://localhost:5074/swagger` in development mode.

### 3. Frontend setup

```bash
cd frontend

# Install dependencies
npm install

# Create environment file (if not present)
echo "NEXT_PUBLIC_API_URL=http://localhost:5074" > .env.local

# Run the dev server
npm run dev
```

The frontend will start at `http://localhost:3000`.

## API Endpoints

| Method | Endpoint                             | Description                    |
|--------|--------------------------------------|--------------------------------|
| GET    | `/api/work-requests`                 | List all (paginated, filterable) |
| GET    | `/api/work-requests/{id}`            | Get details with notes         |
| POST   | `/api/work-requests`                 | Create a new work request      |
| PATCH  | `/api/work-requests/{id}/status`     | Update status                  |
| POST   | `/api/work-requests/{id}/notes`      | Add a note                     |

### Query Parameters (GET list)

| Parameter  | Type   | Default | Description                         |
|------------|--------|---------|-------------------------------------|
| `status`   | string | —       | Filter by status (New, InProgress, Blocked, Completed) |
| `search`   | string | —       | Search by title or client name      |
| `page`     | int    | 1       | Page number                         |
| `pageSize` | int    | 10      | Items per page                      |

### Response Format

All endpoints return a consistent response shape:

```json
{
  "success": true,
  "message": "Work request created.",
  "data": { },
  "pagination": {
    "page": 1,
    "pageSize": 10,
    "totalCount": 47,
    "totalPages": 5
  }
}
```

## Architecture

The backend follows a **layered architecture** with strict dependency rules:

- **Domain** — Entities and enums. No external dependencies.
- **Application** — Business logic, DTOs, validation, mapping. Depends only on Domain.
- **Infrastructure** — EF Core DbContext and repositories. Implements Application interfaces.
- **API** — Controllers and middleware. Thin layer that delegates to Application services.

See [ARCHITECTURE.md](ARCHITECTURE.md) for detailed design decisions and tradeoffs.

## Features

- Paginated work request list with server-side filtering and search
- Create new work requests with validation (title, client, priority, status, due date)
- Update work request status
- Add notes to work requests (append-only)
- Consistent error handling with `ExceptionMiddleware`
- Responsive dark-themed UI

## Database

SQLite is used for development — no additional setup required. The database file (`workrequests.db`) is auto-created when migrations run. The repository interface abstracts all data access, so switching to SQL Server or PostgreSQL requires only a configuration change.

### Running Migrations

```bash
cd backend

# Add a new migration
dotnet ef migrations add <MigrationName>

# Apply migrations
dotnet ef database update
```

## Environment Variables

### Backend (`backend/appsettings.json`)

| Key                                  | Default                        |
|--------------------------------------|--------------------------------|
| `ConnectionStrings:DefaultConnection`| `Data Source=workrequests.db`   |

### Frontend (`frontend/.env.local`)

| Key                    | Default                    |
|------------------------|----------------------------|
| `NEXT_PUBLIC_API_URL`  | `http://localhost:5074`    |

## License

This project is for educational/demonstration purposes.
