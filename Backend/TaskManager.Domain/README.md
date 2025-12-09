# TaskManager.Domain

Core domain layer containing business entities and repository interfaces.

## Structure

```
TaskManager.Domain/
├── Entities/
│   └── TaskItem.cs              # Task entity
└── Interfaces/
    └── ITaskRepository.cs       # Repository contract
```

## Entities

### TaskItem.cs

Core business entity representing a task.

**Properties**:
```csharp
public class TaskItem
{
    public Guid Id { get; set; }                    // Unique identifier
    public string Title { get; set; }               // Required
    public string Description { get; set; }         // Optional
    public bool IsCompleted { get; set; }           // Completion status
    public int Order { get; set; }                  // Custom ordering
    public DateTime CreatedAt { get; set; }         // Creation timestamp
    public DateTime? CompletedAt { get; set; }      // Completion timestamp
}
```

**Business Rules**:
- Title is required
- Order must be unique per task
- CompletedAt is set only when IsCompleted is true
- CreatedAt is set on creation

## Interfaces

### ITaskRepository.cs

Repository pattern interface for data access abstraction.

**Methods**:
```csharp
public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(Guid id);
}
```

**Purpose**:
- Abstracts data access from business logic
- Enables dependency inversion
- Facilitates unit testing with mocks
- Allows multiple implementations (InMemory, SQL, etc.)

## Design Principles

### Clean Architecture

The Domain layer:
- ✅ Has NO dependencies on other layers
- ✅ Contains only business entities and interfaces
- ✅ Defines contracts (interfaces) that others implement
- ✅ Is the center of the architecture

### Dependency Rule

```
Domain (no dependencies)
   ↑
Application (depends on Domain)
   ↑
Infrastructure (depends on Application + Domain)
   ↑
API (depends on all)
```

## Notes

- This layer should never reference:
  - Entity Framework
  - ASP.NET Core
  - Any external libraries
  - Other project layers

- Contains pure C# code
- Framework-agnostic
- Represents business concepts
- Defines the core domain model

