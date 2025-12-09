using TaskManager.Application.DTOs;

namespace TaskManager.Application.Interfaces;

/// <summary>
/// Service interface for task operations
/// </summary>
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

