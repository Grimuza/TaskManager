<template>
  <div class="home">
    <div class="container">
      <header class="header">
        <h1>Task Manager</h1>
        <p>Manage your tasks efficiently</p>
      </header>

      <TaskForm />
      <div class="tasks-grid">
        <TaskList />
        <CompletedTasks 
          :completed-tasks="taskStore.completedTasks"
          @toggle="taskStore.toggleTask"
          @delete="taskStore.deleteTask"
        />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useTaskStore } from '../stores/taskStore';
import TaskForm from '../components/TaskForm.vue';
import TaskList from '../components/TaskList.vue';
import CompletedTasks from '../components/CompletedTasks.vue';

const taskStore = useTaskStore();

/**
 * Load tasks when component mounts
 */
onMounted(() => {
  taskStore.fetchTasks();
});
</script>

<style scoped>
/* Dark mode */
.home {
  min-height: 100vh;
  background: var(--bg-primary);
  padding: 40px 20px;
  transition: background 0.3s ease;
}

.container {
  max-width: 900px;
  margin: 0 auto;
}

.tasks-grid {
  display: flex;
  flex-direction: column;
  gap: 32px;
}

.header {
  text-align: center;
  color: #ffffff;
  margin-bottom: 50px;
  padding: 40px 30px;
  background: linear-gradient(135deg, rgba(0, 217, 163, 0.1) 0%, rgba(0, 201, 167, 0.05) 100%);
  border-radius: 24px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.4);
  border: 1px solid rgba(255, 255, 255, 0.08);
  backdrop-filter: blur(10px);
  transition: all 0.3s ease;
}

.header h1 {
  margin: 0 0 12px 0;
  font-size: 3rem;
  font-weight: 700;
  background: linear-gradient(135deg, #00d9a3 0%, #00c9a7 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
  letter-spacing: -0.5px;
}

.header p {
  margin: 0;
  font-size: 1.1rem;
  opacity: 0.7;
  color: #b0b0b0;
  font-weight: 400;
}

/* Light mode */
:global([data-theme="light"]) .home {
  background: #f5f7fa !important;
}

:global([data-theme="light"]) .header {
  background: linear-gradient(135deg, rgba(0, 217, 163, 0.15) 0%, rgba(0, 201, 167, 0.08) 100%) !important;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.08) !important;
  border: 1px solid rgba(0, 217, 163, 0.2) !important;
}

:global([data-theme="light"]) .header p {
  color: #6c757d !important;
  opacity: 0.9;
}
</style>

