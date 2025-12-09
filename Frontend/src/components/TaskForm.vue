<template>
  <div class="task-form">
    <h2>Add New Task</h2>
    <form @submit.prevent="handleSubmit">
      <div class="form-group">
        <input
          v-model="title"
          type="text"
          placeholder="Task title"
          required
          class="form-input"
        />
      </div>
      <div class="form-group">
        <textarea
          v-model="description"
          placeholder="Task description (optional)"
          rows="3"
          class="form-input"
        ></textarea>
      </div>
      <PrioritySelector v-model="priority" />
      <button type="submit" class="btn-submit" :disabled="!title.trim()">
        Add Task
      </button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useTaskStore } from '../stores/taskStore';
import { TaskPriority } from '../types/task';
import PrioritySelector from './PrioritySelector.vue';

const taskStore = useTaskStore();

const title = ref('');
const description = ref('');
const priority = ref(TaskPriority.Low);

/**
 * Handle form submission to create a new task
 */
async function handleSubmit() {
  if (!title.value.trim()) return;

  try {
    await taskStore.createTask({
      title: title.value.trim(),
      description: description.value.trim(),
      priority: priority.value,
    });
    
    // Reset form
    title.value = '';
    description.value = '';
    priority.value = TaskPriority.Low;
  } catch (error) {
    console.error('Failed to create task:', error);
  }
}
</script>

<style scoped>
.task-form {
  background: var(--bg-card);
  padding: 32px;
  border-radius: 20px;
  box-shadow: 0 8px 32px var(--shadow);
  margin-bottom: 32px;
  border: 1px solid var(--border-color);
  backdrop-filter: blur(10px);
  transition: all 0.3s ease;
}

h2 {
  margin: 0 0 24px 0;
  color: var(--text-primary);
  font-size: 1.4rem;
  font-weight: 600;
  letter-spacing: -0.3px;
  transition: color 0.3s ease;
}

.form-group {
  margin-bottom: 16px;
}

.form-input {
  width: 100%;
  padding: 14px 18px;
  border: 1px solid var(--border-input);
  border-radius: 12px;
  font-size: 0.95rem;
  font-family: inherit;
  transition: all 0.3s ease;
  background: var(--bg-input);
  color: var(--text-primary);
}

.form-input:focus {
  outline: none;
  border-color: #00d9a3;
  box-shadow: 0 0 0 3px rgba(0, 217, 163, 0.1);
}

.form-input::placeholder {
  color: var(--text-muted);
}

/* Light mode */
:global([data-theme="light"]) .form-input {
  background: #ffffff !important;
  border: 1px solid #e0e0e0 !important;
  color: #2c3e50 !important;
}

:global([data-theme="light"]) .form-input:focus {
  background: #ffffff !important;
  border-color: #00d9a3 !important;
  box-shadow: 0 0 0 3px rgba(0, 217, 163, 0.1) !important;
}

:global([data-theme="light"]) .form-input::placeholder {
  color: #adb5bd !important;
}

textarea.form-input {
  resize: vertical;
  min-height: 90px;
}

.btn-submit {
  width: 100%;
  padding: 14px;
  background: rgba(0, 217, 163, 0.4);
  color: #ffffff;
  border: 1px solid rgba(0, 217, 163, 0.5);
  border-radius: 12px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  box-shadow: 0 4px 20px rgba(0, 217, 163, 0.2);
  backdrop-filter: blur(10px);
}

.btn-submit:hover:not(:disabled) {
  background: rgba(0, 217, 163, 0.5);
  border-color: rgba(0, 217, 163, 0.7);
  transform: translateY(-2px);
  box-shadow: 0 6px 25px rgba(0, 217, 163, 0.3);
}

.btn-submit:active:not(:disabled) {
  transform: translateY(0);
}

.btn-submit:disabled {
  background: rgba(40, 40, 40, 0.5);
  color: #666666;
  cursor: not-allowed;
  box-shadow: none;
}
</style>

