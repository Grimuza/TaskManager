<template>
  <div class="priority-selector">
    <label class="priority-label">Priority:</label>
    <div class="priority-options">
      <button
        type="button"
        v-for="priority in priorities"
        :key="priority.value"
        :class="['priority-btn', priority.class, { active: modelValue === priority.value }]"
        @click="$emit('update:modelValue', priority.value)"
        :title="priority.label"
      >
        <span class="priority-box"></span>
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { TaskPriority } from '../types/task';

interface Props {
  modelValue: TaskPriority;
}

defineProps<Props>();
defineEmits<{
  (e: 'update:modelValue', value: TaskPriority): void;
}>();

const priorities = [
  { value: TaskPriority.Low, label: 'Low Priority', class: 'low' },
  { value: TaskPriority.Medium, label: 'Medium Priority', class: 'medium' },
  { value: TaskPriority.High, label: 'High Priority', class: 'high' },
];
</script>

<style scoped>
.priority-selector {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 16px;
}

.priority-label {
  color: var(--text-primary);
  font-size: 0.95rem;
  font-weight: 500;
}

.priority-options {
  display: flex;
  gap: 10px;
}

.priority-btn {
  padding: 0;
  border: 2px solid transparent;
  border-radius: 6px;
  background: transparent;
  cursor: pointer;
  transition: all 0.3s ease;
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.priority-box {
  width: 20px;
  height: 20px;
  border-radius: 4px;
  transition: all 0.3s ease;
}

.priority-btn.low .priority-box {
  background: #00d9a3;
}

.priority-btn.medium .priority-box {
  background: #ffd93d;
}

.priority-btn.high .priority-box {
  background: #ff5555;
}

.priority-btn:hover {
  transform: scale(1.1);
}

.priority-btn.active {
  border-color: #00d9a3;
  background: rgba(0, 217, 163, 0.1);
}

.priority-btn:not(.active) .priority-box {
  opacity: 0.4;
}

.priority-btn:not(.active):hover .priority-box {
  opacity: 0.7;
}
</style>

