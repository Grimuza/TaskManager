# Quick Setup Guide

## Prerequisites Check

Before starting, ensure you have:

```bash
# Check .NET version (should be 9.0 or higher)
dotnet --version

# Check Node.js version (should be 18.0 or higher)
node --version

# Check npm version
npm --version
```

## Step-by-Step Setup

### 1. Clone or Download the Project

```bash
cd TaskManager
```

### 2. Backend Setup

```bash
# Navigate to API project
cd Backend/TaskManager.API

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the API (choose one)
dotnet run                    # Uses default profile (https)
dotnet run --launch-profile http  # Uses http only
```

The API should start on:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:7221 (may vary)
- Swagger/API Docs: http://localhost:5000/swagger

**Expected Output**:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

### 3. Frontend Setup (Open a new terminal)

```bash
# Navigate to frontend
cd Frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

**Expected Output**:
```
  VITE v5.4.11  ready in XXX ms
  ➜  Local:   http://localhost:5173/
```

The frontend will be available at: http://localhost:5173

### 4. Test the Application

1. Open your browser and go to http://localhost:5173
2. You should see the Task Manager interface with:
   - Input field to create new tasks
   - List of existing tasks (if any)
   - Theme toggle button (top-right)
   - Task statistics (Total, Completed, Pending)

3. **Try these interactions**:
   - ✅ Create a new task with title and description
   - ✅ Click the checkbox to mark task as complete
   - ✅ Click the task title to toggle completion
   - ✅ Hover over tasks to see edit/delete buttons appear
   - ✅ Click the **drag handle** (⋮⋮) and drag to reorder pending tasks
   - ✅ Edit a task by clicking the edit button
   - ✅ Delete tasks with the delete (×) button
   - ✅ Notice completed tasks automatically move to the bottom
   - ✅ Toggle the theme button to switch between dark and light modes
   - ✅ Use the sort dropdown to sort by creation date or priority

## Common Issues & Solutions

### Backend Issues

**Problem: Port 5000 already in use**
```bash
# On Windows - Find process using port 5000
netstat -ano | findstr :5000

# Kill the process (replace PID with actual process ID)
taskkill /PID {PID} /F

# Or run backend on different port
# Edit launchSettings.json and change the port number
```

**Problem: Build errors or NuGet restore failures**
```bash
# Clear and rebuild
dotnet clean
dotnet nuget locals all --clear
dotnet restore
dotnet build
```

**Problem: API not accessible from frontend**
- Verify backend is running: Open http://localhost:5000 in browser
- Check CORS settings in `Program.cs` allow frontend port
- Verify API_URL in `Frontend/src/services/api.ts` matches backend port
- Check firewall/antivirus isn't blocking port 5000

**Problem: Swagger UI not loading**
- Backend must be running on http://localhost:5000
- Navigate directly to http://localhost:5000/swagger
- Check .NET version is 9.0 or higher: `dotnet --version`

### Frontend Issues

**Problem: Dependencies fail to install**
```bash
# Clear npm cache and reinstall
npm cache clean --force
rm -rf node_modules package-lock.json
npm install
```

**Problem: "Cannot find module" errors**
```bash
# Ensure you're in the Frontend directory
cd Frontend

# Reinstall dependencies
npm install

# Verify TypeScript is installed
npm list typescript
```

**Problem: Port 5173 already in use**
```bash
# Vite will automatically use next available port
# Or specify port in vite.config.ts

# On Windows, find and kill process
netstat -ano | findstr :5173
taskkill /PID {PID} /F
```

**Problem: Connection to backend fails**
```
Error: Failed to fetch from http://localhost:5000
```
- Ensure backend is running: `dotnet run` in Backend directory
- Check correct port in `Frontend/src/services/api.ts`
- Verify CORS is enabled in backend `Program.cs`
- Check browser console for detailed error

**Problem: TypeScript errors in IDE**
```bash
# Rebuild TypeScript
npm run build

# Check TypeScript configuration
cat tsconfig.json

# Install type definitions if missing
npm install --save-dev @types/node
```

**Problem: Dark theme not persisting**
- Clear browser localStorage: Press F12 → Application → LocalStorage → Clear All
- Check browser allows localStorage (not in private/incognito mode)
- Reload page after toggling theme

### Database/API Issues

**Problem: Data is lost after restart**
- This is expected! Backend uses in-memory database
- Data is not persisted to disk
- To fix: Configure persistent database (SQL Server, PostgreSQL, etc.)
- See README for migration to persistent database

**Problem: Can't create tasks**
```
Error 400: Bad Request
```
- Ensure task title is provided (required field)
- Title must be under 200 characters
- Description must be under 1000 characters
- Priority must be 0, 1, or 2

## Testing the API Independently

You can test the API without the frontend using Swagger UI or command-line tools.

### Using Swagger UI (Easiest)
1. Ensure backend is running on http://localhost:5000
2. Navigate to http://localhost:5000/swagger in your browser
3. You'll see all available endpoints with interactive documentation
4. Click "Try it out" on any endpoint to test it directly

### Using curl (Command Line)

**Get all tasks:**
```bash
curl http://localhost:5000/api/tasks
```

**Create a task:**
```bash
curl -X POST http://localhost:5000/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title":"Test Task",
    "description":"Task Description",
    "priority":0
  }'
