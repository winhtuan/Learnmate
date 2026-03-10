# Plan: Class Module — Student Feature Completion

> Ngày tạo: 2026-03-10
> Branch: `feat/student-feature`

---

## Tổng quan trạng thái hiện tại

### Đã hoàn thành (UI + API kết nối thực)

| Trang | Route | API endpoint |
|-------|-------|--------------|
| ClassPage | `/classes` | `GET /api/classes` |
| ClassDetailPage | `/classes/{Id}` | `GET /api/classes/{Id}` |
| ClassAsgPage | `/classes/{Id}/assignments` | `GET /api/classes/{Id}/assignments` |
| ClassMaterialPage | `/classes/{Id}/materials` | `GET /api/classes/{Id}/materials` |
| ClassSchedulePage | `/classes/{Id}/schedule` | `GET /api/classes/{Id}/schedules` |

### Chưa làm / Chưa hoạt động

| # | Tính năng | Lý do chưa xong | Ưu tiên |
|---|-----------|-----------------|---------|
| 1 | Leave Class | Modal có, nhưng `OnConfirm` chưa gọi API | Cao |
| 2 | Nộp bài tập (file upload) | Dropzone UI có, chưa có handler | Cao |
| 3 | "Mark as Done" | Button có, chưa gọi API | Cao |
| 4 | Schedule: điều hướng tháng | Prev/Next button không có EventCallback | Trung bình |
| 5 | Schedule: Week view | Toggle có nhưng chỉ có Month view | Thấp |
| 6 | Schedule: Sync to Calendar | Button có, chưa làm gì | Thấp |
| 7 | Material: Tải file xuống | Row có nút 3 chấm, chưa có action | Trung bình |
| 8 | Material: Upload file | Nút Upload không có handler | Trung bình |
| 9 | ClassVideoPage | Empty state hoàn toàn, chưa implement | Thấp |

---

## Phân tích gap theo layer

### Backend còn thiếu (chưa có endpoint)

| Endpoint | Dùng cho |
|----------|----------|
| `DELETE /api/classes/{id}/members/me` | Leave Class |
| `POST /api/classes/{id}/assignments/{asgId}/submit` | Nộp bài (file + text) |
| `PATCH /api/classes/{id}/assignments/{asgId}/submit/done` | Mark as Done |
| `POST /api/classes/{id}/materials` | Upload tài liệu |
| `GET /api/classes/{id}/videos` | Danh sách video sessions |

### Frontend còn thiếu (UI button không có handler)

- `ClassPage` → `LeaveClassModal.OnConfirm`
- `AssignmentDetailPanel` → upload dropzone handler + "Mark as Done" handler
- `MaterialsToolbar` → Upload button handler
- `MaterialFileRow` → Dropdown actions (Download, Delete)
- `ScheduleHeader` → Prev/Next month callbacks

---

## Kế hoạch triển khai chi tiết

---

### PHASE 1 — Core Actions (Ưu tiên Cao)

#### 1.1 Leave Class

**Mục tiêu:** Student nhấn "Leave Class" → xác nhận modal → rời khỏi lớp → redirect về `/classes`.

**Backend:**
- Thêm method vào `IClassService`:
  ```csharp
  Task<ApiResponse<bool>> LeaveClassAsync(long classId, long studentId, CancellationToken ct = default);
  ```
- Implement trong `ClassService`: soft delete record trong bảng `class_members` (set `deleted_at = now()`).
- Thêm endpoint vào `ClassController`:
  ```csharp
  [HttpDelete("{id:long}/members/me")]
  public async Task<IActionResult> LeaveClass(long id, CancellationToken ct)
  ```

**Frontend (`ClassPage.razor` + `ClassCard.razor`):**
- Thêm `OnConfirm` EventCallback trong `LeaveClassModal` → gọi `DELETE /api/classes/{id}/members/me`.
- Sau khi thành công: remove class khỏi danh sách local (`_classes.RemoveAll(c => c.Id == id)`) và `StateHasChanged()`.
- Hiển thị toast/notification nếu cần.

**Files cần sửa:**
- `BusinessLogicLayer/Services/Interfaces/IClassService.cs`
- `BusinessLogicLayer/Services/ClassService.cs`
- `LearnmateSolution/Controllers/ClassController.cs`
- `LearnmateSolution/Components/Pages/Class/ClassPage.razor`
- `LearnmateSolution/Components/Pages/Class/Components/ClassList/ClassCard.razor`
- `LearnmateSolution/Components/Pages/Class/Components/ClassList/LeaveClassModal.razor`

---

#### 1.2 Nộp bài tập (Assignment Submission)

**Mục tiêu:** Student chọn file và nhấn "Mark as Done" → file được upload → submission được ghi vào DB → trạng thái assignment cập nhật thành `submitted`.

**Backend:**
- Thêm method vào `IClassService`:
  ```csharp
  Task<ApiResponse<bool>> SubmitAssignmentAsync(long classId, long assignmentId, long studentId, IFormFile? file, CancellationToken ct = default);
  ```
