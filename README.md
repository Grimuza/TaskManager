# Task Manager

A modern task management application built with .NET 9 and Vue 3, demonstrating clean architecture principles and contemporary web development practices.

## Features

### Core Functionality ✅
- ✅ **View all tasks** with statistics (total, completed, pending)
- ✅ **Create new tasks** with title, description, and priority level
- ✅ **Toggle completion** by clicking checkbox or task name
- ✅ **Drag and drop reordering** with visual drag handles
- ✅ **Task priority system** (Low, Medium, High) with color indicators
- ✅ **Auto-ordering** - new tasks automatically assigned sequence order
- ✅ **Edit existing tasks** - update title, description, and priority
- ✅ **Delete tasks** with confirmation dialog
- ✅ **Real-time UI updates** - instant feedback on all actions
- ✅ **Task completion tracking** - with automatic completion timestamps

### User Experience ✅
- ✅ **Responsive design** - works on desktop and tablet
- ✅ **Modern UI** - clean design with glassmorphism effects
- ✅ **Dark/Light theme** - toggle between themes with theme persistence
- ✅ **Completed task sorting** - completed tasks automatically move to bottom
- ✅ **Color-coded priorities** - visual priority indicators with colored left border
- ✅ **Optimistic updates** - instant UI response with rollback on error
- ✅ **Intuitive drag handles** - labeled drag affordance (⋮⋮)
- ✅ **Task statistics** - quick view of total, pending, and completed counts
- ✅ **Sort options** - sort by user-defined order, creation date, or priority

## Architecture

### Backend - Clean Architecture

The backend follows Clean Architecture principles with clear separation of concerns:

- **TaskManager.Domain**: Core business entities and repository interfaces
  - `TaskItem` entity with validation rules
  - `ITaskRepository` interface for data access abstraction

- **TaskManager.Application**: Business logic and use cases
  - Service layer (`ITaskService`, `TaskService`)
  - DTOs for data transfer (CreateTaskDto, UpdateTaskDto, TaskItemDto)
  - Business rules and validation

- **TaskManager.Infrastructure**: External dependencies and data access
  - EF Core In-Memory database implementation
  - `Data/Context/TaskDbContext` for entity configuration
  - `Data/Repositories/TaskRepository` for data access
  - Repository pattern implementation

- **TaskManager.API**: REST API layer
  - RESTful controllers
  - Dependency injection configuration
  - CORS configuration for frontend
  - Swagger/OpenAPI documentation

### Frontend - Component-Based Design

The frontend is built with Vue 3 using modern best practices:

- **Component Architecture**:
  - `App.vue`: Root component with theme toggle
  - `TaskForm.vue`: Form for creating new tasks with priority selector
  - `TaskItem.vue`: Individual task card with drag handle, checkbox, title, description, and action buttons
  - `TaskList.vue`: Container for task collection with statistics and sort controls
  - `CompletedTasks.vue`: Separate view for completed tasks
  - `SortSelector.vue`: Dropdown for sorting options (user-defined, created date, priority)
  - `PrioritySelector.vue`: Reusable priority level selector
  - `ThemeToggle.vue`: Dark/Light theme switcher
  - `HomeView.vue`: Main view composing all components

- **State Management**:
  - Pinia store for centralized state management
  - Reactive computed properties for filtered/sorted tasks
  - API integration layer
  - Error handling with fallback states

- **Type Safety**:
  - TypeScript throughout application
  - Strongly typed interfaces for Task entities
  - Type-safe API calls and store actions

- **UI/UX Features**:
  - Drag handle affordance (⋮⋮) for intuitive reordering
  - Color-coded priority badges (green=Low, yellow=Medium, red=High)
  - Colored left border matching priority level
  - Checkbox for quick toggle
  - Edit and delete buttons inline
  - Task metadata (created, completed timestamps) on each card
  - Loading and error states
  - Responsive layout adapting to different screen sizes

## Tech Stack

### Backend
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core (In-Memory)
- Swagger/OpenAPI

