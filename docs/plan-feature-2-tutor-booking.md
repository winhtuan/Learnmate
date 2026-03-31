# Kế hoạch: Chức năng 2 — Đặt lịch với gia sư

## Tổng quan

**"Tutor" (gia sư) = User có `Role = TEACHER`** kèm `TeacherProfile`. Không có role mới hay entity
riêng. `TeacherProfile` đã có các trường: `FullName`, `AvatarUrl`, `Bio`, `Subjects`
(comma-separated string), `HourlyRate`, `RatingAvg`, `TotalRatingCount`.

`FindTutorPage.razor` hiện dùng **hardcode data** (`_tutors` list). Cần:
1. Kết nối data thật từ `TeacherProfile` (filter by subject + hourly rate)
2. Luồng booking: student gửi yêu cầu → teacher accept/decline (teacher side chưa implement) → tạo Class 1-1

---

## Thứ tự implement

```
1. Tạo BookingRequestStatus enum
2. Tạo TutorBookingRequest entity + EF configuration
3. Viết migration AddTutorBookingRequests
4. Mở rộng ITeacherProfileRepository + TeacherProfileRepository (thêm GetAllTeachersAsync)
5. Tạo ITutorBookingRepository + TutorBookingRepository
6. Tạo DTOs (Tutor/TutorDto.cs)
7. Tạo ITutorService + TutorService
8. Đăng ký DI trong ServiceCollectionExtensions.cs
9. Tạo TutorController (4 endpoints)
10. Kết nối FindTutorPage với API thật
11. Thêm nút Book vào TutorQuickView
12. Tạo BookingModal.razor
13. Tạo MyBookingsPage.razor (/student/bookings)
```

---

## 1. Database

### Enum mới

**File:** `BusinessObject/Enum/BookingRequestStatus.cs`

```csharp
namespace BusinessObject.Enum;

public enum BookingRequestStatus
{
    PENDING,
    ACCEPTED,
    DECLINED,
    CANCELLED,
}
```

### Entity mới

**File:** `BusinessObject/Models/ClassManagement/TutorBookingRequest.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("tutor_booking_requests")]
public class TutorBookingRequest : SoftDeletableEntity
{
    // StudentId → User với Role = STUDENT
    public long StudentId { get; set; }

    // TeacherId → User với Role = TEACHER (chính là "tutor")
    public long TeacherId { get; set; }

    [Required]
    public DateTime RequestedStartTime { get; set; }

    [Required]
    public DateTime RequestedEndTime { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    [Required]
    public BookingRequestStatus Status { get; set; } = BookingRequestStatus.PENDING;

    // Sau khi teacher ACCEPTED, hệ thống tạo Class 1-1 và gán vào đây
    public long? ResultClassId { get; set; }

    [ForeignKey("StudentId")]
    public User Student { get; set; } = null!;

    [ForeignKey("TeacherId")]
    public User Teacher { get; set; } = null!;

    [ForeignKey("ResultClassId")]
    public Class? ResultClass { get; set; }
}
```

> **Lưu ý:** `TeacherId` chỉ là FK → `users.id`; việc đó là user có `Role = TEACHER` là nghiệp
> vụ (không có FK riêng tới `teacher_profiles`). Pattern này nhất quán với `Class.TeacherId`.

### EF Configuration

**File:** `DataAccessLayer/Data/Configurations/TutorBookingConfiguration.cs`

```csharp
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

public class TutorBookingConfiguration : IEntityTypeConfiguration<TutorBookingRequest>
{
    public void Configure(EntityTypeBuilder<TutorBookingRequest> b)
    {
        b.Property(x => x.Status).HasConversion<string>();

        b.HasIndex(x => new { x.StudentId, x.TeacherId });
        b.HasIndex(x => x.Status);

        b.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ResultClass)
            .WithMany()
            .HasForeignKey(x => x.ResultClassId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
```

### AppDbContext

Thêm vào `DataAccessLayer/Data/AppDbContext.cs`:
```csharp
public DbSet<TutorBookingRequest> TutorBookingRequests => Set<TutorBookingRequest>();
```

### Migration

```bash
dotnet ef migrations add AddTutorBookingRequests --project DataAccessLayer --startup-project LearnmateSolution
dotnet ef database update --project DataAccessLayer --startup-project LearnmateSolution
```

