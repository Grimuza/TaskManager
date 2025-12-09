# TaskManager API

RESTful API for task management built with ASP.NET Core 9.

## Quick Start

```bash
dotnet run
```

Or with hot reload:

```bash
dotnet watch run
```

API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:7221
- Swagger: http://localhost:5000/swagger

## API Endpoints

### Get All Tasks
```http
GET /api/tasks
```

**Response**: 200 OK
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Complete project",
    "description": "Finish the task manager application",
    "isCompleted": false,
    "createdAt": "2024-01-01T10:00:00Z",
    "completedAt": null
  }
]
```

### Get Task by ID
```http
GET /api/tasks/{id}
```

**Response**: 200 OK or 404 Not Found

### Create New Task
```http
POST /api/tasks
Content-Type: application/json

{
  "title": "New Task",
  "description": "Task description"
}
```

**Response**: 201 Created
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "New Task",
  "description": "Task description",
  "isCompleted": false,
  "createdAt": "2024-01-01T10:00:00Z",
  "completedAt": null
}
```

### Update Task
```http
PUT /api/tasks/{id}
Content-Type: application/json

{
  "title": "Updated Task",
  "description": "Updated description",
  "isCompleted": true
}
```

**Response**: 200 OK or 404 Not Found

### Toggle Task Completion
```http
PATCH /api/tasks/{id}/toggle
```

**Response**: 200 OK or 404 Not Found

This endpoint toggles the `isCompleted` status:
- If currently `false`, sets to `true` and sets `completedAt` timestamp
- If currently `true`, sets to `false` and clears `completedAt`

### Reorder Tasks
```http
PUT /api/tasks/reorder
Content-Type: application/json

[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "order": 1
  },
  {
    "id": "7b29a821-9c3d-4f12-a456-789012345678",
    "order": 2
  }
]
```

**Response**: 204 No Content

Bulk update task order. Used for drag & drop functionality.

### Delete Task
```http
DELETE /api/tasks/{id}
```

**Response**: 204 No Content or 404 Not Found

## Project Structure

```
TaskManager.API/
├── Controllers/
│   └── TasksController.cs    # API endpoints (includes reorder)
├── Properties/
│   └── launchSettings.json   # Launch configuration
├── appsettings.json          # Application settings
├── Program.cs                # Application startup and DI configuration
└── TaskManager.API.http      # REST client test file
```

## Configuration

### CORS

CORS is configured to allow requests from `http://localhost:5173` (Vue frontend).

To add more origins, modify `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://your-other-origin")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
```

### Database

The API uses EF Core In-Memory database configured in `Program.cs`:

```csharp
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseInMemoryDatabase("TaskManagerDb"));
```

**Note**: Data is lost when the application stops.

## Dependencies

The API layer depends on:
- TaskManager.Application (Business logic)
- TaskManager.Infrastructure (Data access)

All dependencies are automatically resolved through the project references and dependency injection.

## Error Handling

The API returns appropriate HTTP status codes:
- 200 OK - Successful GET, PUT, PATCH
- 201 Created - Successful POST
- 204 No Content - Successful DELETE
- 404 Not Found - Resource not found
- 400 Bad Request - Invalid request data

## Development

### Adding New Endpoints

1. Add method to `ITaskService` interface in Application layer
2. Implement method in `TaskService` class
3. Add controller action in `TasksController`
4. Test using Swagger UI

### Swagger Documentation

Swagger UI is available at `/swagger` in development mode.

API documentation is automatically generated from:
- XML comments in controllers
- Data annotations on DTOs
- Response type attributes

## Testing

### Manual Testing with Swagger

1. Run the API
2. Navigate to http://localhost:5000/swagger
3. Expand endpoints and click "Try it out"
4. Fill in parameters and execute

### Testing with HTTP Files

Use the `TaskManager.API.http` file with Visual Studio or REST Client extensions.

