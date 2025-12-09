<template>
  <div 
    class="task-item" 
    :class="{ completed: task.isCompleted, dragging: isDragging, editing: isEditing }"
    :draggable="draggable && !isEditing"
    @dragstart="$emit('dragstart', $event)"
    @dragover="$emit('dragover', $event)"
    @drop="$emit('drop', $event)"
    @dragend="$emit('dragend', $event)"
  >
    <div class="priority-indicator" :class="priorityClass" :title="priorityLabel"></div>
    
    <!-- View Mode -->
    <div v-if="!isEditing" class="task-content">
      <div class="task-header">
        <div v-if="!isEditing" class="drag-handle" title="Drag to reorder">
          ⋮⋮
        </div>
        <input
          type="checkbox"
          :checked="task.isCompleted"
          @change="handleToggle"
          @click.stop
          class="task-checkbox"
        />
        <h3 class="task-title" @click="handleToggle">{{ task.title }}</h3>
        <div class="task-actions">
          <button v-if="!task.isCompleted" @click="startEdit" class="btn-edit" title="Edit task">
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
              <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
              <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
            </svg>
          </button>
          <button @click="handleDelete" class="btn-delete" title="Delete task">
            ✕
          </button>
        </div>
      </div>
      <p v-if="task.description" class="task-description">
        {{ task.description }}
      </p>
      <span v-if="task.completedAt" class="task-date completed-date">
        Completed: {{ formatDate(task.completedAt) }}
      </span>
    </div>

    <!-- Edit Mode -->
    <div v-else class="task-edit-form">
      <div class="edit-field">
        <label>Title:</label>
        <input 
          v-model="editedTitle" 
          type="text" 
          class="edit-input"
          @click.stop
        />
      </div>
      <div class="edit-field">
        <label>Description:</label>
        <textarea 
          v-model="editedDescription" 
          class="edit-textarea"
          rows="3"
          @click.stop
        ></textarea>
      </div>
      <div class="edit-field">
        <label>Priority:</label>
        <PrioritySelector v-model="editedPriority" @click.stop />
      </div>
      <div class="edit-actions">
        <button @click="saveEdit" class="btn-save">
          ✓ Save
        </button>
        <button @click="cancelEdit" class="btn-cancel">
          ✕ Cancel
        </button>
      </div>
    </div>

    <!-- Action Buttons -->
    <div v-if="!isEditing" class="actions-footer">
      <span class="task-date created-date">
        Created: {{ formatDate(task.createdAt) }}
      </span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import type { Task } from '../types/task';
import { TaskPriority } from '../types/task';
import { useTaskStore } from '../stores/taskStore';
import PrioritySelector from './PrioritySelector.vue';

interface Props {
  task: Task;
  draggable?: boolean;
}

const props = defineProps<Props>();
const taskStore = useTaskStore();

const isDragging = ref(false);
const isEditing = ref(false);
const editedTitle = ref('');
const editedDescription = ref('');
const editedPriority = ref(TaskPriority.Low);

/**
 * Priority CSS class
 */
const priorityClass = computed(() => {
  switch (props.task.priority) {
    case TaskPriority.Low:
      return 'priority-low';
    case TaskPriority.Medium:
      return 'priority-medium';
    case TaskPriority.High:
      return 'priority-high';
    default:
      return 'priority-low';
  }
});

/**
 * Priority label
 */
const priorityLabel = computed(() => {
  switch (props.task.priority) {
    case TaskPriority.Low:
      return 'Low Priority';
    case TaskPriority.Medium:
      return 'Medium Priority';
    case TaskPriority.High:
      return 'High Priority';
    default:
      return 'Low Priority';
  }
});

/**
 * Toggle task completion status
 */
async function handleToggle() {
  try {
    await taskStore.toggleTask(props.task.id);
  } catch (error) {
    console.error('Failed to toggle task:', error);
  }
}

/**
 * Delete the task
 */
async function handleDelete() {
  if (confirm('Are you sure you want to delete this task?')) {
    try {
      await taskStore.deleteTask(props.task.id);
    } catch (error) {
      console.error('Failed to delete task:', error);
    }
  }
}

