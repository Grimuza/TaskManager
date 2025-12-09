/**
 * Task priority levels
 */
export enum TaskPriority {
  Low = 0,     // Green
  Medium = 1,  // Yellow
  High = 2     // Red
}

/**
 * Task item type definition
 */
export interface Task {
  id: string;
  title: string;
  description: string;
  isCompleted: boolean;
  order: number;
  priority: TaskPriority;
  createdAt: string;
  completedAt?: string;
}

/**
 * Create task payload type
 */
export interface CreateTask {
  title: string;
  description: string;
  priority: TaskPriority;
}

/**
 * Reorder task payload type
 */
export interface ReorderTask {
  id: string;
  order: number;
}

/**
 * Sort options
 */
export type SortBy = 'created' | 'userDefined' | 'priority';
export type SortOrder = 'asc' | 'desc';

