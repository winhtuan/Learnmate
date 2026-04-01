## 📊 User Account Management Diagrams - PlantUML

This folder contains **3 detailed PlantUML diagrams** for the User Account Management feature (Lock/Unlock/Suspend User Accounts) in the LearnMate platform.

---

## 📁 Files

### 1. **activity-diagram.puml** 
- **Type:** UML Activity Diagram
- **Purpose:** Shows the complete workflow from Admin opening the page to blocking/unblocking a user
- **Scope:** Step-by-step flow of activities and decision points
- **Key Elements:**
  - Admin navigates to /admin/users
  - User list loaded via GetUsersAsync()
  - Admin searches/filters users
  - Two parallel paths: Block Path vs Unblock Path
  - Validation checks
  - Database update
  - UI reload with success message

### 2. **state-diagram.puml**
- **Type:** UML State Diagram
- **Purpose:** Shows all possible states of a User account and transitions between them
- **Scope:** User account lifecycle
- **States:**
  - **Creating:** Initial state when user is created
  - **Active:** IsActive = true (normal operation)
  - **Blocked:** IsActive = false (cannot login)
  - **SoftDeleted:** DeletedAt ≠ null (archived)
- **Transitions:**
  - Creating → Active (default)
  - Active ⇄ Blocked (admin block/unblock)
  - Active/Blocked → SoftDeleted (soft delete)
  - Deleted → [End] (archived)

### 3. **communication-diagram.puml**
- **Type:** UML Communication Diagram (Sequence variant)
- **Purpose:** Shows component interactions and message passing
- **Participants:**
  - Admin (actor)
  - Blazor UI (Admin_UserManagement.razor)
  - UserManagementService (BusinessLogicLayer)
  - AppDbContext (EF Core ORM)
  - PostgreSQL Database
- **Flow:**
  - 6 Phases: Load → Select → Submit → Update → Reload → Display
  - Numbered messages showing exact order of calls
  - SQL queries and responses
  - Error handling alternatives

---

## 🚀 How to Use