/**
 * Start editing mode
 */
function startEdit() {
  isEditing.value = true;
  editedTitle.value = props.task.title;
  editedDescription.value = props.task.description;
  editedPriority.value = props.task.priority;
}

/**
 * Cancel editing
 */
function cancelEdit() {
  isEditing.value = false;
}

/**
 * Save edited task
 */
async function saveEdit() {
  if (!editedTitle.value.trim()) {
    alert('Title is required');
    return;
  }

  try {
    await taskStore.updateTask(props.task.id, {
      title: editedTitle.value.trim(),
      description: editedDescription.value.trim(),
      isCompleted: props.task.isCompleted,
      priority: editedPriority.value,
    });
    isEditing.value = false;
  } catch (error) {
    console.error('Failed to update task:', error);
    alert('Failed to update task');
  }
}

/**
 * Format date to readable string
 */
function formatDate(dateString: string): string {
  return new Date(dateString).toLocaleString();
}
</script>

<style scoped>
.task-item {
  display: flex;
  flex-direction: column;
  align-items: stretch;
  gap: 0;
  background: var(--bg-card);
  padding: 20px;
  border-radius: 16px;
  box-shadow: 0 4px 20px var(--shadow);
  transition: all 0.3s ease;
  cursor: move;
  border: 1px solid var(--border-color);
  backdrop-filter: blur(10px);
  position: relative;
}

.priority-indicator {
  position: absolute;
  left: 0;
  top: 0;
  bottom: 0;
  width: 5px;
  border-radius: 16px 0 0 16px;
  transition: opacity 0.3s ease;
}

.priority-low {
  background: #00d9a3;
}

.priority-medium {
  background: #ffd93d;
}

.priority-high {
  background: #ff5555;
}

.task-item:hover {
  transform: translateY(-3px);
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.4);
  background: rgba(30, 30, 30, 0.7);
  border-color: rgba(0, 217, 163, 0.2);
}

.task-item.completed {
  opacity: 0.5;
  background: rgba(20, 20, 20, 0.5);
}

.task-item.completed .priority-indicator {
  opacity: 0.3;
}

.task-item.dragging {
  opacity: 0.6;
  transform: scale(0.98);
  box-shadow: 0 10px 40px rgba(0, 217, 163, 0.2);
}

.drag-handle {
  color: #444444;
  font-size: 1.3rem;
  cursor: grab;
  user-select: none;
  padding: 0 4px;
  display: flex;
  align-items: center;
  transition: color 0.3s ease;
}

.drag-handle:hover {
  color: #00d9a3;
}

:global([data-theme="light"]) .drag-handle {
  color: #adb5bd !important;
}

:global([data-theme="light"]) .drag-handle:hover {
  color: #00d9a3 !important;
}

.drag-handle:active {
  cursor: grabbing;
}

.task-content {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 8px;
  cursor: pointer;
  user-select: none;
}

.task-header {
  display: flex;
  gap: 12px;
  align-items: center;
}

.drag-handle {
  color: #444444;
  font-size: 1.3rem;
  cursor: grab;
  user-select: none;
  padding: 0 4px;
  display: flex;
  align-items: center;
  transition: color 0.3s ease;
  flex-shrink: 0;
}

.drag-handle:hover {
  color: #00d9a3;
}

:global([data-theme="light"]) .drag-handle {
  color: #adb5bd !important;
}

:global([data-theme="light"]) .drag-handle:hover {
  color: #00d9a3 !important;
}

.drag-handle:active {
  cursor: grabbing;
}

.task-checkbox {
  width: 20px;
  height: 20px;
  cursor: pointer;
  flex-shrink: 0;
  accent-color: #00d9a3;
}

.task-title {
  margin: 0;
  color: var(--text-primary);
  font-size: 1.05rem;
  font-weight: 600;
  letter-spacing: -0.2px;
  line-height: 1.4;
  transition: color 0.3s ease;
  flex: 1;
}

