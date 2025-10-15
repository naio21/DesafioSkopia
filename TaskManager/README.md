# TaskManager

## Overview
TaskManager is a .NET 8 RESTful WebAPI for managing projects and tasks in a collaborative team environment. It enables users to organize, monitor, and report on daily tasks and project progress.

---

## Features
- **Project Management**
  - List all user projects
  - Create new projects
  - Restrict project removal if pending tasks exist
- **Task Management**
  - View all tasks for a specific project
  - Add new tasks to a project (max 20 per project)
  - Update task status/details (with change history)
  - Remove tasks from a project
  - Assign priorities (low, medium, high; cannot be changed after creation)
  - Add comments to tasks (stored in change history)
- **Change History**
  - Every task update is logged with field changed, date/time, and user
  - Comments are included in history
- **Performance Reports**
  - Manager-only endpoint for average tasks completed per user in last 30 days
- **Testing**
  - xUnit, Moq, and in-memory database
  - At least 80% code coverage for business rules
- **Docker Support**
  - Ready for containerized deployment

---

## Business Rules
- **Task Priorities:** Assigned at creation, cannot be changed
- **Project Removal:** Not allowed if pending tasks exist
- **Change History:** All updates and comments are logged
- **Task Limit:** Max 20 tasks per project
- **Performance Reports:** Only accessible by users with manager role
- **UserId:** Passed as `X-User-Id` header in every request
- **Users:** Fixed set, with boolean `IsManager` field

---

## API Endpoints
- `GET /api/projects` — List all user projects
- `POST /api/projects` — Create a new project
- `GET /api/projects/{projectId}/tasks` — List tasks for a project
- `POST /api/projects/{projectId}/tasks` — Add a new task
- `PUT /api/tasks/{taskId}` — Update a task
- `DELETE /api/tasks/{taskId}` — Remove a task
- `GET /api/tasks/{taskId}/history` — View task change history
- `POST /api/tasks/{taskId}/comments` — Add a comment to a task
- `GET /api/reports/performance` — Manager-only performance report

---

## Setup & Running
### Local
1. Run the SQL script in `Database/Scripts/01_InitialSchema.sql` to create the database and tables
2. Update `appsettings.json` with your LocalDb connection string if needed
3. Run the API:
   ```sh
   dotnet run --project TaskManager/TaskManager.csproj
   ```
4. Access Swagger UI at `http://localhost:<port>/swagger`

### Docker
1. Build the Docker image:
   ```sh
   docker build -t taskmanager-api ./TaskManager
   ```
2. Run the container:
   ```sh
   docker run -p 5243:80 taskmanager-api
   ```

---

## Database Scripts
- All database objects and seed data are in `Database/Scripts/01_InitialSchema.sql`

---

## Testing & Coverage
- Run tests:
  ```sh
  dotnet test TaskManager.Tests/TaskManager.Tests.csproj
  ```
- Coverage is collected with `coverlet.collector` (see test project)
- Tests use xUnit, Moq, and in-memory database

---

## Phase 2: Refinement Questions
- What additional user roles or permissions are needed?
- Should task assignment to specific users be supported?
- Is notification or reminder functionality required for due dates?
- Should project archiving or soft-delete be implemented?
- Are there requirements for file attachments or richer comments?
- Should API support pagination/filtering for large lists?
- Any integration with external authentication/authorization?

---

## Phase 3: Improvements & Vision
- Implement authentication/authorization (e.g., JWT, Identity)
- Add support for user management and invitations
- Improve error handling and validation (FluentValidation)
- Add pagination, sorting, and filtering to endpoints
- Enhance reporting (custom date ranges, more metrics)
- Adopt CQRS and MediatR for scalable architecture
- Add OpenAPI documentation and examples
- Prepare for cloud deployment (Azure SQL, App Service)
- Add CI/CD pipeline for automated testing and deployment
- Consider microservices for scaling task/project/report modules

---

## License
MIT
