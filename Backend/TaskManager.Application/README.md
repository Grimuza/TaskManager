# TaskManager.Application

Application layer containing business logic, services, and data transfer objects.

## Structure

```
TaskManager.Application/
├── DTOs/
│   ├── TaskItemDto.cs           # Task response DTO
│   ├── CreateTaskDto.cs         # Create task request
│   ├── UpdateTaskDto.cs         # Update task request
│   ├── ToggleTaskDto.cs         # Toggle completion request
│   └── ReorderTaskDto.cs        # Reorder request
├── Interfaces/
│   └── ITaskService.cs          # Service contract
└── Services/
    └── TaskService.cs           # Business logic implementation
```

## DTOs (Data Transfer Objects)

### TaskItemDto.cs
Response DTO for task data.
```csharp
public class TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
```

### CreateTaskDto.cs
Request DTO for creating tasks.
```csharp
public class CreateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    // Order is auto-assigned by service
}
```

### UpdateTaskDto.cs
Request DTO for updating tasks.
```csharp
public class UpdateTaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}
```

### ReorderTaskDto.cs
Request DTO for bulk reordering.
```csharp
public class ReorderTaskDto
{
    public Guid Id { get; set; }
    public int Order { get; set; }
}
```

## Services

### TaskService.cs

Contains all business logic for task operations.

**Key Methods**:

1. **GetAllTasksAsync()** - Returns all tasks ordered by Order field

2. **GetTaskByIdAsync(id)** - Retrieves single task

3. **CreateTaskAsync(dto)** - Creates new task
   - Automatically assigns order (max + 1)
   - Sets CreatedAt timestamp
   - Validates input

4. **UpdateTaskAsync(id, dto)** - Updates existing task
   - Updates all fields
   - Handles completion timestamp logic

5. **ToggleTaskCompletionAsync(id)** - Toggles completion
   - Flips IsCompleted boolean
   - Sets/clears CompletedAt timestamp

6. **ReorderTasksAsync(tasks)** - Bulk reorder
   - Updates order for multiple tasks
   - Atomic operation

7. **DeleteTaskAsync(id)** - Removes task

**Business Rules Implemented**:
- New tasks get auto-incrementing order
- CompletedAt is set when task is marked complete
- CompletedAt is cleared when task is unmarked
- Order must be explicitly updated via reorder endpoint

## Interfaces

### ITaskService.cs

Service contract defining all business operations.

```csharp
public interface ITaskService
{
    Task<IEnumerable<TaskItemDto>> GetAllTasksAsync();
    Task<TaskItemDto?> GetTaskByIdAsync(Guid id);
    Task<TaskItemDto> CreateTaskAsync(CreateTaskDto createTaskDto);
    Task<TaskItemDto> UpdateTaskAsync(Guid id, UpdateTaskDto updateTaskDto);
    Task<TaskItemDto> ToggleTaskCompletionAsync(Guid id);
    Task ReorderTasksAsync(List<ReorderTaskDto> reorderTasks);
    Task<bool> DeleteTaskAsync(Guid id);
}
```

## Dependencies

- TaskManager.Domain (project reference)

**Why DTOs?**
- Separates internal entities from API contracts
- Allows different views of data
- Prevents over-posting attacks
- Enables API versioning
- Simplifies serialization

## Design Patterns

### Service Layer Pattern
- Encapsulates business logic
- Sits between API and data access
- Orchestrates operations
- Handles transactions

### DTO Pattern
- Data transfer objects for API boundaries
- Mapping between entities and DTOs
- Clean separation of concerns

## Notes

- All operations are async
- Uses repository pattern for data access
- No direct database access
- Maps entities to DTOs
- Contains validation logic
- Handles business rules

