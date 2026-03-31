# 📊 PlantUML Diagrams - User Account Management (Block/Unlock)

## ✅ Diagrams Created

Dựa trên phân tích **source code** của project LearnMate, tôi đã tạo 3 sơ đồ PlantUML hoàn chỉnh cho tính năng **Quản Lý Khóa/Mở Tài Khoản User** (dành cho Admin):

---

## 📋 1️⃣ Activity Diagram (Sơ Đồ Hoạt Động)

**File:** `docs/activity-diagram.puml`

**Mô tả:** Biểu diễn luồng từng bước chi tiết từ khi Admin truy cập trang quản lý user đến khi tài khoản được khóa hoặc mở khóa.

**Các bước chính:**

```
START
  ↓
Admin Access /admin/users
  ↓
Load User List (GetUsersAsync)
  ↓
Display Table
  ↓
Admin Search/Filter
  ↓
BRANCH (2 paths parallel):
  
  PATH A: BLOCK USER
  └─ Click "Block" button
    └─ Open Modal
    └─ Enter Reason + Notes
    └─ Click "Confirm Block"
    └─ Validate (reason not empty)
    └─ Call ChangeUserStatusAsync(userId, false, reason)
    └─ Find user in database
    └─ Set User.IsActive = false
    └─ Save to PostgreSQL
    └─ Return success
  
  PATH B: UNBLOCK USER
  └─ (Tương tự, nhưng isActive = true)

END: Reload list + Display result
```

**Key Decision Points:**
- ✓ Validate blockReason != empty?
- ✓ User found in database?
- ✓ Safe to update

---

## 🔄 2️⃣ State Diagram (Sơ Đồ Trạng Thái)

**File:** `docs/state-diagram.puml`

**Mô tả:** Biểu diễn tất cả các trạng thái của một User account và các chuyển đổi trạng thái.

**States:**

```
┌─────────────┐
│  Creating   │  (Đang tạo user mới)
│ IsActive=?  │
└──────┬──────┘
       │ User.IsActive = true
       ↓
┌──────────────────────────────┐
│  ACTIVE                      │  ✅ Can Login
│  IsActive = true             │  ✅ Full Access
│  DeletedAt = null            │  Duration: Unlimited
└───────────┬──────────────┬───┘
            │              │
    (Block) │              │ (Delete)
            ↓              ↓
   ┌──────────────┐   ┌──────────────┐
   │  BLOCKED     │   │ SOFT_DELETED │
   │ IsActive=    │   │ DeletedAt ≠  │
   │ false        │   │ null         │
   │ ❌ Cannot    │   │ ❌ Cannot    │
   │ Login        │   │ Login        │
   └──────┬───────┘   └──────┬───────┘
          │                  │
    (Unb) │                  │ (Archive)
          │                  │
          └──────────────────→ [END]
```

**Transitions:**
- `Creating → Active`: Tự động khi tạo user
- `Active → Blocked`: Admin block lý do vi phạm
- `Blocked → Active`: Admin unblock (appeal approved)
- `Active/Blocked → SoftDeleted`: Admin soft delete (soft xóa)
- `SoftDeleted → END`: Tài khoản được lưu trữ

**Block Reasons:**
- Teacher Violation (teacher vi phạm)
- Account Abuse (lạm dụng tài khoản)
- Policy Breach (vi phạm điều khoản)
- Compliance Rejection (từ chối xác thực)

---

## 🔗 3️⃣ Communication Diagram (Sơ Đồ Giao Tiếp)

**File:** `docs/communication-diagram.puml`

**Mô tả:** Biểu diễn cách các components tương tác, các message được gửi, và thứ tự xử lý.

**Components:**
```
    Admin
      ↓
  [Blazor UI]  ← → [UserManagementService] ← → [AppDbContext/EF Core] ← → [PostgreSQL DB]
```

**6 Phases:**

### **Phase 1: Load User List**
```
1. Admin → UI: Navigate to /admin/users
2. UI → Service: GetUsersAsync()
3. Service → DbContext: Query with filters
4. DbContext → DB: SELECT * FROM users JOIN profiles
5. DB → DbContext: User records
6. DbContext → Service: IEnumerable<User>
7. Service → UI: List<UserRowDto> (mapped)
8. UI → Admin: Display table
```

### **Phase 2: Select User & Open Modal**
```
9. Admin → UI: Click "Block" button
10. UI → UI: OpenBlockModal(selectedUser)
11. UI → Admin: Show modal form
```

### **Phase 3: Submit Block Request**
```
12. Admin → UI: Enter reason + notes, click Confirm
13. UI → UI: Validate (reason != empty)
14. UI → Service: ChangeUserStatusAsync(userId, false, reason)
```

### **Phase 4: Database Update**
```
15. Service → DbContext: FindAsync(userId)
16. DbContext → DB: SELECT user WHERE id=?
17. DB → DbContext: User entity
18. Service → Service: user.IsActive = false
19. Service → DbContext: Update(user)
20. DbContext → DB: UPDATE users SET is_active=false
21. DB → DbContext: Success
22. Service → UI: ApiResponse.Ok()
```