.task-item.completed .task-title {
  text-decoration: line-through;
  color: var(--text-muted);
}

.task-description {
  margin: 4px 0 0 48px;
  color: var(--text-secondary);
  font-size: 0.85rem;
  line-height: 1.5;
  transition: color 0.3s ease;
}

.task-item.completed .task-description {
  color: var(--text-muted);
}

.task-date {
  display: inline-block;
  margin-right: 16px;
  color: #666666;
  font-size: 0.8rem;
  font-weight: 500;
  padding: 4px 10px;
  background: rgba(255, 255, 255, 0.03);
  border-radius: 6px;
  transition: all 0.3s ease;
}

.created-date {
  margin: 0;
  display: block;
}

.completed-date {
  color: #00d9a3;
  background: rgba(0, 217, 163, 0.1);
}

:global([data-theme="light"]) .task-date {
  color: #6c757d !important;
  background: rgba(0, 0, 0, 0.03) !important;
}

:global([data-theme="light"]) .completed-date {
  color: #00d9a3 !important;
  background: rgba(0, 217, 163, 0.1) !important;
}

.task-actions {
  display: flex;
  gap: 8px;
  align-items: center;
  justify-content: flex-end;
  margin: 0;
}

.task-header .task-actions {
  margin: 0 0 0 auto;
}

.actions-footer {
  text-align: right;
  margin-top: 12px;
  padding-top: 12px;
  border-top: 1px solid rgba(255, 255, 255, 0.05);
}

.btn-edit {
  background: rgba(0, 217, 163, 0.15);
  color: #00d9a3;
  border: 1px solid rgba(0, 217, 163, 0.2);
  border-radius: 10px;
  width: 36px;
  height: 36px;
  cursor: pointer;
  transition: all 0.3s ease;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0;
}

.btn-edit:hover {
  background: rgba(0, 217, 163, 0.25);
  border-color: #00d9a3;
  transform: scale(1.05);
}

.btn-delete {
  background: rgba(255, 60, 60, 0.15);
  color: #ff5555;
  border: 1px solid rgba(255, 60, 60, 0.2);
  border-radius: 10px;
  width: 36px;
  height: 36px;
  font-size: 1.1rem;
  cursor: pointer;
  transition: all 0.3s ease;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-delete:hover {
  background: rgba(255, 60, 60, 0.25);
  border-color: #ff5555;
  transform: scale(1.05);
}

/* Edit Mode Styles */
.task-item.editing {
  cursor: default;
}

.task-edit-form {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 16px;
  padding: 8px 0;
}

.edit-field {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.edit-field label {
  color: var(--text-secondary);
  font-size: 0.85rem;
  font-weight: 600;
}

.edit-input,
.edit-textarea {
  width: 100%;
  padding: 10px 14px;
  background: var(--bg-input);
  border: 1px solid var(--border-input);
  border-radius: 8px;
  color: var(--text-primary);
  font-size: 0.95rem;
  font-family: inherit;
  transition: all 0.3s ease;
}

.edit-input:focus,
.edit-textarea:focus {
  outline: none;
  border-color: #00d9a3;
  box-shadow: 0 0 0 2px rgba(0, 217, 163, 0.1);
}

.edit-textarea {
  resize: vertical;
  min-height: 60px;
}

.edit-actions {
  display: flex;
  gap: 10px;
  margin-top: 8px;
}

.btn-save,
.btn-cancel {
  flex: 1;
  padding: 10px;
  border-radius: 8px;
  font-size: 0.9rem;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
}

.btn-save {
  background: rgba(0, 217, 163, 0.2);
  color: #00d9a3;
  border: 1px solid rgba(0, 217, 163, 0.3);
}

.btn-save:hover {
  background: rgba(0, 217, 163, 0.3);
  border-color: #00d9a3;
}

.btn-cancel {
  background: rgba(255, 60, 60, 0.15);
  color: #ff5555;
  border: 1px solid rgba(255, 60, 60, 0.2);
}

.btn-cancel:hover {
  background: rgba(255, 60, 60, 0.25);
  border-color: #ff5555;
}
</style>