### Frontend
- Vue 3 (Composition API)
- TypeScript
- Pinia (State Management)
- Vue Router
- Vite (Build Tool)

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tasks` | Get all tasks (ordered by order field) |
| GET | `/api/tasks/{id}` | Get task by ID |
| POST | `/api/tasks` | Create new task (auto-assigns order) |
| PUT | `/api/tasks/{id}` | Update task |
| PATCH | `/api/tasks/{id}/toggle` | Toggle task completion |
| PUT | `/api/tasks/reorder` | Reorder multiple tasks |
| DELETE | `/api/tasks/{id}` | Delete task |

## Getting Started

### Prerequisites

- .NET 9 SDK
- Node.js 18+ (note: project was created with Node 18, newer versions recommended)
- npm

### Backend Setup

```bash
cd Backend/TaskManager.API
dotnet restore
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7xxx`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:7xxx/swagger`

### Frontend Setup

```bash
cd Frontend
npm install
npm run dev
```

The frontend will be available at: `http://localhost:5173`

## Project Structure

```
TaskManager/
├── Backend/
│   ├── TaskManager.Domain/
│   │   ├── Entities/
│   │   │   └── TaskItem.cs
│   │   └── Interfaces/
│   │       └── ITaskRepository.cs
│   ├── TaskManager.Application/
│   │   ├── DTOs/
│   │   │   ├── CreateTaskDto.cs
│   │   │   ├── UpdateTaskDto.cs
│   │   │   ├── TaskItemDto.cs
│   │   │   ├── ToggleTaskDto.cs
│   │   │   └── ReorderTaskDto.cs
│   │   ├── Interfaces/
│   │   │   └── ITaskService.cs
│   │   └── Services/
│   │       └── TaskService.cs
│   ├── TaskManager.Infrastructure/
│   │   └── Data/
│   │       ├── Context/
│   │       │   └── TaskDbContext.cs
│   │       └── Repositories/
│   │           └── TaskRepository.cs
│   └── TaskManager.API/
│       ├── Controllers/
│       │   └── TasksController.cs
│       ├── Program.cs
│       └── TaskManager.API.http
└── Frontend/
    └── src/
        ├── components/
        │   ├── TaskForm.vue
        │   ├── TaskItem.vue
        │   └── TaskList.vue
        ├── stores/
        │   └── taskStore.ts
        ├── services/
        │   └── api.ts
        ├── types/
        │   └── task.ts
        └── views/
            └── HomeView.vue
```

## Design Decisions & Assumptions

### Backend

1. **In-Memory Database**: Used EF Core In-Memory provider for simplicity and rapid development. For production, this would be replaced with SQL Server, PostgreSQL, or other persistent databases.

2. **Clean Architecture**: Separated concerns into distinct layers:
   - **Domain layer** has no dependencies (pure business logic)
   - **Application** depends only on Domain (service implementations)
   - **Infrastructure** depends on Application and Domain (data access)
   - **API** depends on all layers but contains no business logic (HTTP handling only)
   - Organized folders: Context and Repositories clearly separated

3. **Repository Pattern**: Abstracted data access to allow easy switching of data sources without affecting business logic.

4. **Task Ordering**: 
   - Each task has an `Order` field for custom sequencing
   - New tasks automatically get `max(order) + 1`
   - Dedicated endpoint for bulk reordering via drag-and-drop
   - Completed tasks sort separately to bottom

5. **Task Priority**: 
   - Implemented as enum (Low=0, Medium=1, High=2)
   - Used for both visual indicators and sorting
   - Easily extendable for future priority levels

6. **Toggle Endpoint**: Created dedicated PATCH endpoint for toggling completion status as it's a frequent operation.

7. **Timestamps**: 
   - `CreatedAt` automatically set on creation
   - `CompletedAt` set when task is marked complete
   - Used for sorting and historical tracking

8. **CORS**: Enabled for local development with frontend running on different port (5173 vs 5000).

9. **Validation**: 
   - Title field required, max 200 characters
   - Description optional, max 1000 characters
   - Error handling with meaningful HTTP status codes

### Frontend

1. **Component-Based Design**: Each component has single responsibility:
   - `TaskForm` handles creation only
   - `TaskItem` handles display and inline interactions
   - `TaskList` handles collection management and drag-drop coordination
   - `SortSelector` and `PrioritySelector` are reusable utilities

2. **State Management with Pinia**: 
   - Centralized store instead of prop drilling
   - Computed properties for derived state (pending, completed, sorted)
   - Actions encapsulate API calls and state mutations
   - Error state management with user-facing messages

3. **Drag and Drop Implementation**:
   - Native HTML5 drag and drop API (no external library)
   - Visual drag handle (⋮⋮) for affordance
   - Drag feedback with opacity changes
   - Optimistic UI updates (update locally, rollback on error)
   - Maintains pending/completed task separation during reorder