- Implement trong `ClassService`:
  - Tạo record `Submission` với `submitted_at = now()`, `status = submitted`.
  - Lưu file vào storage (local disk hoặc cloud) → lưu URL vào `SubmissionAnswer`.
- Thêm endpoint vào `ClassController`:
  ```csharp
  [HttpPost("{id:long}/assignments/{asgId:long}/submit")]
  public async Task<IActionResult> SubmitAssignment(long id, long asgId, IFormFile? file, CancellationToken ct)
  ```

**Frontend (`AssignmentDetailPanel.razor`):**
- Thêm `InputFile` component để chọn file, lưu vào `IBrowserFile? _selectedFile`.
- Nút "Mark as Done" → gọi `POST` với `multipart/form-data`.
- Sau khi thành công: cập nhật `Assignment.SubmissionStatus = "submitted"` → refresh danh sách.
- Disable nút trong khi đang upload, hiển thị loading spinner.

**Lưu ý DTO:**
- `AssignmentDetailPanel` hiện nhận model `AssignmentDetail` (record). Cần truyền thêm `AssignmentId` và `ClassId` để biết gọi endpoint nào.

**Files cần sửa:**
- `BusinessLogicLayer/Services/Interfaces/IClassService.cs`
- `BusinessLogicLayer/Services/ClassService.cs`
- `BusinessLogicLayer/DTOs/Class/` — thêm `SubmitAssignmentRequest`
- `LearnmateSolution/Controllers/ClassController.cs`
- `LearnmateSolution/Components/Pages/Class/Components/Assignments/AssignmentDetailPanel.razor`
- `LearnmateSolution/Components/Pages/Class/ClassAsgPage.razor` — truyền `ClassId` xuống panel

---

### PHASE 2 — Material Management (Ưu tiên Trung bình)

#### 2.1 Download / More options cho material

**Mục tiêu:** Nút 3 chấm trong `MaterialFileRow` → dropdown với "Download" và "Delete" (nếu là owner).

**Frontend (`MaterialFileRow.razor`):**
- Thêm `EventCallback<long> OnDownload` parameter.
- "Download" → `window.open(fileUrl, '_blank')` qua JS interop.
- "Delete" → gọi `DELETE /api/classes/{id}/materials/{materialId}` (chỉ teacher/owner).

**Hiện tại:** Student chỉ xem, không cần delete. Chỉ cần wire download là đủ.

**Files cần sửa:**
- `LearnmateSolution/Components/Pages/Class/Components/Materials/MaterialFileRow.razor`
- `LearnmateSolution/Components/Pages/Class/ClassMaterialPage.razor`

---

#### 2.2 Upload tài liệu

**Mục tiêu:** Nút "Upload" trong `MaterialsToolbar` → modal chọn file → upload lên server → refresh danh sách.

**Thiết kế:** Upload modal (inline hoặc component riêng) với:
- `InputFile` chọn file
- Text input cho title
- Nút "Upload"

**Backend:**
- Thêm method vào `IClassService`:
  ```csharp
  Task<ApiResponse<ClassMaterialDto>> UploadMaterialAsync(long classId, long userId, string title, IFormFile file, CancellationToken ct = default);
  ```
- Endpoint:
  ```csharp
  [HttpPost("{id:long}/materials")]
  public async Task<IActionResult> UploadMaterial(long id, [FromForm] string title, IFormFile file, CancellationToken ct)
  ```

**Files cần sửa:**
- `BusinessLogicLayer/Services/Interfaces/IClassService.cs`
- `BusinessLogicLayer/Services/ClassService.cs`
- `LearnmateSolution/Controllers/ClassController.cs`
- `LearnmateSolution/Components/Pages/Class/Components/Materials/MaterialsToolbar.razor` — thêm EventCallback OnUpload
- `LearnmateSolution/Components/Pages/Class/ClassMaterialPage.razor` — handle upload, refresh list

---

### PHASE 3 — Schedule Interactivity (Ưu tiên Trung bình)

#### 3.1 Điều hướng tháng (Prev / Next)

**Mục tiêu:** Nút `<` / `>` trong `ScheduleHeader` → chuyển tháng → rebuild calendar.

**Thiết kế:**
- `ScheduleHeader` cần 2 EventCallback: `EventCallback OnPrevMonth`, `EventCallback OnNextMonth`.
- `ClassSchedulePage` giữ state `_displayDate` (hiện chỉ set 1 lần).
- Khi click Prev/Next: `_displayDate = _displayDate.AddMonths(±1)` → `BuildCalendar(...)` → `StateHasChanged()`.
- Không cần gọi API thêm — data lịch đã load hết 1 lần, chỉ filter theo tháng.

**Files cần sửa:**
- `LearnmateSolution/Components/Pages/Class/Components/Schedule/ScheduleHeader.razor`
- `LearnmateSolution/Components/Pages/Class/ClassSchedulePage.razor`

---

#### 3.2 Sync to Calendar (.ics export)

**Mục tiêu:** Nút "Sync to Calendar" → download file `.ics` chứa tất cả schedule events.

**Thiết kế (frontend only):**
- Generate `.ics` string từ `_schedules` data đã có.
- JS interop: tạo blob → trigger download.
- Không cần backend endpoint mới.

