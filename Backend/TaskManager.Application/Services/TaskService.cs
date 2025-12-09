using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

/// <summary>
/// Service implementation for task operations
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<IEnumerable<TaskItemDto>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks.Select(MapToDto);
    }

    public async Task<TaskItemDto?> GetTaskByIdAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task != null ? MapToDto(task) : null;
    }

    public async Task<TaskItemDto> CreateTaskAsync(CreateTaskDto createTaskDto)
    {
        var allTasks = await _taskRepository.GetAllAsync();
        var maxOrder = allTasks.Any() ? allTasks.Max(t => t.Order) : 0;

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            IsCompleted = false,
            Order = maxOrder + 1,
            Priority = (TaskPriority)createTaskDto.Priority,
            CreatedAt = DateTime.UtcNow
        };

        var createdTask = await _taskRepository.CreateAsync(task);
        return MapToDto(createdTask);
    }

    public async Task<TaskItemDto> UpdateTaskAsync(Guid id, UpdateTaskDto updateTaskDto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            throw new KeyNotFoundException($"Task with id {id} not found");

        task.Title = updateTaskDto.Title;
        task.Description = updateTaskDto.Description;
        task.IsCompleted = updateTaskDto.IsCompleted;
        task.Priority = (TaskPriority)updateTaskDto.Priority;

        if (updateTaskDto.IsCompleted && !task.IsCompleted)
            task.CompletedAt = DateTime.UtcNow;
        else if (!updateTaskDto.IsCompleted)
            task.CompletedAt = null;

        var updatedTask = await _taskRepository.UpdateAsync(task);
        return MapToDto(updatedTask);
    }

    public async Task<TaskItemDto> ToggleTaskCompletionAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            throw new KeyNotFoundException($"Task with id {id} not found");

        task.IsCompleted = !task.IsCompleted;
        task.CompletedAt = task.IsCompleted ? DateTime.UtcNow : null;

        var updatedTask = await _taskRepository.UpdateAsync(task);
        return MapToDto(updatedTask);
    }

    public async Task ReorderTasksAsync(List<ReorderTaskDto> reorderTasks)
    {
        foreach (var reorderTask in reorderTasks)
        {
            var task = await _taskRepository.GetByIdAsync(reorderTask.Id);
            if (task != null)
            {
                task.Order = reorderTask.Order;
                await _taskRepository.UpdateAsync(task);
            }
        }
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        return await _taskRepository.DeleteAsync(id);
    }

    private static TaskItemDto MapToDto(TaskItem task)
    {
        return new TaskItemDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            Order = task.Order,
            Priority = (int)task.Priority,
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt
        };
    }
}