4. **Click to Toggle**: 
   - Checkbox click toggles completion
   - Task title/name click also toggles (for ergonomics)
   - Prevents multiple toggle methods interfering

5. **Smart Sorting**: 
   - Pending tasks display in custom order (default)
   - Can sort by creation date (ascending/descending)
   - Can sort by priority (Low to High or High to Low)
   - Completed tasks always grouped at bottom
   - Max 10 pending tasks shown (pagination consideration)

6. **Optimistic Updates**: 
   - Store updates immediately for responsive feel
   - Error handling with automatic rollback
   - User sees instant feedback while network request completes

7. **Error Handling**: 
   - Try-catch blocks around all async operations
   - Error state in store for UI notification
   - Network errors logged to console
   - Production version would add toast notifications

8. **Styling**: 
   - Pure CSS (no framework) for lightweight build
   - CSS custom properties (variables) for theme switching
   - Glassmorphism effects with backdrop-filter
   - Smooth transitions and hover effects
   - Responsive grid layout

9. **Theme System**:
   - Dark and light mode with CSS variables
   - System preference detection
   - Theme persistence via localStorage
   - Smooth transitions between themes

10. **TypeScript**: 
    - Full type safety throughout
    - Interfaces for all data structures
    - Enums for priority levels and sort options
    - Strict null checking enabled

## Component Layout Details

### TaskItem Card Layout
```
┌─────────────────────────────────────────────────────┐
│ [drag] ☑ Title Text          [edit btn] [delete btn]│  ← Header
│                                                      │
│  Description text                                    │  ← Description (aligned with checkbox)
│                                                      │
│                                 Created: DATE TIME   │  ← Footer
└─────────────────────────────────────────────────────┘
```

**Color Indicators**:
- Left border color indicates priority (Green=Low, Yellow=Medium, Red=High)
- Each task card has semi-transparent background with subtle shadow

### UI Features
- Drag handle appears on left with grabby cursor on hover
- Checkbox toggles completion (strikethrough and opacity change)
- Title is clickable for toggle (UX affordance)
- Action buttons (edit, delete) appear inline on right
- Description auto-wraps and is compact
- Timestamps in small muted text at bottom-right

## What Could Be Added (Future Enhancements)

### Features
- User authentication and authorization (JWT)
- Task categories/tags for organization
- Due dates and reminder notifications
- Search and advanced filter functionality
- Pagination for large task lists (currently limits to 10 pending)
- Task subtasks/checklist items
- Task notes/comments
- Task history/activity log
- Recurring tasks
- Task templates
- Bulk operations (select multiple, batch delete)
- Task archiving instead of deletion
- Undo/redo functionality

### Technical Improvements
- Persistent database (SQL Server, PostgreSQL, MongoDB)
- Full unit and integration test suite
- E2E tests (Playwright, Cypress)
- API request/response logging
- Performance monitoring
- Keyboard shortcuts for power users
- Task export (PDF, CSV)
- Task import from file
- Offline support with sync when online (PWA)
- Real-time collaboration (WebSockets)
- API rate limiting
- Request validation with FluentValidation
- Comprehensive error logging (Serilog)

### DevOps & Deployment
- Docker containerization
- Docker Compose for local dev environment
- CI/CD pipelines (GitHub Actions, Azure DevOps)
- Environment-specific configurations
- Database migrations
- Health checks and monitoring
- Application Insights integration
- Deployment to Azure/AWS/GCP
- Database backup strategy

## Notes

- **Data Persistence**: The API uses an In-Memory database, so data is lost when the application restarts. For production use, configure a persistent database.
- **CORS Configuration**: CORS is configured for development (localhost:5173). Update for production deployments.
- **API Base URL**: Frontend expects backend on `http://localhost:5000`. Update `Frontend/src/services/api.ts` if backend runs on different port.
- **Code Standards**: All code and comments are in English as per requirements.
- **No External CSS Framework**: Styling uses pure CSS with custom properties for maintainability and minimal bundle size.
- **Task Limit**: Currently displays maximum 10 pending tasks. This is configurable in `TaskList.vue`.
- **Theme Persistence**: Dark/Light theme preference is saved to localStorage.
- **Drag and Drop**: Uses native HTML5 drag-drop API. Browser support: Chrome 4+, Firefox 3.6+, Safari 3.1+, Edge 12+