---

## 2. Repository layer

### Mở rộng ITeacherProfileRepository

Thêm method vào `DataAccessLayer/Repositories/Interfaces/Identity/ITeacherProfileRepository.cs`:

```csharp
/// <summary>
/// Lấy tất cả giáo viên (Role = TEACHER) có TeacherProfile, với optional filter.
/// subjectFilter: tìm trong TeacherProfile.Subjects (comma-separated string).
/// maxRate: lọc HourlyRate <= maxRate.
/// </summary>
Task<IReadOnlyList<TeacherProfile>> GetAllTeachersAsync(
    string? subjectFilter = null,
    decimal? maxRate = null,
    CancellationToken ct = default
);
```

**Implementation** (thêm vào `TeacherProfileRepository.cs`):
```csharp
public async Task<IReadOnlyList<TeacherProfile>> GetAllTeachersAsync(
    string? subjectFilter = null,
    decimal? maxRate = null,
    CancellationToken ct = default)
{
    var query = db.TeacherProfiles
        .AsNoTracking()
        .Include(p => p.User)
        .Where(p => p.User.IsActive && p.User.DeletedAt == null);

    if (!string.IsNullOrWhiteSpace(subjectFilter))
        query = query.Where(p => EF.Functions.ILike(p.Subjects, $"%{subjectFilter}%"));

    if (maxRate.HasValue)
        query = query.Where(p => p.HourlyRate <= maxRate.Value);

    return await query
        .OrderByDescending(p => p.RatingAvg)
        .ThenByDescending(p => p.TotalRatingCount)
        .ToListAsync(ct);
}
```

> **Lưu ý:** Filter `p.User.IsActive` đảm bảo chỉ hiển thị teacher đang hoạt động.
> Không cần filter `Role == TEACHER` vì `teacher_profiles` chỉ được tạo cho TEACHER users.

### ITutorBookingRepository + TutorBookingRepository

**File:** `DataAccessLayer/Repositories/Interfaces/ClassManagement/ITutorBookingRepository.cs`

```csharp
using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface ITutorBookingRepository
{
    Task<TutorBookingRequest> CreateAsync(TutorBookingRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<TutorBookingRequest>> GetByStudentIdAsync(long studentId, CancellationToken ct = default);
    Task<TutorBookingRequest?> GetByIdAsync(long id, CancellationToken ct = default);
    Task UpdateAsync(TutorBookingRequest request, CancellationToken ct = default);
}
```

**File:** `DataAccessLayer/Repositories/ClassManagement/TutorBookingRepository.cs`

```csharp
using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TutorBookingRepository(AppDbContext db) : ITutorBookingRepository
{
    public async Task<TutorBookingRequest> CreateAsync(TutorBookingRequest request, CancellationToken ct = default)
    {
        db.TutorBookingRequests.Add(request);
        await db.SaveChangesAsync(ct);
        return request;
    }

    public async Task<IReadOnlyList<TutorBookingRequest>> GetByStudentIdAsync(long studentId, CancellationToken ct = default) =>
        await db.TutorBookingRequests
            .AsNoTracking()
            .Where(r => r.StudentId == studentId && r.DeletedAt == null)
            .Include(r => r.Teacher).ThenInclude(t => t.TeacherProfile)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

    public async Task<TutorBookingRequest?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await db.TutorBookingRequests
            .Include(r => r.Teacher).ThenInclude(t => t.TeacherProfile)
            .Include(r => r.Student).ThenInclude(s => s.StudentProfile)
            .FirstOrDefaultAsync(r => r.Id == id && r.DeletedAt == null, ct);

    public async Task UpdateAsync(TutorBookingRequest request, CancellationToken ct = default)
    {
        db.TutorBookingRequests.Update(request);
        await db.SaveChangesAsync(ct);
    }
}
```

---

## 3. BLL layer

### DTOs

**File:** `BusinessLogicLayer/DTOs/Tutor/TutorDto.cs`

