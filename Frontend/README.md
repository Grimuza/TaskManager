# TaskManager Frontend

Modern task management UI built with Vue 3, TypeScript, and Vite.

## Quick Start

```bash
npm install
npm run dev
```

Application will be available at: http://localhost:5173

## Features

- ✅ View all tasks with real-time statistics
- ✅ Create tasks with title and description
- ✅ Toggle completion by clicking checkbox or task name
- ✅ **Drag and drop to reorder tasks**
- ✅ **Custom ordering with visual feedback**
- ✅ **Completed tasks automatically move to bottom**
- ✅ Delete tasks with confirmation
- ✅ Responsive design
- ✅ Modern gradient UI with drag handles

## Project Structure

```
src/
├── components/           # Vue components
│   ├── TaskForm.vue     # Form for creating tasks
│   ├── TaskItem.vue     # Individual task display
│   └── TaskList.vue     # Task collection container
├── stores/              # Pinia state management
│   └── taskStore.ts     # Task state and actions
├── services/            # API communication
│   └── api.ts           # API service layer
├── types/               # TypeScript types
│   └── task.ts          # Task interfaces
├── views/               # Page views
│   └── HomeView.vue     # Main application view
├── router/              # Vue Router configuration
│   └── index.ts         # Route definitions
├── App.vue              # Root component
└── main.ts              # Application entry point
```

## Architecture

### Component Hierarchy

```
App.vue
└── HomeView.vue
    ├── TaskForm.vue
    └── TaskList.vue (handles drag & drop coordination)
        └── TaskItem.vue (draggable, with drag handle)
```

### State Management

The application uses Pinia for state management:

```typescript
const taskStore = useTaskStore();

// Available state
taskStore.tasks      // Array of tasks
taskStore.loading    // Loading indicator
taskStore.error      // Error message

// Available actions
taskStore.fetchTasks()              // Load all tasks
taskStore.createTask(data)          // Create new task (auto-orders)
taskStore.toggleTask(id)            // Toggle completion
taskStore.reorderTasks(tasks)       // Reorder tasks (drag & drop)
taskStore.deleteTask(id)            // Delete task
```

### API Service

All API calls go through the centralized API service in `services/api.ts`:

```typescript
import { taskApi } from '@/services/api';

// Usage
const tasks = await taskApi.getAllTasks();
const newTask = await taskApi.createTask({ title, description });
await taskApi.toggleTask(taskId);
await taskApi.reorderTasks([{ id, order }]); // NEW: bulk reorder
await taskApi.deleteTask(taskId);
```

## Components

### TaskForm.vue

Form component for creating new tasks.

**Features**:
- Input validation
- Auto-focus on title field
- Form reset after submission
- Disabled submit button for empty titles

### TaskItem.vue

Displays individual task with interaction and drag capabilities.

**Features**:
- **Drag handle (⋮⋮) for reordering**
- **Draggable with native HTML5 drag & drop**
- Checkbox for completion toggle
- Click on task name to toggle
- Delete button with confirmation
- Formatted timestamps
- Visual feedback for completed tasks
- Hover and drag effects

**Props**:
```typescript
interface Props {
  task: Task;
  draggable?: boolean;
}
```

**Events**:
- `@dragstart` - Fired when drag begins
- `@dragover` - Fired when dragging over
- `@drop` - Fired when dropped
- `@dragend` - Fired when drag ends

### TaskList.vue

Container component that displays all tasks, statistics, and handles drag & drop.

**Features**:
- Real-time statistics (total, completed, pending)
- **Drag and drop coordination**
- **Smart sorting (pending first, completed last)**
- **Order preservation within groups**
- Loading state
- Error handling
- Empty state message
- Responsive layout

**Drag & Drop Implementation**:
- Tracks dragged item index
- Provides visual feedback during drag
- Handles drop and reorders array
- Syncs with backend via Pinia store
- Optimistic updates with error rollback

## TypeScript Types

### Task Interface

```typescript
interface Task {
  id: string;
  title: string;
  description: string;
  isCompleted: boolean;
  order: number;              // NEW: for custom ordering
  createdAt: string;
  completedAt?: string;
}
```

### CreateTask Interface

```typescript
interface CreateTask {
  title: string;
  description: string;
  // Order is auto-assigned by backend
}
```

### ReorderTask Interface

```typescript
interface ReorderTask {
  id: string;
  order: number;
}
```

## Styling

The application uses scoped CSS with:
- Modern gradient backgrounds
- Card-based layout
- Smooth transitions
- Hover effects
- Responsive design
- No external CSS frameworks (lightweight)

### Color Palette

- Primary: `#42b983` (Green)
- Secondary: `#2c3e50` (Dark blue-gray)
- Danger: `#e74c3c` (Red)
- Background gradient: Purple to blue

## Scripts

```bash
# Development server with hot reload
npm run dev

# Type-check and build for production
npm run build

# Preview production build
npm run preview
```

## Configuration

### API Endpoint

The API URL is configured in `services/api.ts`:

```typescript
const API_URL = 'http://localhost:5000/api/tasks';
```

To change the backend URL:
1. Edit `API_URL` in `src/services/api.ts`
2. Ensure CORS is configured on the backend

### Vite Configuration

Configuration is in `vite.config.ts`:

```typescript
export default defineConfig({
  plugins: [vue()],
  server: {
    port: 5173
  }
})
```

## Development Guide

### Adding a New Feature

1. **Define TypeScript types** in `src/types/`
2. **Add API method** in `src/services/api.ts`
3. **Update Pinia store** in `src/stores/taskStore.ts`
4. **Create/update component** in `src/components/`
5. **Test in browser**

### Component Development

Components follow Vue 3 Composition API:

```vue
<template>
  <!-- Template -->
</template>

<script setup lang="ts">
// Composition API logic
</script>

<style scoped>
/* Scoped styles */
</style>
```

### State Updates

State updates are reactive through Pinia:

```typescript
// Store automatically triggers UI updates
await taskStore.createTask(taskData);
// Components using taskStore.tasks automatically re-render
```

## Browser Compatibility

- Chrome/Edge: Latest 2 versions
- Firefox: Latest 2 versions
- Safari: Latest 2 versions

## Best Practices Used

- ✅ TypeScript for type safety
- ✅ Composition API for better code organization
- ✅ Centralized state management
- ✅ Single responsibility components
- ✅ Proper error handling with rollback
- ✅ Accessible HTML semantics
- ✅ Native HTML5 drag and drop API
- ✅ Optimistic UI updates
- ✅ Scoped CSS to prevent leaks
- ✅ Async/await for API calls
- ✅ Loading and error states
- ✅ Event delegation for drag events

## Troubleshooting

### Issue: Changes not reflecting

- Check if backend is running
- Verify API URL in `services/api.ts`
- Check browser console for errors
- Clear browser cache

### Issue: TypeScript errors

```bash
# Check types
npm run build

# Ensure all dependencies are installed
npm install
```

### Issue: CORS errors

- Verify backend CORS configuration
- Check backend is running on correct port
- Ensure API_URL matches backend URL

