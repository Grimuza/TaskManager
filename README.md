# Task Manager

A full-stack task management app with .NET 9 backend and Vue 3 frontend.

## Features

- ✅ Create, read, update, delete tasks
- ✅ Toggle completion & drag-to-reorder
- ✅ Task priorities (Low, Medium, High) with color coding
- ✅ Dark/Light theme
- ✅ Task statistics (Total, Pending, Completed)
- ✅ Responsive design

## Tech Stack

- **Backend**: .NET 9, ASP.NET Core, EF Core (In-Memory)
- **Frontend**: Vue 3, TypeScript, Pinia, Vite
- **Architecture**: Clean Architecture (4-layer backend)

## Quick Start

### Prerequisites
- .NET 9 SDK
- Node.js 18+
- npm

### Setup

```bash
# Backend
cd Backend/TaskManager.API
dotnet run

# Frontend (new terminal)
cd Frontend
npm install
npm run dev
```

Access the app at `http://localhost:5173`

**See [SETUP.md](SETUP.md) for detailed setup instructions and troubleshooting.**

## Project Structure

```
TaskManager/
├── Backend/
│   ├── TaskManager.Domain/      (Entities, Interfaces)
│   ├── TaskManager.Application/ (Services, DTOs)
│   ├── TaskManager.Infrastructure/ (Database, Repositories)
│   └── TaskManager.API/         (Controllers, HTTP)
├── Frontend/
│   └── src/
│       ├── components/          (Vue components)
│       ├── stores/              (Pinia state)
│       ├── services/            (API client)
│       └── types/               (TypeScript)
├── README.md
└── SETUP.md
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tasks` | List all tasks |
| POST | `/api/tasks` | Create task |
| PUT | `/api/tasks/{id}` | Update task |
| PATCH | `/api/tasks/{id}/toggle` | Toggle completion |
| PUT | `/api/tasks/reorder` | Reorder tasks |
| DELETE | `/api/tasks/{id}` | Delete task |

**Swagger docs**: http://localhost:5000/swagger

## Architecture Overview

### Backend - Clean Architecture
- **Domain**: Business entities (TaskItem, TaskPriority)
- **Application**: Services & DTOs
- **Infrastructure**: EF Core, repository implementation
- **API**: Controllers & HTTP handling

### Frontend - Component-Based
- **Components**: TaskForm, TaskItem, TaskList, etc.
- **State**: Pinia store for centralized management
- **Services**: API client abstraction
- **Type-safe**: Full TypeScript implementation

## Key Features Explained

### Task Management
- Auto-ordered tasks (new tasks get max order + 1)
- Drag-to-reorder with visual feedback
- Edit inline without full modal
- Timestamps: created & completed dates

### Priorities
- 3 levels: Low (green), Medium (yellow), High (red)
- Color-coded left border on each task
- Sort by priority option

### UI/UX
- Responsive design (desktop/tablet)
- Dark/Light theme with persistence
- Optimistic UI updates
- Task statistics dashboard

## Development

### Environment

Backend uses in-memory database (data resets on restart).
For production, configure a persistent database in `Program.cs`.

### Detailed Documentation

See **[SETUP.md](SETUP.md)** for:
- Troubleshooting guide
- Testing the API (curl, PowerShell, Swagger)
- Configuration & environment variables
- Development tips & debugging
- Performance optimization
- Future enhancements

## License

MIT