```csharp
namespace BusinessLogicLayer.DTOs.Tutor;

/// <summary>
/// Chỉ chứa các trường thực sự tồn tại trong TeacherProfile + User.
/// Các trường UI-only (IsVerified, Location, TotalLessons, AvailableSlots)
/// được xử lý ở Blazor layer với giá trị mặc định.
/// </summary>
public record TutorSummaryDto(
    long TeacherUserId,       // = TeacherProfile.UserId
    string FullName,          // = TeacherProfile.FullName
    string? AvatarUrl,        // = TeacherProfile.AvatarUrl ?? User.AvatarUrl
    string Subjects,          // comma-separated, e.g. "Math, Physics"
    decimal HourlyRate,       // = TeacherProfile.HourlyRate
    double RatingAvg,         // = (double)TeacherProfile.RatingAvg
    int TotalRatingCount,     // = TeacherProfile.TotalRatingCount
    string? Bio               // = TeacherProfile.Bio
);

public record CreateBookingRequestDto
{
    public long TeacherId { get; set; }
    public DateTime RequestedStartTime { get; set; }
    public DateTime RequestedEndTime { get; set; }
    public string? Note { get; set; }
}

public record BookingRequestDto(
    long Id,
    long TeacherId,
    string TeacherName,
    string? TeacherAvatarUrl,
    DateTime RequestedStartTimeLocal,
    DateTime RequestedEndTimeLocal,
    string Status,
    string? Note,
    DateTime CreatedAtLocal
);
```

### Interface + Service

**File:** `BusinessLogicLayer/Services/Interfaces/ClassManagement/ITutorService.cs`

```csharp
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Tutor;

namespace BusinessLogicLayer.Services.Interfaces;

public interface ITutorService
{
    Task<ApiResponse<IReadOnlyList<TutorSummaryDto>>> GetTutorsAsync(
        string? subject, decimal? maxRate, CancellationToken ct = default);

    Task<ApiResponse<BookingRequestDto>> CreateBookingRequestAsync(
        long studentId, CreateBookingRequestDto dto, CancellationToken ct = default);

    Task<ApiResponse<IReadOnlyList<BookingRequestDto>>> GetStudentBookingsAsync(
        long studentId, CancellationToken ct = default);

    Task<ApiResponse<bool>> CancelBookingAsync(
        long bookingId, long studentId, CancellationToken ct = default);
}
```

**File:** `BusinessLogicLayer/Services/ClassManagement/TutorService.cs`

Key logic:
- `GetTutorsAsync`: gọi `ITeacherProfileRepository.GetAllTeachersAsync(subject, maxRate)` → map sang `TutorSummaryDto`
  - `AvatarUrl = profile.AvatarUrl ?? profile.User.AvatarUrl`
  - `RatingAvg = (double)profile.RatingAvg`
- `CreateBookingRequestAsync`:
  - Validate: `RequestedStartTime < RequestedEndTime`, cả hai đều ở tương lai
  - Tạo `TutorBookingRequest` với `Status = PENDING`, normalize time về UTC
- `GetStudentBookingsAsync`: gọi repo → map, dùng `r.Teacher.TeacherProfile?.FullName ?? r.Teacher.Email`
- `CancelBookingAsync`: kiểm tra `request.StudentId == studentId && request.Status == PENDING` → set `CANCELLED`

### DI Registration

Thêm vào `BusinessLogicLayer/ServiceCollectionExtensions.cs`:

```csharp
services.AddScoped<ITutorBookingRepository, TutorBookingRepository>();
services.AddScoped<ITutorService, TutorService>();
```

---

## 4. API Controller

**File:** `LearnmateSolution/Controllers/TutorController.cs`

```
GET    /api/tutors                    — Danh sách giáo viên (query: subject, maxRate)
POST   /api/tutors/{teacherId}/book   — Tạo booking request (student only)
GET    /api/bookings/my               — Lịch booking của student
DELETE /api/bookings/{id}             — Student hủy booking (chỉ PENDING)
```

Tất cả endpoints đều yêu cầu `[Authorize]`. Lấy `studentId` từ JWT claim
`ClaimTypes.NameIdentifier` (đã có trong AuthController pattern hiện tại).

---

## 5. UI / Blazor

### Gap giữa TutorSummaryDto và TutorListItem

`TutorListItem` (UI model hiện có) có nhiều trường không tồn tại trong DB:

