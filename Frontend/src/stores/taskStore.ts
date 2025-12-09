import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { Task, CreateTask, ReorderTask, SortBy, SortOrder } from '../types/task';
import { taskApi } from '../services/api';

/**
 * Pinia store for task management
 */
export const useTaskStore = defineStore('tasks', () => {
  const tasks = ref<Task[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);
  const sortBy = ref<SortBy>('userDefined');
  const sortOrder = ref<SortOrder>('asc');

  /**
   * Fetch all tasks from the API
   */
  async function fetchTasks() {
    loading.value = true;
    error.value = null;
    try {
      tasks.value = await taskApi.getAllTasks();
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to fetch tasks';
    } finally {
      loading.value = false;
    }
  }

  /**
   * Create a new task
   */
  async function createTask(taskData: CreateTask) {
    loading.value = true;
    error.value = null;
    try {
      const newTask = await taskApi.createTask(taskData);
      tasks.value.push(newTask);
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to create task';
      throw e;
    } finally {
      loading.value = false;
    }
  }

  /**
   * Toggle task completion status
   */
  async function toggleTask(id: string) {
    try {
      const updatedTask = await taskApi.toggleTask(id);
      const index = tasks.value.findIndex(t => t.id === id);
      if (index !== -1) {
        tasks.value[index] = updatedTask;
      }
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to toggle task';
      throw e;
    }
  }

  /**
   * Update a task
   */
  async function updateTask(id: string, updateData: { title: string; description: string; isCompleted: boolean; priority: number }) {
    try {
      const updatedTask = await taskApi.updateTask(id, updateData);
      const index = tasks.value.findIndex(t => t.id === id);
      if (index !== -1) {
        tasks.value[index] = updatedTask;
      }
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to update task';
      throw e;
    }
  }

  /**
   * Reorder tasks locally and sync with server
   */
  async function reorderTasks(reorderedTasks: Task[]) {
    const oldTasks = [...tasks.value];
    tasks.value = reorderedTasks;
    
    try {
      const reorderPayload: ReorderTask[] = reorderedTasks.map((task, index) => ({
        id: task.id,
        order: index + 1
      }));
      
      await taskApi.reorderTasks(reorderPayload);
      
      // Update local state with new orders
      tasks.value = reorderedTasks.map((task, index) => ({
        ...task,
        order: index + 1
      }));
    } catch (e) {
      tasks.value = oldTasks;
      error.value = e instanceof Error ? e.message : 'Failed to reorder tasks';
      throw e;
    }
  }

  /**
   * Delete a task
   */
  async function deleteTask(id: string) {
    try {
      await taskApi.deleteTask(id);
      tasks.value = tasks.value.filter(t => t.id !== id);
    } catch (e) {
      error.value = e instanceof Error ? e.message : 'Failed to delete task';
      throw e;
    }
  }

  /**
   * Get pending (not completed) tasks
   */
  const pendingTasks = computed(() => {
    return tasks.value.filter(t => !t.isCompleted);
  });

  /**
   * Get completed tasks
   */
  const completedTasks = computed(() => {
    return tasks.value.filter(t => t.isCompleted);
  });

  /**
   * Get sorted pending tasks
   */
  const sortedPendingTasks = computed(() => {
    const pending = [...pendingTasks.value];
    
    if (sortBy.value === 'userDefined') {
      return pending.sort((a, b) => a.order - b.order);
    }
    
    if (sortBy.value === 'created') {
      return pending.sort((a, b) => {
        const dateA = new Date(a.createdAt).getTime();
        const dateB = new Date(b.createdAt).getTime();
        return sortOrder.value === 'asc' ? dateA - dateB : dateB - dateA;
      });
    }
    
    if (sortBy.value === 'priority') {
      return pending.sort((a, b) => {
        return sortOrder.value === 'asc' 
          ? a.priority - b.priority 
          : b.priority - a.priority;
      });
    }
    
    return pending;
  });

  /**
   * Update sort settings
   */
  function updateSort(newSortBy: SortBy, newSortOrder: SortOrder) {
    sortBy.value = newSortBy;
    sortOrder.value = newSortOrder;
  }

  return {
    tasks,
    loading,
    error,
    sortBy,
    sortOrder,
    pendingTasks,
    completedTasks,
    sortedPendingTasks,
    fetchTasks,
    createTask,
    toggleTask,
    updateTask,
    reorderTasks,
    deleteTask,
    updateSort,
  };
});

