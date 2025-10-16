# TaskManager

# Solution Documentation

## Database Configuration

The application uses **SQL Server LocalDb**.  
Connection string is defined in `appsettings.json`: "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=TaskManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true" }

**Database Schema:**  
All tables and seed data are created by running the script:
- `Database/Scripts/01_InitialSchema.sql`

To set up the database:
1. Open SQL Server Management Studio or use `sqlcmd`.
2. Execute the script to create tables and insert default users.

---

## Libraries

The solution uses the following main libraries (see `.csproj` for full details):

- **Dapper**: Lightweight ORM for data access
- **Microsoft.Data.SqlClient**: SQL Server database provider
- **FluentValidation.AspNetCore**: Model validation
- **Swashbuckle.AspNetCore**: Swagger/OpenAPI documentation
- **xUnit**: Unit testing framework
- **Moq**: Mocking for unit tests
- **Microsoft.Data.Sqlite**: In-memory database for tests
- **coverlet.collector**: Code coverage for tests

---

## Running the Application on Docker

### 1. Build the Docker Image

Open a terminal in the solution root and run: docker build -t taskmanager-api ./TaskManager

### 2. Run the Docker Container

docker run -p 5243:80 taskmanager-api

- The API will be available at `http://localhost:5243`
- Swagger UI: `http://localhost:5243/swagger`

---

## Additional Notes

- Ensure the database is created and accessible before running the API.
- Pass the user ID in every request header as `X-User-Id`.
- For testing, run: dotnet test TaskManager.Tests/TaskManager.Tests.csproj