| Field UI | DB | Giải pháp |
|----------|-----|-----------|
| `IsVerified` | ✗ | `false` (hoặc `User.IsActive`) |
| `IsSuperTutor` | ✗ | `false` |
| `Location` | ✗ | `"Online"` |
| `ResponseTime` | ✗ | `""` |
| `TotalLessons` | ✗ | `0` |
| `AvailableSlots` | ✗ | `[]` — time filter sẽ không hoạt động với real data |
| `Availability` | ✗ | `AvailabilityStatus.NextSlot` |
| `StudentCount` | ✗ | `0` |
| `Rating` | ✓ `RatingAvg` | map trực tiếp |
| `ReviewCount` | ✓ `TotalRatingCount` | map trực tiếp |
| `Subjects` string[] | ✓ `Subjects` string (CSV) | `.Split(',')` và `.Trim()` |

Mapper trong `FindTutorPage.razor`:
```csharp
private static TutorListItem MapToListItem(TutorSummaryDto dto) => new(
    Id:               dto.TeacherUserId,
    Name:             dto.FullName,
    AvatarUrl:        dto.AvatarUrl ?? $"https://placehold.co/200/e0e7ff/4f46e5?text={Uri.EscapeDataString(dto.FullName[..2].ToUpper())}",
    IsVerified:       false,
    IsSuperTutor:     false,
    Rating:           dto.RatingAvg,
    ReviewCount:      dto.TotalRatingCount,
    StudentCount:     0,
    Bio:              dto.Bio ?? "",
    Subjects:         dto.Subjects.Split(',').Select(s => s.Trim()).ToArray(),
    HourlyRate:       dto.HourlyRate,
    Availability:     AvailabilityStatus.NextSlot,
    AvailabilityLabel: null,
    Location:         "Online",
    ResponseTime:     "",
    TotalLessons:     0,
    FullBio:          dto.Bio ?? "",
    AvailableSlots:   []
);
```

> **Note:** Filter Availability ("Available Today", "Has Slots") và Time range sẽ bị vô hiệu hóa
> hoặc ẩn đi vì DB không có slot data. Có thể ẩn 2 filter panel này hoặc để hiển thị nhưng
> luôn pass (trả về true).

### FindTutorPage.razor — kết nối API thật

- Xóa hardcode `_tutors` list và `private readonly List<TutorListItem> _tutors = [...]`
- Thêm `@inject IHttpClientFactory HttpClientFactory` + `@using BusinessLogicLayer.DTOs.Tutor`
- `OnAfterRenderAsync(firstRender)`: sau khi verify session, gọi `await FetchTutorsAsync()`
- `FetchTutorsAsync()`: gọi `GET /api/tutors` → nhận `ApiResponse<IReadOnlyList<TutorSummaryDto>>` → `_tutors = data.Select(MapToListItem).ToList()` → `ApplyFilter()`
- Khi filter thay đổi (subject chip / price): debounce hoặc re-fetch với query params
- `_loading = true` khi fetch, `_loading = false` sau khi xong

### TutorQuickView.razor — thêm nút Book

- Thêm `[Parameter] public EventCallback<long> OnBookClicked { get; set; }`
- Thêm nút **"Book a Session"** bên cạnh nút "Message"
- `@onclick="() => OnBookClicked.InvokeAsync(Tutor!.Id)"`
- Trong `FindTutorPage.razor`: `<TutorQuickView Tutor="_selected" OnBookClicked="OpenBookingModal" />`

### BookingModal.razor (tạo mới)

**File:** `LearnmateSolution/Components/Pages/Student/FindTutor/Components/BookingModal.razor`

```
Parameters:
  long TeacherId
  string TeacherName
  EventCallback OnClose
  EventCallback<string> OnBooked   // message hiển thị sau khi book thành công

UI:
- Header: "Book a Session with {TeacherName}"
- Date picker (input type="date")
- Time pickers: giờ bắt đầu / kết thúc (input type="time")
- Textarea: ghi chú (optional, 1000 chars max)
- Submit → POST /api/tutors/{TeacherId}/book body={ TeacherId, RequestedStartTime, RequestedEndTime, Note }
- Cancel → gọi OnClose

On success: gọi OnBooked("Your booking request has been sent!") → đóng modal
On error:   hiện error message bên dưới form
```

Combine date + time: `DateTime.Parse($"{_date}T{_startTime}:00")` rồi convert sang UTC trước khi gửi.

### MyBookingsPage.razor (tạo mới)

