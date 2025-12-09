using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

/// <summary>
/// Controller for task management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Get all tasks
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    /// <summary>
    /// Get task by id
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItemDto>> GetById(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();
        return Ok(task);
    }

    /// <summary>
    /// Create a new task
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> Create(CreateTaskDto createTaskDto)
    {
        var task = await _taskService.CreateTaskAsync(createTaskDto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    /// <summary>
    /// Update an existing task
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItemDto>> Update(Guid id, UpdateTaskDto updateTaskDto)
    {
        try
        {
            var task = await _taskService.UpdateTaskAsync(id, updateTaskDto);
            return Ok(task);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Toggle task completion status
    /// </summary>
    [HttpPatch("{id}/toggle")]
    public async Task<ActionResult<TaskItemDto>> ToggleCompletion(Guid id)
    {
        try
        {
            var task = await _taskService.ToggleTaskCompletionAsync(id);
            return Ok(task);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Reorder tasks
    /// </summary>
    [HttpPut("reorder")]
    public async Task<ActionResult> Reorder(List<ReorderTaskDto> reorderTasks)
    {
        await _taskService.ReorderTasksAsync(reorderTasks);
        return NoContent();
    }

    /// <summary>
    /// Delete a task
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _taskService.DeleteTaskAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}