### **Option 1: View Online (Recommended)**
1. Visit **[PlantUML Online Editor](http://www.plantuml.com/plantuml/uml/)**
2. Copy content from the `.puml` file
3. Paste into the editor
4. Select output format (SVG, PNG, PDF)

### **Option 2: Local PlantUML**
```bash
# Install PlantUML (requires Java)
apt-get install plantuml  # Ubuntu/Debian
brew install plantuml      # macOS

# Generate diagram
plantuml activity-diagram.puml -o output/
plantuml state-diagram.puml -o output/
plantuml communication-diagram.puml -o output/
```

### **Option 3: VS Code Extension**
1. Install: [PlantUML extension](https://marketplace.visualstudio.com/items?itemName=jebbs.plantuml)
2. Open `.puml` file
3. Alt + D to preview

---

## 📋 Source Code Reference

The diagrams are based on the following source files:

### **Data Model:**
- [BusinessObject/Models/Identity/User.cs](../BusinessObject/Models/Identity/User.cs)
  ```csharp
  public class User : SoftDeletableEntity
  {
      public string Email { get; set; }
      public bool IsActive { get; set; } = true;  // Key field for block/unblock
      public UserRole Role { get; set; }
      public DateTime? DeletedAt { get; set; }    // Soft delete
  }
  ```

### **Service Layer:**
- [BusinessLogicLayer/Services/UserManagementService.cs](../BusinessLogicLayer/Services/UserManagementService.cs)
  ```csharp
  public class UserManagementService : IUserManagementService
  {
      public async Task<IEnumerable<UserRowDto>> GetUsersAsync(
          string roleFilter = "", 
          string statusFilter = "", 
          string searchQuery = "")
      
      public async Task<ApiResponse<object?>> ChangeUserStatusAsync(
          long userId, 
          bool isActive,        // true = unblock, false = block
          string reason, 
          string notes = "")
  }
  ```

### **UI Component:**
- [LearnmateSolution/Components/Pages/Admin/Admin_UserManagement.razor](../LearnmateSolution/Components/Pages/Admin/Admin_UserManagement.razor)
  - Modal actions: ConfirmBlock(), ConfirmUnblock(), ConfirmDelete()
  - Filter/search controls
  - User table display

### **Database Schema:**
- [PostgreSQL Table: users](../docs/entities.md)
  ```sql
  CREATE TABLE users (
      id BIGINT PRIMARY KEY,
      email VARCHAR(255) UNIQUE NOT NULL,
      password_hash VARCHAR(255) NOT NULL,
      is_active BOOLEAN DEFAULT true,
      deleted_at TIMESTAMPTZ,
      role VARCHAR(50),
      updated_at TIMESTAMPTZ,
      ...
  )
  ```

---

## 🎯 Key Business Logic

### **User Status Management**

| State | IsActive | DeletedAt | Can Login? | Reason |
|-------|----------|-----------|-----------|--------|
| Creating | true | null | No (not yet) | Initial creation |
| Active | true | null | ✅ Yes | Normal operation |
| Blocked | false | null | ❌ No | Admin action: violation/abuse |
| SoftDeleted | false/true | ≠ null | ❌ No | Admin delete (archived) |

### **Block Reasons**
- Teacher Violation (policy/compliance breach)
- Account Abuse (spam/harassment)
- Policy Breach (T&C violation)
- Compliance Rejection (document issues)
- Payment Default (financial issues)

### **Method Flow**

```
Admin UI Action
    ↓
Form Validation (Reason required)
    ↓
Service.ChangeUserStatusAsync(userId, isActive, reason)
    ↓
DbContext.Users.FindAsync(userId)
    ↓
User.IsActive = isActive
    ↓
DbContext.SaveChangesAsync()
    ↓
Response to UI
    ↓
Reload User List
    ↓
Display Updated Status
```

---

## 🔍 Diagram Details

### **Activity Diagram - Key Decision Points**
1. **Validation Check:** blockReason != empty?
2. **User Existence:** User Found?
3. **Success/Error:** Determines next action

### **State Diagram - Key Transitions**
- **Creating → Active:** Automatic on user creation
- **Active ⇄ Blocked:** Manual admin action with reason
- **Active/Blocked → SoftDeleted:** Soft delete operation
- **SoftDeleted → [*]:** End of lifecycle (archived)

### **Communication Diagram - Key Phases**
1. **Phase 1:** Load User List (GetUsersAsync)
2. **Phase 2:** Admin selects user, opens modal
3. **Phase 3:** Admin submits form (validation)
4. **Phase 4:** Database update (UserManagementService)
5. **Phase 5:** UI reloads and displays updated status
6. **Phase 6:** Unblock flow (reverse of block)

---

## 🛠️ Integration Points

### **API Endpoints** (if REST API exists)
```
POST /api/admin/users/{id}/block
  Body: { reason: string, notes?: string }
  Returns: ApiResponse

POST /api/admin/users/{id}/unblock
  Body: { reason: string }
  Returns: ApiResponse

GET /api/admin/users?roleFilter=&statusFilter=&searchQuery=
  Returns: List<UserRowDto>
```

### **DTOs Used**
- `UserRowDto` - Table display
- `NewUserRequestDto` - User creation
- `ApiResponse<T>` - Response wrapper

### **Interfaces**
- `IUserManagementService` - Business logic contract

---

## 📝 Notes for Developers

1. **Audit Trail:** Currently block/unblock reason stored in notes. Consider adding dedicated `AuditLog` table for compliance.

2. **Teacher Violation:** TeacherComplianceService handles compliance status separately. Integration needed for automated blocking on rejection.

3. **Soft Delete:** Users are soft-deleted (DeletedAt set) not physically removed, allowing data recovery and compliance.

4. **Error Handling:** Both methods include null checks and ApiResponse wrapper for consistent error handling.

5. **Database:** PostgreSQL uses snake_case columns (`is_active`, `deleted_at`). EF Core applies `UseSnakeCaseNamingConvention()`.

6. **Authorization:** Admin_UserManagement.razor requires ADMIN role. Add [Authorize(Roles = "ADMIN")] if using API endpoints.

---

## 📚 Related Documentation

- [CLAUDE.md](../CLAUDE.md) - Project structure and conventions
- [entities.md](../docs/entities.md) - Database schema
- [database-relationships.md](../docs/database-relationships.md) - ER diagram

---

**Last Updated:** March 2026
**Author:** AI Assistant (Claude)
**Status:** Generated based on source code analysis