**File:** `LearnmateSolution/Components/Pages/Student/Bookings/MyBookingsPage.razor`
**Route:** `/student/bookings`
**Layout:** `@layout ClassLayout`

```
Load: GET /api/bookings/my khi trang mở

UI:
- Breadcrumb: Dashboard > My Bookings
- Danh sách booking cards
  - Avatar + tên teacher (từ TeacherProfile.FullName)
  - Ngày giờ: RequestedStartTime – RequestedEndTime (toLocalTime)
  - Ghi chú (nếu có)
  - Status badge:
    PENDING  → amber  "Waiting for response"
    ACCEPTED → green  "Accepted"
    DECLINED → red    "Declined"
    CANCELLED→ slate  "Cancelled"
  - Nút "Cancel" (chỉ hiện khi PENDING) → DELETE /api/bookings/{id}
- Empty state khi không có booking
```

---

## Files cần tạo / sửa

| Action | File |
|--------|------|
| Tạo | `BusinessObject/Enum/BookingRequestStatus.cs` |
| Tạo | `BusinessObject/Models/ClassManagement/TutorBookingRequest.cs` |
| Sửa | `DataAccessLayer/Data/AppDbContext.cs` — thêm DbSet |
| Tạo | `DataAccessLayer/Data/Configurations/TutorBookingConfiguration.cs` |
| Sửa | `DataAccessLayer/Repositories/Interfaces/Identity/ITeacherProfileRepository.cs` — thêm `GetAllTeachersAsync` |
| Sửa | `DataAccessLayer/Repositories/Identity/TeacherProfileRepository.cs` — implement |
| Tạo | `DataAccessLayer/Repositories/Interfaces/ClassManagement/ITutorBookingRepository.cs` |
| Tạo | `DataAccessLayer/Repositories/ClassManagement/TutorBookingRepository.cs` |
| Tạo | `BusinessLogicLayer/DTOs/Tutor/TutorDto.cs` |
| Tạo | `BusinessLogicLayer/Services/Interfaces/ClassManagement/ITutorService.cs` |
| Tạo | `BusinessLogicLayer/Services/ClassManagement/TutorService.cs` |
| Sửa | `BusinessLogicLayer/ServiceCollectionExtensions.cs` — đăng ký DI |
| Tạo | `LearnmateSolution/Controllers/TutorController.cs` |
| Sửa | `LearnmateSolution/Components/Pages/Student/FindTutor/FindTutorPage.razor` — kết nối API + mapper |
| Sửa | `LearnmateSolution/Components/Pages/Student/FindTutor/Components/TutorQuickView.razor` — thêm Book callback |
| Tạo | `LearnmateSolution/Components/Pages/Student/FindTutor/Components/BookingModal.razor` |
| Tạo | `LearnmateSolution/Components/Pages/Student/Bookings/MyBookingsPage.razor` |
| Tạo | Migration `AddTutorBookingRequests` |

---

## Lưu ý kỹ thuật

### Tutor = Teacher
Không có role "TUTOR" riêng. User với `Role = TEACHER` và `TeacherProfile` chính là gia sư.
Thuật ngữ "tutor" trong UI, "teacher" trong DB/code backend — đây là cùng một thực thể.

### TeacherProfile.Subjects là CSV
`"Math, Physics, Chemistry"` → filter bằng `EF.Functions.ILike(p.Subjects, "%Math%")`.
Khi map sang `TutorListItem.Subjects` (string[]) thì `.Split(',').Select(s => s.Trim()).ToArray()`.

### UI fields không có trong DB
`TutorListItem` có nhiều fields UI-only (`IsVerified`, `Location`, `AvailableSlots`...) hiện đang
được hardcode. Khi kết nối real data, các fields này sẽ dùng giá trị mặc định (xem mapper ở trên).
Filters dựa trên chúng (Availability, Time range) sẽ không hoạt động — ẩn hoặc bypass.

### Booking flow
Student tạo → Status = PENDING → Teacher accept (trang Teacher, chưa implement) → Status = ACCEPTED
→ hệ thống tạo Class 1-1, gán `ResultClassId`. `ResultClassId` để null cho đến khi teacher accept.

### Normalize time về UTC
Blazor Server runs trên server → `DateTime` từ input có thể là local. Gọi `.ToUniversalTime()`
hoặc dùng `DateTimeKind.Utc` trước khi lưu DB.
