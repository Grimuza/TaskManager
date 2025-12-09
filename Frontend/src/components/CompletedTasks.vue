<template>
  <div class="completed-tasks-card">
    <h3>Completed Tasks</h3>
    <div v-if="completedTasks.length === 0" class="empty-state">
      <p>No completed tasks yet!</p>
    </div>
    <div v-else class="completed-tasks-list">
      <TaskItem
        v-for="task in displayedTasks"
        :key="task.id"
        :task="task"
        @toggle="$emit('toggle', $event)"
        @delete="$emit('delete', $event)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import type { Task } from '../types/task';
import TaskItem from './TaskItem.vue';

interface Props {
  completedTasks: Task[];
}

const props = defineProps<Props>();
defineEmits<{
  (e: 'toggle', id: string): void;
  (e: 'delete', id: string): void;
}>();

const displayedTasks = computed(() => {
  return props.completedTasks.slice(0, 5);
});
</script>

<style scoped>
.completed-tasks-card {
  background: var(--bg-card);
  padding: 24px;
  border-radius: 20px;
  box-shadow: 0 8px 32px var(--shadow);
  border: 1px solid var(--border-color);
  backdrop-filter: blur(10px);
  transition: all 0.3s ease;
}

h3 {
  margin: 0 0 20px 0;
  color: var(--text-primary);
  font-size: 1.3rem;
  font-weight: 600;
  letter-spacing: -0.3px;
}

.completed-tasks-list {
  max-height: 500px;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  gap: 12px;
  padding-right: 8px;
}

/* Custom scrollbar */
.completed-tasks-list::-webkit-scrollbar {
  width: 8px;
}

.completed-tasks-list::-webkit-scrollbar-track {
  background: var(--bg-input);
  border-radius: 10px;
}

.completed-tasks-list::-webkit-scrollbar-thumb {
  background: rgba(0, 217, 163, 0.3);
  border-radius: 10px;
  transition: background 0.3s ease;
}

.completed-tasks-list::-webkit-scrollbar-thumb:hover {
  background: rgba(0, 217, 163, 0.5);
}

/* Firefox scrollbar */
.completed-tasks-list {
  scrollbar-width: thin;
  scrollbar-color: rgba(0, 217, 163, 0.3) var(--bg-input);
}

.empty-state {
  text-align: center;
  padding: 40px 20px;
  color: var(--text-muted);
}

.empty-state p {
  margin: 0;
  font-size: 1rem;
}
</style>

