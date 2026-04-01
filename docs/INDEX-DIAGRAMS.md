# 🎯 User Account Management Diagrams - Quick Index

## ⚡ Start Here

Bạn đang tìm sơ đồ PlantUML cho tính năng **Quản Lý Khóa/Mở Tài Khoản User (Block/Unlock User Account)** của admin?

✅ **Tất cả 3 sơ đồ đã được tạo!**

---

## 📊 3 Sơ Đồ Chính

### 1️⃣ **Activity Diagram** (Sơ Đồ Hoạt Động)
- **File:** [`docs/activity-diagram.puml`](./activity-diagram.puml)
- **Mục đích:** Hiển thị luồng từng bước chi tiết
- **Nội dung:**
  - Admin truy cập trang User Management
  - Admin tìm kiếm/lọc users
  - Chọn user → Mở block modal
  - Nhập reason → Confirm block
  - Xử lý database → Reload list
  - (Tương tự cho unblock)
- **Dùng khi:** Bạn muốn hiểu **cánh hoạt động** từng bước

### 2️⃣ **State Diagram** (Sơ Đồ Trạng Thái)
- **File:** [`docs/state-diagram.puml`](./state-diagram.puml)
- **Mục đích:** Hiển thị tất cả trạng thái user và chuyển đổi
- **States:**
  - `Creating` → `Active` → `Blocked` → `SoftDeleted` → `[End]`
- **Transitions** được hiển thị rõ ràng
- **Dùng khi:** Bạn muốn hiểu **design entities** và **state machine**

### 3️⃣ **Communication Diagram** (Sơ Đồ Giao Tiếp / Sequence)
- **File:** [`docs/communication-diagram.puml`](./communication-diagram.puml)
- **Mục đích:** Hiển thị cách components tương tác
- **Actors:**
  - Admin (người dùng)
  - UI (Blazor Component)
  - Service (Business Logic)
  - DbContext (EF Core)
  - Database (PostgreSQL)
- **6 Phases:** Load → Select → Submit → Update → Reload → Display
- **Dùng khi:** Bạn muốn hiểu **system integration** giữa các tầng

---

## 📚 Tài Liệu Hỗ Trợ

| File | Mục Đích | Chi tiết |
|------|----------|---------|
| [`DIAGRAMS-SUMMARY.md`](./DIAGRAMS-SUMMARY.md) | 📋 Tóm tắt nhanh | Key points, rules, components |
| [`README-DIAGRAMS.md`](./README-DIAGRAMS.md) | 📖 Hướng dẫn chi tiết | Cách sử dụng, tools, source code ref |
| [`diagrams-user-management.md`](./diagrams-user-management.md) | 🔧 Tài liệu kỹ thuật | Code snippets, full details |

---

## 🚀 Cách Sử Dụng

### **Nhanh nhất (1 phút)**
1. Đi đến: http://www.plantuml.com/plantuml/uml/
2. Copy nội dung từ file `.puml`
3. Paste → Xem diagram
4. Download SVG/PNG

### **Cách thứ 2**
```bash
# Cài PlantUML
sudo apt install plantuml

# Generate diagram
plantuml docs/activity-diagram.puml -o output/
```

### **Cách thứ 3 (VS Code)**
1. Cài extension: `jebbs.plantuml`
2. Mở file `.puml`
3. Alt + D (preview)

---

## 🔍 Source Code Reference

### **Key Components**

**User Model:**
```csharp
// BusinessObject/Models/Identity/User.cs
public class User : SoftDeletableEntity {
    public bool IsActive { get; set; } = true;     // false = blocked
    public DateTime? DeletedAt { get; set; }       // soft delete
}
```

**Service:**
```csharp
// BusinessLogicLayer/Services/UserManagementService.cs
public Task<ApiResponse<object?>> ChangeUserStatusAsync(
    long userId, 
    bool isActive,  // true = unblock, false = block
    string reason);
```

