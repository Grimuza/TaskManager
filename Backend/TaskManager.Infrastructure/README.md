# TaskManager.Infrastructure

Infrastructure layer implementing data access and external dependencies.

## Structure

```
TaskManager.Infrastructure/
└── Data/
    ├── Context/
    │   └── TaskDbContext.cs          # EF Core DbContext
    └── Repositories/
        └── TaskRepository.cs          # Repository implementation
```

## Components

### Data/Context/TaskDbContext.cs

Entity Framework Core database context.

**Configuration**:
- In-Memory database provider
- Entity configurations for TaskItem
- Validation rules and constraints

**Entity Configuration**:
```csharp
- Title: Required, max 200 characters
- Description: Optional, max 1000 characters
- IsCompleted: Required boolean
- Order: Required integer for custom ordering
- CreatedAt: Required DateTime
- CompletedAt: Optional DateTime
```

### Data/Repositories/TaskRepository.cs

Implementation of `ITaskRepository` interface.

**Methods**:
- `GetAllAsync()` - Returns tasks ordered by Order field
- `GetByIdAsync(id)` - Find task by ID
- `CreateAsync(task)` - Add new task
- `UpdateAsync(task)` - Update existing task
- `DeleteAsync(id)` - Remove task

**Features**:
- Async/await pattern throughout
- Ordered results by default
- Proper null handling

## Dependencies

- Microsoft.EntityFrameworkCore 9.0.0
- Microsoft.EntityFrameworkCore.InMemory 9.0.0
- TaskManager.Domain (project reference)
- TaskManager.Application (project reference)

## Database

Uses EF Core In-Memory database:
- No connection string required
- Data persists during app lifetime
- Resets on restart
- Perfect for development and demos

### Production Migration

To use a real database:

1. Install EF Core provider:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
# or
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

2. Update Program.cs:
```csharp
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(connectionString));
```

3. Add migrations:
```bash
dotnet ef migrations add Initial
dotnet ef database update
```

## Design Decisions

### Separation of Concerns

- **Context folder**: Database configuration and entity setup
- **Repositories folder**: Data access implementation

This separation allows:
- Easy unit testing
- Clear boundaries
- Simple maintenance

### Repository Pattern

Benefits:
- Abstracted data access
- Easy to mock for testing
- Can swap implementations
- Consistent interface

### Ordering Strategy

Tasks are ordered by the `Order` field by default:
```csharp
return await _context.Tasks.OrderBy(t => t.Order).ToListAsync();
```

This ensures:
- Consistent ordering
- Fast retrieval
- No sorting needed at API layer

## Usage in API

Registered in Program.cs:
```csharp
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseInMemoryDatabase("TaskManagerDb"));

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
```

## Notes

- All operations are asynchronous
- Uses Task<T> return types
- Follows repository pattern
- Clean separation from business logic
- No business rules in repository (that's Application layer's job)

