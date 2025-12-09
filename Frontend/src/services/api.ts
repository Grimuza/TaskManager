import type { Task, CreateTask, ReorderTask } from '../types/task';

const API_URL = 'http://localhost:5000/api/tasks';

/**
 * API service for task management operations
 */
export const taskApi = {
  /**
   * Fetch all tasks from the server
   */
  async getAllTasks(): Promise<Task[]> {
    const response = await fetch(API_URL);
    if (!response.ok) throw new Error('Failed to fetch tasks');
    return response.json();
  },

  /**
   * Create a new task
   */
  async createTask(task: CreateTask): Promise<Task> {
    const response = await fetch(API_URL, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(task),
    });
    if (!response.ok) throw new Error('Failed to create task');
    return response.json();
  },

  /**
   * Toggle task completion status
   */
  async toggleTask(id: string): Promise<Task> {
    const response = await fetch(`${API_URL}/${id}/toggle`, {
      method: 'PATCH',
    });
    if (!response.ok) throw new Error('Failed to toggle task');
    return response.json();
  },

  /**
   * Update a task
   */
  async updateTask(id: string, updateData: { title: string; description: string; isCompleted: boolean; priority: number }): Promise<Task> {
    const response = await fetch(`${API_URL}/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(updateData),
    });
    if (!response.ok) throw new Error('Failed to update task');
    return response.json();
  },

  /**
   * Reorder tasks
   */
  async reorderTasks(tasks: ReorderTask[]): Promise<void> {
    const response = await fetch(`${API_URL}/reorder`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(tasks),
    });
    if (!response.ok) throw new Error('Failed to reorder tasks');
  },

  /**
   * Delete a task
   */
  async deleteTask(id: string): Promise<void> {
    const response = await fetch(`${API_URL}/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok) throw new Error('Failed to delete task');
  },
};

