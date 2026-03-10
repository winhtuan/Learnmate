# Student Feature Implementation — Session Notes

> Branch: `feat/student-feature`
> Completed: 2026-03-10

---

## Tổng quan

Toàn bộ 4 phase tính năng học sinh đã được implement xong và build thành công (0 errors).

---

## Phase 1 — Leave Class

### Backend

**DAL — `IClassMemberRepository`**
```csharp
Task<bool> LeaveClassAsync(long classId, long studentId, CancellationToken ct = default);
```

**DAL — `ClassMemberRepository`**
- Tìm member với `Status == ACTIVE`, set `Status = DROPPED`, lưu.
- Không dùng soft-delete vì `ClassMember` không có `DeletedAt`.

**BLL — `IClassService`**
```csharp
Task<ApiResponse<bool>> LeaveClassAsync(long classId, long studentId, CancellationToken ct = default);
```

**Controller**
```
DELETE /api/classes/{id}/members/me
```
- Lấy `studentId` từ JWT claims, gọi `LeaveClassAsync`.

### Frontend

**`ClassCard.razor`**
- Inject `IHttpClientFactory`, `UserSessionService`.
- Thêm `[Parameter] EventCallback<long> OnLeaved`.
- `ConfirmLeaveAsync()`: gọi `DELETE api/classes/{Id}/members/me`, sau đó `OnLeaved.InvokeAsync(Class.Id)`.

**`ClassPage.razor`**
- Thêm `OnLeaved="@HandleLeaved"` vào `<ClassCard>`.
- `HandleLeaved(long classId)`: `_classes.RemoveAll(c => c.Id == classId); RebuildFiltered();`

---

## Phase 2 — Submit Assignment (file upload + Mark as Done)

### Backend

**`Submission.cs`** — Thêm field:
```csharp
[MaxLength(1000)]
public string? FileUrl { get; set; }
```

**DAL — `IClassRepository`**
```csharp
Task<Submission?> GetSubmissionAsync(long assignmentId, long studentId, CancellationToken ct = default);
Task<Submission> UpsertSubmissionAsync(Submission submission, CancellationToken ct = default);
```

**BLL — `IClassService`**
```csharp
Task<ApiResponse<bool>> SubmitAssignmentAsync(
    long classId, long assignmentId, long studentId,
    Stream? fileStream, string? fileName, string? contentType,
    CancellationToken ct = default);
```
- BLL nhận `Stream` (không phải `IFormFile`) để tránh phụ thuộc `Microsoft.AspNetCore.*`.
- Controller extract stream từ `IFormFile` trước khi gọi service.

**Flow trong `ClassService.SubmitAssignmentAsync`**
1. Kiểm tra enrollment.
2. Reject nếu đã `SUBMITTED` hoặc `GRADED`.
3. Upload file lên MinIO tại path `submissions/{assignmentId}/{studentId}/{timestamp}_{filename}`.
4. Upsert `Submission` record với `FileUrl` từ MinIO.

**Controller**
```
POST /api/classes/{id}/assignments/{asgId}/submit  (multipart/form-data)
```

**Migration**: `AddSubmissionFileUrl`

### Frontend

**`AssignmentDetailPanel.razor`** — Rewrite hoàn toàn
- State: `IBrowserFile? _selectedFile`, `bool _submitting`, `bool _isSubmitted`, `string _errorMessage`
- `OnParametersSet()`: `_isSubmitted = Assignment.SubmissionStatus is "submitted" or "graded"`
- Dropzone: `<InputFile>` ẩn trong `<label>`, click để chọn file.
- `SubmitAsync()`: build `MultipartFormDataContent`, POST lên API.
- Khi submitted: hiển thị UI emerald "Assignment Submitted".

> **Lưu ý Razor parser**: Markup phức tạp (SVG bên trong `@if` bên trong `<button>`) gây lỗi `CS0201/CS7014` — parser không nhận ra ranh giới `@code {}`. Fix: dùng ternary text đơn giản thay SVG spinner, dùng method reference thay inline lambda.

**`ClassAsgPage.razor`**
- `MapToDetail` thêm `ClassId: Id, SubmissionStatus: a.SubmissionStatus`.
- `HandleSubmitted()`: chuyển assignment từ upcoming/missing sang completed, cập nhật `selectedAssignment`.

**`ClassModels.cs`** — Cập nhật records:
```csharp
// Thêm ClassId và SubmissionStatus
public record AssignmentDetail(
    long Id, long ClassId, string Title, string Type, string DueDate, string Points,
    string SubmissionStatus,
    AssignmentInstructions Instructions, List<AssignmentResource> Resources,
    string UploadText, string UploadSubtext);
```

---

## Phase 3 — Material: Upload & Download

### Backend

**DAL — `IClassRepository`**
```csharp
Task<Material> CreateMaterialAsync(Material material, CancellationToken ct = default);
```

**BLL — `IClassService`**
```csharp
Task<ApiResponse<ClassMaterialDto>> UploadMaterialAsync(
    long classId, long userId, string title,
    Stream fileStream, string fileName, string contentType,
    CancellationToken ct = default);
```
- Upload lên MinIO tại `materials/{classId}/{timestamp}_{filename}`.
- Tạo `Material` record với `FileType` xác định từ extension.