```

**Get a specific task (replace {id} with actual ID):**
```bash
curl http://localhost:5000/api/tasks/{id}
```

**Toggle task completion:**
```bash
curl -X PATCH http://localhost:5000/api/tasks/{id}/toggle \
  -H "Content-Type: application/json"
```

**Update a task:**
```bash
curl -X PUT http://localhost:5000/api/tasks/{id} \
  -H "Content-Type: application/json" \
  -d '{
    "title":"Updated Title",
    "description":"Updated Description",
    "isCompleted":false,
    "priority":1
  }'
```

**Reorder tasks (bulk update):**
```bash
curl -X PUT http://localhost:5000/api/tasks/reorder \
  -H "Content-Type: application/json" \
  -d '[
    {"id":"{id1}","order":1},
    {"id":"{id2}","order":2},
    {"id":"{id3}","order":3}
  ]'
```

**Delete a task:**
```bash
curl -X DELETE http://localhost:5000/api/tasks/{id}
```

### Using PowerShell (Windows)

**Get all tasks:**
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/tasks" -Method Get
```

**Create a task:**
```powershell
$body = @{
    title = "Test Task"
    description = "Test Description"
    priority = 0
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/tasks" `
  -Method Post `
  -Headers @{"Content-Type"="application/json"} `
  -Body $body
```

**Toggle completion:**
```powershell
$taskId = "your-task-id-here"
Invoke-RestMethod -Uri "http://localhost:5000/api/tasks/$taskId/toggle" `
  -Method Patch
```
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/tasks" -Method Post -Body $body -ContentType "application/json"
```

## Development Tips

### Hot Reload

Both backend and frontend support hot reload:
- **Backend**: Code changes will automatically rebuild (if using `dotnet watch run`)
- **Frontend**: Changes are reflected immediately in the browser

### Backend with Watch Mode

```bash
cd Backend/TaskManager.API
dotnet watch run
```

### Database

The application uses EF Core In-Memory database, which means:
- ✅ No setup required
- ✅ Fast for development
- ⚠️ Data is lost when the API stops
- ⚠️ Not suitable for production

To switch to a persistent database:
1. Install EF Core provider (e.g., SQL Server, PostgreSQL)
2. Update connection string in appsettings.json
3. Update Program.cs to use the new provider
4. Run migrations: `dotnet ef migrations add Initial`
5. Update database: `dotnet ef database update`

## Environment Variables & Configuration

### Backend Configuration

**File**: `Backend/TaskManager.API/appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**File**: `Backend/TaskManager.API/Properties/launchSettings.json`
- Modify ports if needed
- Configure HTTP vs HTTPS settings
- Set environment (Development/Production)

### Frontend Configuration

**File**: `Frontend/src/services/api.ts`
```typescript
const API_URL = 'http://localhost:5000/api/tasks';
// Change 5000 if backend runs on different port
```

**File**: `Frontend/vite.config.ts`
```typescript
export default defineConfig({
  server: {
    port: 5173  // Change if 5173 is already in use
  }
})
```

## Development Tips

### Hot Reload
- **Frontend**: Changes auto-refresh in browser (Vite HMR)
- **Backend**: Requires restart for code changes
  - For auto-restart, install dotnet-watch: `dotnet tool install -g dotnet-watch`
  - Run with: `dotnet watch run`

### Debugging

**Backend (Visual Studio Code)**:
1. Install C# extension
2. Set breakpoints in .cs files
3. Press F5 to start debugging
4. API will pause at breakpoints

**Frontend (Browser DevTools)**:
1. Press F12 to open developer tools
2. Sources tab to set breakpoints
3. Console tab to view logs
4. Network tab to inspect API calls

### File Organization

Key files to know:
- Backend API: `Backend/TaskManager.API/Controllers/TasksController.cs`
- Backend Logic: `Backend/TaskManager.Application/Services/TaskService.cs`
- Frontend Store: `Frontend/src/stores/taskStore.ts`
- Frontend API: `Frontend/src/services/api.ts`
- Task Types: `Frontend/src/types/task.ts`
- Main Task Component: `Frontend/src/components/TaskItem.vue`

## Performance Tips

### Frontend Optimization
- Currently displays max 10 pending tasks (configurable in TaskList.vue)
- Consider pagination for large datasets
- Use browser DevTools Lighthouse for performance audit
- Check Network tab to monitor API call sizes

### Backend Optimization
- In-memory database is fast for development
- Switch to persistent DB for production
- Consider adding pagination to GET /api/tasks
- Add request/response logging for debugging

## Next Steps After Setup

1. ✅ Verify both backend and frontend are running
2. ✅ Test basic CRUD operations (Create, Read, Update, Delete)
3. ✅ Try drag-and-drop reordering
4. ✅ Test theme switching (Dark/Light)
5. ✅ Review code structure in both projects
6. ✅ Read through Component documentation in README.md
7. ✅ Explore API documentation via Swagger
8. ✅ Consider adding persistence (database migration)
9. ✅ Add authentication if needed
10. ✅ Deploy to cloud service (Azure, AWS, Heroku, etc.)