**Files cần sửa:**
- `LearnmateSolution/Components/Pages/Class/ClassSchedulePage.razor` — thêm method `DownloadIcs()`
- `LearnmateSolution/Components/Pages/Class/Components/Schedule/ScheduleHeader.razor` — thêm EventCallback OnSync

---

### PHASE 4 — Video Sessions (Ưu tiên Thấp)

#### 4.1 Implement ClassVideoPage

**Mục tiêu:** Hiển thị danh sách video sessions đã record và live sessions.

**Backend:**
- Thêm method vào `IClassService`:
  ```csharp
  Task<ApiResponse<ClassVideosDto>> GetClassVideosAsync(long classId, long studentId, CancellationToken ct = default);
  ```
- `ClassVideosDto` chứa:
  - `LiveSession?` — session đang live hoặc sắp live
  - `List<VideoLectureItem>` — các session đã record
- Query từ bảng `video_sessions`.
- Endpoint: `GET /api/classes/{id}/videos`

**Frontend (`ClassVideoPage.razor`):**
- Load data từ API.
- Nếu có live session: render `LiveSessionCard` ở đầu.
- Nếu có recorded videos: render grid `VideoLectureCard`.
- Empty state hiện tại giữ nguyên nếu cả hai list đều rỗng.

**Files cần sửa:**
- `BusinessLogicLayer/Services/Interfaces/IClassService.cs`
- `BusinessLogicLayer/Services/ClassService.cs`
- `BusinessLogicLayer/DTOs/Class/` — thêm `ClassVideosDto`, `VideoLectureItem`
- `LearnmateSolution/Controllers/ClassController.cs`
- `LearnmateSolution/Components/Pages/Class/ClassVideoPage.razor`

---

## Thứ tự triển khai gợi ý

```
Phase 1 (Core Actions)
  ├─ 1.1 Leave Class          [~2h] Backend + Frontend
  └─ 1.2 Assignment Submit    [~4h] Backend + Frontend + file storage

Phase 2 (Materials)
  ├─ 2.1 Download material    [~1h] Frontend only
  └─ 2.2 Upload material      [~3h] Backend + Frontend

Phase 3 (Schedule)
  ├─ 3.1 Month navigation     [~1h] Frontend only
  └─ 3.2 Sync to Calendar     [~2h] Frontend only (JS ICS gen)

Phase 4 (Video)
  └─ 4.1 Video page           [~4h] Backend + Frontend
```

---

## Lưu ý kỹ thuật

### File Storage — MinIO

**Quyết định:** Dùng **MinIO** cho cả development và production (self-hosted, S3-compatible).

**Setup:**
```bash
# Docker Compose (thêm vào docker-compose.yml hoặc chạy standalone)
docker run -d --name minio \
  -p 9000:9000 -p 9001:9001 \
  -e MINIO_ROOT_USER=admin \
  -e MINIO_ROOT_PASSWORD=password123 \
  -v minio_data:/data \
  minio/minio server /data --console-address ":9001"
```

**Package cần thêm vào `LearnmateSolution`:**
```xml
<PackageReference Include="Minio" Version="6.*" />
```

**Config trong `appsettings.json`:**
```json
"MinIO": {
  "Endpoint": "localhost:9000",
  "AccessKey": "admin",
  "SecretKey": "password123",
  "BucketName": "learnmate",
  "UseSSL": false
}
```

**Cấu trúc bucket:** Tổ chức theo thư mục logic trong 1 bucket duy nhất:
```
learnmate/
  ├── materials/{classId}/{filename}        ← tài liệu giáo viên upload
  ├── submissions/{assignmentId}/{studentId}/{filename}  ← bài nộp của sinh viên
  └── avatars/{userId}/{filename}           ← ảnh đại diện
```

**Interface trừu tượng** (tạo trong `BusinessLogicLayer`):
```csharp
public interface IFileStorageService
{
    Task<string> UploadAsync(string folder, string fileName, Stream content, string contentType, CancellationToken ct = default);
    Task DeleteAsync(string objectPath, CancellationToken ct = default);
    string GetPublicUrl(string objectPath);
}
```
Implement `MinioFileStorageService` trong `LearnmateSolution/Services/`, đăng ký DI trong `Program.cs`.

### Assignment Points
`MapToDetail()` trong `ClassAsgPage.razor` truyền `Points: ""` vì `ClassAssignmentDto` chưa có field `TotalPoints`. Cần bổ sung vào DTO và query nếu muốn hiển thị điểm.

### Material File Size
`MaterialPage` map `Size: ""` vì DB không lưu file size. Cần thêm column `file_size_bytes` vào bảng `materials` và migration.

### Schedule Week View
Toggle "Week" trong `ScheduleHeader` chưa làm gì. Cần thêm state `ViewMode` (Month/Week) và component `ScheduleWeekGrid` riêng. Đây là tính năng phức tạp nhất trong schedule — để sau cùng.

### Search & Filter trong Materials
`MaterialsToolbar` render search input nhưng `ClassMaterialPage` không bind giá trị. Cần wire `SearchQuery` state và filter `_materialFiles` theo tên file.
