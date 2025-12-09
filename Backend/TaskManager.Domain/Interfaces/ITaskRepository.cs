using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

/// <summary>
/// Repository interface for TaskItem operations
/// </summary>
public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task<TaskItem> CreateAsync(TaskItem task);
    Task<TaskItem> UpdateAsync(TaskItem task);
    Task<bool> DeleteAsync(Guid id);
}

