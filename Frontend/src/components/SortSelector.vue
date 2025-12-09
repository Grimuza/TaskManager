<template>
  <div class="sort-selector">
    <label class="sort-label">Order by:</label>
    <select v-model="localSortBy" class="sort-select" @change="handleSortChange">
      <option value="userDefined">User Defined</option>
      <option value="created">Created Date</option>
      <option value="priority">Priority</option>
    </select>
    
    <button
      v-if="localSortBy !== 'userDefined'"
      type="button"
      class="sort-order-btn"
      @click="toggleSortOrder"
      :title="localSortOrder === 'asc' ? 'Ascending' : 'Descending'"
    >
      <svg v-if="localSortOrder === 'asc'" class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M12 19V5M5 12l7-7 7 7"/>
      </svg>
      <svg v-else class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
        <path d="M12 5v14M5 12l7 7 7-7"/>
      </svg>
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import type { SortBy, SortOrder } from '../types/task';

interface Props {
  sortBy: SortBy;
  sortOrder: SortOrder;
}

const props = defineProps<Props>();
const emit = defineEmits<{
  (e: 'update:sortBy', value: SortBy): void;
  (e: 'update:sortOrder', value: SortOrder): void;
}>();

const localSortBy = ref(props.sortBy);
const localSortOrder = ref(props.sortOrder);

watch(() => props.sortBy, (newValue) => {
  localSortBy.value = newValue;
});

watch(() => props.sortOrder, (newValue) => {
  localSortOrder.value = newValue;
});

function handleSortChange() {
  emit('update:sortBy', localSortBy.value);
}

function toggleSortOrder() {
  localSortOrder.value = localSortOrder.value === 'asc' ? 'desc' : 'asc';
  emit('update:sortOrder', localSortOrder.value);
}
</script>

<style scoped>
.sort-selector {
  display: flex;
  align-items: center;
  gap: 10px;
}

.sort-label {
  color: var(--text-primary);
  font-size: 0.85rem;
  font-weight: 500;
}

.sort-select {
  padding: 6px 12px;
  background: var(--bg-input);
  border: 1px solid var(--border-input);
  border-radius: 8px;
  color: var(--text-primary);
  font-size: 0.85rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.3s ease;
}

.sort-select:focus {
  outline: none;
  border-color: #00d9a3;
  box-shadow: 0 0 0 2px rgba(0, 217, 163, 0.1);
}

.sort-order-btn {
  width: 32px;
  height: 32px;
  border: 1px solid var(--border-input);
  border-radius: 8px;
  background: var(--bg-input);
  color: var(--text-primary);
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0;
}

.sort-order-btn:hover {
  border-color: #00d9a3;
  color: #00d9a3;
  transform: scale(1.05);
}

.icon {
  width: 18px;
  height: 18px;
}
</style>