**Controller**
```
POST /api/classes/{id}/materials  (multipart/form-data, fields: title + file)
```

### Frontend

**`MaterialFile` record** — Thêm `string FileUrl`:
```csharp
public record MaterialFile(long Id, string Name, string Type, string Date, string Size, string FileUrl, ...);
```

**`MaterialFileRow.razor`** — Rewrite
- Inject `IJSRuntime JS`.
- Dropdown menu "Download" → `JS.InvokeVoidAsync("open", File.FileUrl, "_blank")`.

**`MaterialsToolbar.razor`**
- Thêm `EventCallback OnUpload`, `EventCallback<string> OnSearchChange`.
- Upload button gọi `OnUpload.InvokeAsync()`.
- Search input dùng `@bind:event="oninput"`.

**`ClassMaterialPage.razor`** — Rewrite
- State upload modal: `_showUpload`, `_uploadTitle`, `_uploadFile`, `_uploading`, `_uploadError`.
- `UploadAsync()`: POST multipart tới API, reload list khi thành công.
- Search: `_searchQuery` + `ApplySearch()` filter trên `_filtered`.

---

## Phase 4 — Schedule: Month Navigation + ICS Export

### Frontend

**`ScheduleHeader.razor`**
- Thêm `EventCallback OnPrevMonth`, `EventCallback OnNextMonth`, `EventCallback OnSync`.
- Wired chevron và Sync button.

**`ClassSchedulePage.razor`** — Rewrite
- `_schedules` load một lần, giữ trong memory (không re-fetch khi đổi tháng).
- `_displayDate`: track tháng hiện tại.
- `NavPrevMonth()` / `NavNextMonth()`: `_displayDate.AddMonths(±1)` → `RefreshCalendar()`.
- `ExportIcs()`: sinh VCALENDAR string (RFC 5545), gọi `JS.InvokeVoidAsync("downloadFile", ...)`.

**`App.razor`** — Thêm JS helper:
```javascript
window.downloadFile = function (filename, content, mimeType) {
    const blob = new Blob([content], { type: mimeType });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url; a.download = filename;
    document.body.appendChild(a); a.click();
    document.body.removeChild(a); URL.revokeObjectURL(url);
};
```

---

## Phase 4b — Videos Page

### Backend

**`ClassVideosDto.cs`** — New file
```csharp
public record ClassVideosDto(VideoSessionItemDto? LiveSession, IReadOnlyList<VideoSessionItemDto> RecordedSessions);
public record VideoSessionItemDto(long Id, long ScheduleId, string Title, string MeetingUrl,
    string Provider, string SessionStatus, DateTime StartTimeLocal, DateTime? EndTimeLocal);
```

**DAL — `IClassRepository`**
```csharp
Task<IReadOnlyList<Schedule>> GetSchedulesWithVideosAsync(long classId, CancellationToken ct = default);
```
- Include `VideoSession`, order by `StartTime DESC`.

**BLL — `IClassService`**
```csharp
Task<ApiResponse<ClassVideosDto>> GetClassVideosAsync(long classId, long studentId, CancellationToken ct = default);
```
- Live: `VideoSession.Status == LIVE`.
- Recorded: `VideoSession.Status == ENDED`.

**Controller**
```
GET /api/classes/{id}/videos
```

### Frontend

**`ClassModels.cs`** — Cập nhật records:
```csharp
// LiveSession thêm MeetingUrl
public record LiveSession(string Status, string Date, string Title, string Description,
    LiveSessionHost Host, int StudentsWaiting, string MeetingUrl = "#");

// VideoLecture: Id thành long, thêm MeetingUrl
public record VideoLecture(long Id, string Title, string Duration, string Date, string Unit,
    string MeetingUrl, string GradientFrom, string GradientTo, string AccentGradient);
```

**`ClassVideoPage.razor`** — Rewrite
- Load `GET api/classes/{Id}/videos`.
- Map `VideoSessionItemDto` → `LiveSession` (MeetingUrl từ DTO).
- Map → `VideoLecture` list với gradient cycling.
- Empty state khi không có video.

**`LiveSessionCard.razor`**
- "Join Room" link: `href="@Session.MeetingUrl" target="_blank"`.

---

## Kiến trúc quan trọng

| Vấn đề | Giải pháp |
|--------|-----------|
| BLL không được dùng `IFormFile` | Nhận `Stream + fileName + contentType`; controller extract từ `IFormFile` |
| Leave Class không có soft-delete | Set `Status = DROPPED` trên `ClassMember` |
| Month nav không re-fetch | Cache `_schedules` trong memory, chỉ re-filter |
| Razor parser lỗi với SVG trong `@if` | Đơn giản hoá markup: dùng ternary text, method reference thay inline lambda |
| File download từ MinIO | `JS.InvokeVoidAsync("open", url, "_blank")` với pre-signed URL |
| ICS export | Sinh string trong C# rồi download qua `window.downloadFile` JS helper |

---

## Build Status

```
Build succeeded.
4 Warning(s) — pre-existing, không liên quan
0 Error(s)
```
