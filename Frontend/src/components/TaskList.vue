<template>
  <div class="task-list">
    <div class="task-list-header">
      <h2>Pending Tasks</h2>
      <div class="header-actions">
        <SortSelector 
          :sort-by="taskStore.sortBy"
          :sort-order="taskStore.sortOrder"
          @update:sortBy="updateSortBy"
          @update:sortOrder="updateSortOrder"
        />
        <div class="task-stats">
          <span>Total: {{ taskStore.tasks.length }}</span>
          <span>Completed: {{ taskStore.completedTasks.length }}</span>
          <span>Pending: {{ taskStore.pendingTasks.length }}</span>
        </div>
      </div>
    </div>

    <div v-if="loading" class="loading">
      Loading tasks...
    </div>

    <div v-else-if="error" class="error">
      {{ error }}
    </div>

    <div v-else-if="displayedTasks.length === 0" class="empty-state">
      <p>No pending tasks! Add your first task above!</p>
    </div>

    <div v-else class="tasks-container">
      <TaskItem 
        v-for="(task, index) in displayedTasks" 
        :key="task.id" 
        :task="task"
        :draggable="taskStore.sortBy === 'userDefined'"
        @dragstart="handleDragStart($event, index)"
        @dragover="handleDragOver($event, index)"
        @drop="handleDrop($event, index)"
        @dragend="handleDragEnd"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import { useTaskStore } from '../stores/taskStore';
import TaskItem from './TaskItem.vue';
import SortSelector from './SortSelector.vue';
import type { SortBy, SortOrder } from '../types/task';

const taskStore = useTaskStore();

const loading = computed(() => taskStore.loading);
const error = computed(() => taskStore.error);

const draggedIndex = ref<number | null>(null);
const dragOverIndex = ref<number | null>(null);

/**
 * Display max 10 pending tasks
 */
const displayedTasks = computed(() => {
  return taskStore.sortedPendingTasks.slice(0, 10);
});

/**
 * Update sort by
 */
function updateSortBy(newSortBy: SortBy) {
  taskStore.updateSort(newSortBy, taskStore.sortOrder);
}

/**
 * Update sort order
 */
function updateSortOrder(newSortOrder: SortOrder) {
  taskStore.updateSort(taskStore.sortBy, newSortOrder);
}

/**
 * Handle drag start
 */
function handleDragStart(event: DragEvent, index: number) {
  draggedIndex.value = index;
  if (event.dataTransfer) {
    event.dataTransfer.effectAllowed = 'move';
  }
}

/**
 * Handle drag over
 */
function handleDragOver(event: DragEvent, index: number) {
  event.preventDefault();
  if (event.dataTransfer) {
    event.dataTransfer.dropEffect = 'move';
  }
  dragOverIndex.value = index;
}

/**
 * Handle drop
 */
async function handleDrop(event: DragEvent, dropIndex: number) {
  event.preventDefault();
  
  if (draggedIndex.value === null || draggedIndex.value === dropIndex) {
    return;
  }

  const reorderedTasks = [...displayedTasks.value];
  const [draggedTask] = reorderedTasks.splice(draggedIndex.value, 1);
  reorderedTasks.splice(dropIndex, 0, draggedTask);

  try {
    await taskStore.reorderTasks(reorderedTasks);
  } catch (error) {
    console.error('Failed to reorder tasks:', error);
  }
}

/**
 * Handle drag end
 */
function handleDragEnd() {
  draggedIndex.value = null;
  dragOverIndex.value = null;
}
</script>

<style scoped>
.task-list {
  background: var(--bg-card);
  padding: 32px;
  border-radius: 20px;
  box-shadow: 0 8px 32px var(--shadow);
  border: 1px solid var(--border-color);
  backdrop-filter: blur(10px);
  transition: all 0.3s ease;
}

.task-list-header {
  margin-bottom: 28px;
  padding-bottom: 20px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
}

h2 {
  margin: 0 0 16px 0;
  color: var(--text-primary);
  font-size: 1.4rem;
  font-weight: 600;
  letter-spacing: -0.3px;
  transition: color 0.3s ease;
}

.header-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 16px;
}

.task-stats {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
}

.task-stats span {
  color: var(--text-badge);
  font-size: 0.85rem;
  font-weight: 600;
  padding: 8px 16px;
  background: var(--bg-badge);
  border-radius: 10px;
  border: 1px solid var(--border-badge);
  backdrop-filter: blur(10px);
  transition: all 0.3s ease;
}

.task-stats span:hover {
  transform: translateY(-2px);
}

.tasks-container {
  display: flex;
  flex-direction: column;
  gap: 12px;
  max-height: 800px;
  overflow-y: auto;
  padding-right: 8px;
}

/* Custom scrollbar */
.tasks-container::-webkit-scrollbar {
  width: 8px;
}

.tasks-container::-webkit-scrollbar-track {
  background: var(--bg-input);
  border-radius: 10px;
}

.tasks-container::-webkit-scrollbar-thumb {
  background: rgba(0, 217, 163, 0.3);
  border-radius: 10px;
  transition: background 0.3s ease;
}

.tasks-container::-webkit-scrollbar-thumb:hover {
  background: rgba(0, 217, 163, 0.5);
}

/* Firefox scrollbar */
.tasks-container {
  scrollbar-width: thin;
  scrollbar-color: rgba(0, 217, 163, 0.3) var(--bg-input);
}

.loading,
.error,
.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: var(--text-muted);
}

.error {
  color: #ff5555;
  font-weight: 500;
}

.empty-state p {
  margin: 0;
  font-size: 1.1rem;
  font-weight: 400;
}
</style>