### **Phase 5: UI Response & Reload**
```
23. UI → UI: Close modal
24. UI → Service: GetUsersAsync() [RELOAD]
25. ... (Repeat Phase 1 query)
26. UI → Admin: Display updated list with new status
```

### **Phase 6: Unblock (Similar)**
```
Identical to Block phases, but:
- ChangeUserStatusAsync(userId, **true**, reason)
- User.IsActive = **true**
- Status → "Active"
```

---

## 🗄️ Database Details

### **User Table Schema (PostgreSQL)**

```sql
CREATE TABLE users (
    id BIGINT PRIMARY KEY,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL,
    is_active BOOLEAN DEFAULT true,    ← KEY FIELD (false = blocked)
    deleted_at TIMESTAMPTZ NULL,        ← NULL = active, NOT NULL = deleted
    updated_at TIMESTAMPTZ NOT NULL,
    created_at TIMESTAMPTZ NOT NULL,
    ...
);
```

---

## 💻 Source Code Files Used

### **Data Model:**
- `BusinessObject/Models/Identity/User.cs`
  - Property: `public bool IsActive { get; set; } = true;`
  - Property: `public DateTime? DeletedAt { get; set; }` (from SoftDeletableEntity)

### **Service Layer:**
- `BusinessLogicLayer/Services/UserManagementService.cs`
  ```csharp
  // Get users with filters
  public async Task<IEnumerable<UserRowDto>> GetUsersAsync(
      string roleFilter = "", 
      string statusFilter = "", 
      string searchQuery = "")
  
  // Block or unblock user
  public async Task<ApiResponse<object?>> ChangeUserStatusAsync(
      long userId, 
      bool isActive,        // true = unblock, false = block
      string reason, 
      string notes = "")
  
  // Soft delete user
  public async Task<ApiResponse<object?>> DeleteUserAsync(long userId)
  ```

### **UI Component:**
- `LearnmateSolution/Components/Pages/Admin/Admin_UserManagement.razor`
  - Methods: `OpenBlockModal()`, `ConfirmBlock()`, `ConfirmUnblock()`, etc.
  - Modal validations and error handling
  - User table display with filters

---

## 🛠️ How to Use These Diagrams

### **Option 1: View in PlantUML Online Editor (Fastest)**
1. Go to: http://www.plantuml.com/plantuml/uml/
2. Copy content từ file `.puml`
3. Paste vào online editor
4. Get SVG/PNG/PDF output

### **Option 2: Generate Locally**
```bash
# Install PlantUML (requires Java)
sudo apt-get install plantuml  # Linux
brew install plantuml          # macOS

# Generate diagrams
plantuml docs/activity-diagram.puml -o output/
plantuml docs/state-diagram.puml -o output/
plantuml docs/communication-diagram.puml -o output/
```

### **Option 3: VS Code**
1. Install extension: "PlantUML"
2. Open `.puml` file
3. Alt + D (preview)
4. Right-click → Export

---

## 📂 Files Created

```
docs/
├── activity-diagram.puml              ← Activity diagram (PlantUML)
├── state-diagram.puml                 ← State diagram (PlantUML)
├── communication-diagram.puml         ← Communication diagram (PlantUML)
├── diagrams-user-management.md        ← Mermaid versions + detailed notes
└── README-DIAGRAMS.md                 ← Complete guide
```

---

## ✨ Features Covered

### ✅ **Block User**
- Admin selects user
- Opens block modal
- Enters block reason
- System validates
- Database updates `IsActive = false`
- List reloads with "Blocked" status

### ✅ **Unblock User**
- Similar flow
- Database updates `IsActive = true`
- Status returns to "Active"

### ✅ **Soft Delete User**
- Sets `DeletedAt = DateTime.UtcNow()`
- User record preserved (not physically deleted)
- Useful for compliance/audit trail

### ✅ **Filter & Search**
- By Role (Student, Teacher, Admin)
- By Status (Active, Blocked)
- By Email/Name/ID

### ✅ **Error Handling**
- Validation checks (reason required)
- User existence check
- Database error responses

---

## 🎯 Key Business Rules

| Rule | Implementation |
|------|-----------------|
| Block requires reason | UI validation + DB notes |
| User status = IsActive | `IsActive=true` (Active), `IsActive=false` (Blocked) |
| Soft delete = archive | `DeletedAt` field set, record preserved |
| Admin only | LearnmateSolution checks ADMIN role |
| Password hashed | BCrypt.Net.BCrypt.HashPassword() |
| Case-insensitive email | `.ToLowerInvariant()` |

---

## 📊 Diagram Comparison

| Aspect | Activity | State | Communication |
|--------|----------|-------|-----------------|
| **Focus** | Workflow/Steps | Entity States | Component Interaction |
| **Timeline** | Sequential | Abstract | Sequence of messages |
| **Shows** | Actions/Decisions | Transitions | Participants & calls |
| **Best for** | Understanding flow | Design states | API interaction |

---

## 🔗 Related Files

- [CLAUDE.md](../CLAUDE.md) - Project structure
- [entities.md](../docs/entities.md) - Database schema
- [database-relationships.md](../docs/database-relationships.md) - ER diagram

---

**Status:** ✅ Complete
**Date:** March 2026
**Source:** Live code analysis from LearnMate project
**Format:** PlantUML (industry standard for UML diagrams)