**UI:**
```razor
// LearnmateSolution/Components/Pages/Admin/Admin_UserManagement.razor
@page "/admin/users"
// Modal actions: ConfirmBlock(), ConfirmUnblock()
```

**Database:**
```sql
-- PostgreSQL: users table
CREATE TABLE users (
    is_active BOOLEAN DEFAULT true,     -- false = blocked
    deleted_at TIMESTAMPTZ              -- null = active
)
```

---

## 📊 Diagram Features

### ✅ **Activity Diagram**
- [x] Parallel flows (Block vs Unblock)
- [x] Decision points (validation checks)
- [x] Database operations
- [x] Error handling
- [x] UI feedback & reload

### ✅ **State Diagram**
- [x] 4 States (Creating, Active, Blocked, SoftDeleted)
- [x] State transitions
- [x] Entry/Exit descriptions
- [x] Duration notes
- [x] Business rules per state

### ✅ **Communication Diagram**
- [x] 5 Participants (Admin, UI, Service, DbContext, DB)
- [x] 46 numbered interactions
- [x] 6 distinct phases
- [x] SQL queries shown
- [x] Error alternatives
- [x] Message parameters explicit

---

## 🎯 Business Logic Summary

```
INPUT: Admin clicks "Block" button on user row
  ↓
VALIDATION: blockReason must not be empty
  ↓
SERVICE CALL: ChangeUserStatusAsync(userId, false, "Reason")
  ↓
DATABASE: UPDATE users SET is_active=false WHERE id=?
  ↓
RESPONSE: Success message + reload list
  ↓
OUTPUT: User status changes to "Blocked"
```

---

## ❓ FAQ

**Q: Mình nên dùng diagram nào?**
- Activity: Hiểu luồng hoạt động
- State: Thiết kế database/entities
- Communication: Tích hợp system components

**Q: File nào là chính?**
- `activity-diagram.puml` (chi tiết nhất)
- `communication-diagram.puml` (toàn cảnh hệ thống)
- `state-diagram.puml` (thiết kế entities)

**Q: Có thể chỉnh sửa diagrams không?**
- Có! Tất cả `.puml` files có thể edit
- Dùng PlantUML syntax (xem [docs](https://plantuml.com/))

**Q: Có bao gồm teacher violation logic không?**
- Activity/State/Communication bao gồm
- TeacherComplianceService xử lý riêng (APPROVED/REJECTED)

---

## 📝 Thông Tin Tài Liệu

- **Created:** March 2026
- **Source:** Live code analysis (LearnMate project)
- **Format:** PlantUML (UML standard)
- **Language:** English + Vietnamese
- **Status:** ✅ Complete & Verified

---

## 📂 File Structure

```
docs/
├── 📄 INDEX-DIAGRAMS.md                    ← Bạn đang xem
├── 📊 activity-diagram.puml                ← UML Activity (PlantUML)
├── 🔄 state-diagram.puml                   ← UML State (PlantUML)
├── 🔗 communication-diagram.puml           ← UML Communication (PlantUML)
├── 📋 DIAGRAMS-SUMMARY.md                  ← Quick reference
├── 📖 README-DIAGRAMS.md                   ← Full guide
└── 🔧 diagrams-user-management.md          ← Mermaid versions + details
```

---

## 🎓 Học Thêm

- [PlantUML Documentation](https://plantuml.com/)
- [UML Activity Diagrams](https://www.uml-diagrams.org/activity-diagrams.html)
- [UML State Diagrams](https://www.uml-diagrams.org/state-machine-diagrams.html)
- [UML Communication Diagrams](https://www.uml-diagrams.org/communication-diagrams.html)

---

**Cần giúp? 👉** Xem [`README-DIAGRAMS.md`](./README-DIAGRAMS.md) hoặc [`DIAGRAMS-SUMMARY.md`](./DIAGRAMS-SUMMARY.md)

**Ready to use? 👉** Copy content từ `.puml` files vào PlantUML editor!

