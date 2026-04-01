# Payment & Enrollment Flow — Implementation Spec

## Mục tiêu

Thay thế flow cũ (student book → enroll ngay) bằng flow mới:

```
Search class → Bấm Enroll → Trang thanh toán → Thanh toán thành công → Vào class
```

**Invariant duy nhất cần giữ:**
> `Invoice.Status == PAID` ⟺ `ClassMember` tồn tại ⟺ Student được vào class

`ClassMember` **chỉ được tạo một chỗ duy nhất**: bên trong `ConfirmPaymentAsync()` sau khi payment SUCCESS.

---

## Checklist thực thi (theo thứ tự)

- [ ] 1. Thêm enum mới vào `BusinessObject`
- [ ] 2. Cập nhật entity `Class`, `Invoice`, `Payment`
- [ ] 3. Tạo migration
- [ ] 4. Thêm DTO mới vào `BusinessLogicLayer`
- [ ] 5. Viết `IPaymentService` + `PaymentService`
- [ ] 6. Thêm `PaymentController`
- [ ] 7. Đăng ký DI trong `Program.cs`
- [ ] 8. Tạo `PaymentPage.razor`
- [ ] 9. Sửa `BookingModal` / `ClassListCard` — đổi nút Enroll
- [ ] 10. Test end-to-end

---

## Layer 1 — BusinessObject

### 1.1 Tạo enum `InvoiceStatus`

**File:** `BusinessObject/Enum/InvoiceStatus.cs`

```csharp
namespace BusinessObject.Enum;

public enum InvoiceStatus
{
    PENDING,
    PAID,
    CANCELLED,
    REFUNDED
}
```

### 1.2 Tạo enum `PaymentStatus`

**File:** `BusinessObject/Enum/PaymentStatus.cs`

```csharp
namespace BusinessObject.Enum;

public enum PaymentStatus
{
    PENDING,
    SUCCESS,
    FAILED
}
```

### 1.3 Tạo enum `PaymentMethod`

**File:** `BusinessObject/Enum/PaymentMethod.cs`

```csharp
namespace BusinessObject.Enum;

public enum PaymentMethod
{
    VNPAY,
    MOMO,
    BANK_TRANSFER
}
```

### 1.4 Cập nhật entity `Class`

**File:** `BusinessObject/Models/ClassManagement/Class.cs`

Thêm field sau vào class (giữ nguyên các field hiện có):

```csharp
// Thêm field giá lớp học
public decimal Price { get; set; } = 0;
```

### 1.5 Cập nhật entity `Invoice`

**File:** `BusinessObject/Models/Finance/Invoice.cs`

Đảm bảo entity có đủ các field sau (thêm nếu thiếu, không xoá field cũ):

```csharp
using BusinessObject.Enum;
using BusinessObject.Models.Base;
using BusinessObject.Models.ClassManagement;
using BusinessObject.Models.Identity;

namespace BusinessObject.Models;

public class Invoice : SoftDeletableEntity
{
    public int Id { get; set; }

    public int StudentId { get; set; }
    public User Student { get; set; } = null!;

    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;

    public decimal Amount { get; set; }

    public InvoiceStatus Status { get; set; } = InvoiceStatus.PENDING;

    // Navigation
    public Payment? Payment { get; set; }
}
```

### 1.6 Cập nhật entity `Payment`

**File:** `BusinessObject/Models/Finance/Payment.cs`

Đảm bảo entity có đủ các field sau:

```csharp
using BusinessObject.Enum;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

public class Payment : AuditableEntity
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;

    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.PENDING;

    /// <summary>Mã giao dịch từ cổng thanh toán (VNPay, MoMo...). Null nếu chưa xử lý.</summary>
    public string? TransactionId { get; set; }

    public string? FailureReason { get; set; }
}
```

---

## Layer 2 — DataAccessLayer

### 2.1 Cập nhật DbContext

**File:** `DataAccessLayer/Data/AppDbContext.cs` (hoặc tên file DbContext hiện tại)

Đảm bảo có `DbSet` cho `Invoice` và `Payment`. Thêm vào `OnModelCreating`:

```csharp
// Invoice
modelBuilder.Entity<Invoice>(e =>
{
    e.ToTable("invoices");
    e.HasKey(x => x.Id);
    e.Property(x => x.Status)
        .HasConversion<string>()
        .HasMaxLength(20);
    e.Property(x => x.Amount)
        .HasPrecision(18, 2);
    e.HasOne(x => x.Student)
        .WithMany()
        .HasForeignKey(x => x.StudentId)
        .OnDelete(DeleteBehavior.Restrict);
    e.HasOne(x => x.Class)
        .WithMany()
        .HasForeignKey(x => x.ClassId)
        .OnDelete(DeleteBehavior.Restrict);
});

// Payment
modelBuilder.Entity<Payment>(e =>
{
    e.ToTable("payments");
    e.HasKey(x => x.Id);
    e.Property(x => x.Status)
        .HasConversion<string>()
        .HasMaxLength(20);
    e.Property(x => x.Method)
        .HasConversion<string>()
        .HasMaxLength(30);
    e.Property(x => x.Amount)
        .HasPrecision(18, 2);
    e.HasOne(x => x.Invoice)
        .WithOne(x => x.Payment)
        .HasForeignKey<Payment>(x => x.InvoiceId)
        .OnDelete(DeleteBehavior.Restrict);
});

// Class — thêm cột price
modelBuilder.Entity<Class>(e =>
{
    e.Property(x => x.Price).HasPrecision(18, 2).HasDefaultValue(0);
});
```

### 2.2 Tạo migration

```bash
cd DataAccessLayer
dotnet ef migrations add AddPaymentEnrollmentFlow --startup-project ../LearnmateSolution
dotnet ef database update --startup-project ../LearnmateSolution
```

---

## Layer 3 — BusinessLogicLayer

### 3.1 Tạo DTO

**File:** `BusinessLogicLayer/DTOs/Payment/PaymentDtos.cs`

```csharp
using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Payment;

/// <summary>Trả về cho client sau khi tạo invoice — dùng để redirect sang PaymentPage</summary>
public record CreateInvoiceResponseDto(
    int InvoiceId,
    int ClassId,
    string ClassName,
    string TeacherName,
    decimal Amount
);

/// <summary>Thông tin đầy đủ của invoice — PaymentPage dùng để hiển thị</summary>
public record InvoiceDetailDto(
    int InvoiceId,
    int ClassId,
    string ClassName,
    string TeacherName,
    string? Schedule,
    decimal Amount,
    InvoiceStatus Status
);

/// <summary>Request body khi student xác nhận thanh toán</summary>
public record ConfirmPaymentRequestDto(
    int InvoiceId,
    PaymentMethod Method,
    /// <summary>Mock: truyền "SUCCESS" hoặc "FAILED" để test. Thực tế: mã từ cổng thanh toán.</summary>
    string TransactionId
);

/// <summary>Kết quả sau khi confirm thanh toán</summary>
public record ConfirmPaymentResponseDto(
    bool Success,
    int? ClassId,       // Redirect đến class này nếu Success
    string Message
);
```

### 3.2 Tạo `IPaymentService`

**File:** `BusinessLogicLayer/Services/ClassManagement/IPaymentService.cs`

```csharp
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Payment;

namespace BusinessLogicLayer.Services.ClassManagement;

public interface IPaymentService
{
    /// <summary>
    /// Student bấm Enroll → tạo Invoice PENDING → trả invoiceId để redirect PaymentPage.
    /// Kiểm tra: student chưa enroll lớp này, lớp còn ACTIVE.
    /// </summary>
    Task<ApiResponse<CreateInvoiceResponseDto>> CreateInvoiceAsync(int studentId, int classId);

    /// <summary>
    /// Lấy thông tin invoice để PaymentPage hiển thị.
    /// Chỉ trả về nếu invoice thuộc về studentId (guard tránh xem invoice người khác).
    /// </summary>
    Task<ApiResponse<InvoiceDetailDto>> GetInvoiceDetailAsync(int invoiceId, int studentId);

    /// <summary>
    /// Xác nhận thanh toán.
    /// Nếu SUCCESS: đổi Invoice→PAID, tạo ClassMember — tất cả trong 1 transaction.
    /// Nếu FAILED: đổi Payment→FAILED, Invoice giữ PENDING (cho phép retry).
    /// </summary>
    Task<ApiResponse<ConfirmPaymentResponseDto>> ConfirmPaymentAsync(
        ConfirmPaymentRequestDto request, int studentId);

    /// <summary>
    /// Student tự huỷ invoice (chỉ khi Status == PENDING).
    /// </summary>
    Task<ApiResponse<bool>> CancelInvoiceAsync(int invoiceId, int studentId);
}
```

### 3.3 Implement `PaymentService`

**File:** `BusinessLogicLayer/Services/ClassManagement/PaymentService.cs`

```csharp
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Payment;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer; // DbContext namespace — điều chỉnh theo project
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services.ClassManagement;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _db; // Thay bằng tên DbContext thực tế

    public PaymentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ApiResponse<CreateInvoiceResponseDto>> CreateInvoiceAsync(
        int studentId, int classId)
    {
        // Guard: lớp tồn tại và ACTIVE
        var cls = await _db.Classes
            .Include(c => c.Teacher).ThenInclude(t => t.User)
            .FirstOrDefaultAsync(c => c.Id == classId && c.DeletedAt == null);

        if (cls == null)
            return ApiResponse<CreateInvoiceResponseDto>.Fail("Lớp học không tồn tại.");

        // Guard: student chưa enroll lớp này
        var alreadyEnrolled = await _db.ClassMembers
            .AnyAsync(m => m.ClassId == classId && m.StudentId == studentId);

        if (alreadyEnrolled)
            return ApiResponse<CreateInvoiceResponseDto>.Fail("Bạn đã tham gia lớp học này.");

        // Guard: không có invoice PENDING cho cùng cặp (student, class)
        var pendingInvoice = await _db.Invoices
            .FirstOrDefaultAsync(i =>
                i.StudentId == studentId &&
                i.ClassId == classId &&
                i.Status == InvoiceStatus.PENDING &&
                i.DeletedAt == null);

        if (pendingInvoice != null)
        {
            // Trả về invoice cũ để redirect tiếp — không tạo trùng
            return ApiResponse<CreateInvoiceResponseDto>.Ok(new CreateInvoiceResponseDto(
                pendingInvoice.Id,
                cls.Id,
                cls.Title,
                cls.Teacher.User.FullName, // điều chỉnh theo model thực tế
                pendingInvoice.Amount
            ));
        }

        // Tạo Invoice mới
        var invoice = new Invoice
        {
            StudentId = studentId,
            ClassId = classId,
            Amount = cls.Price,
            Status = InvoiceStatus.PENDING
        };

        _db.Invoices.Add(invoice);
        await _db.SaveChangesAsync();

        return ApiResponse<CreateInvoiceResponseDto>.Ok(new CreateInvoiceResponseDto(
            invoice.Id,
            cls.Id,
            cls.Title,
            cls.Teacher.User.FullName,
            invoice.Amount
        ));
    }

    public async Task<ApiResponse<InvoiceDetailDto>> GetInvoiceDetailAsync(
        int invoiceId, int studentId)
    {
        var invoice = await _db.Invoices
            .Include(i => i.Class)
                .ThenInclude(c => c.Teacher).ThenInclude(t => t.User)
            .FirstOrDefaultAsync(i =>
                i.Id == invoiceId &&
                i.StudentId == studentId &&
                i.DeletedAt == null);

        if (invoice == null)
            return ApiResponse<InvoiceDetailDto>.Fail("Không tìm thấy hoá đơn.");

        return ApiResponse<InvoiceDetailDto>.Ok(new InvoiceDetailDto(
            invoice.Id,
            invoice.ClassId,
            invoice.Class.Title,
            invoice.Class.Teacher.User.FullName,
            null, // TODO: format lịch học từ Schedule nếu cần
            invoice.Amount,
            invoice.Status
        ));
    }

    public async Task<ApiResponse<ConfirmPaymentResponseDto>> ConfirmPaymentAsync(
        ConfirmPaymentRequestDto request, int studentId)
    {
        // Dùng transaction — tất cả hoặc không có gì
        await using var tx = await _db.Database.BeginTransactionAsync();

        try
        {
            var invoice = await _db.Invoices
                .Include(i => i.Class)
                .FirstOrDefaultAsync(i =>
                    i.Id == request.InvoiceId &&
                    i.StudentId == studentId &&
                    i.DeletedAt == null);

            if (invoice == null)
                return ApiResponse<ConfirmPaymentResponseDto>.Fail("Không tìm thấy hoá đơn.");

            if (invoice.Status != InvoiceStatus.PENDING)
                return ApiResponse<ConfirmPaymentResponseDto>.Fail(
                    $"Hoá đơn ở trạng thái {invoice.Status}, không thể thanh toán.");

            // Mock gateway: TransactionId == "SUCCESS" → thành công
            // Thực tế: gọi VNPay/MoMo SDK verify ở đây
            var isSuccess = request.TransactionId.ToUpperInvariant() != "FAILED";

            var payment = new Payment
            {
                InvoiceId = invoice.Id,
                Amount = invoice.Amount,
                Method = request.Method,
                TransactionId = request.TransactionId,
                Status = isSuccess ? PaymentStatus.SUCCESS : PaymentStatus.FAILED,
                FailureReason = isSuccess ? null : "Giao dịch bị từ chối bởi ngân hàng."
            };

            _db.Payments.Add(payment);

            if (isSuccess)
            {
                // Đổi Invoice → PAID
                invoice.Status = InvoiceStatus.PAID;

                // Tạo ClassMember — ĐÂY LÀ CHỖ DUY NHẤT TẠO ClassMember
                var member = new ClassMember
                {
                    ClassId = invoice.ClassId,
                    StudentId = studentId
                    // Thêm các field khác của ClassMember nếu cần (JoinedAt, v.v.)
                };
                _db.ClassMembers.Add(member);
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            if (isSuccess)
            {
                return ApiResponse<ConfirmPaymentResponseDto>.Ok(new ConfirmPaymentResponseDto(
                    true, invoice.ClassId, "Thanh toán thành công. Chào mừng bạn đến lớp học!"));
            }

            return ApiResponse<ConfirmPaymentResponseDto>.Ok(new ConfirmPaymentResponseDto(
                false, null, "Thanh toán thất bại. Vui lòng thử lại."));
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task<ApiResponse<bool>> CancelInvoiceAsync(int invoiceId, int studentId)
    {
        var invoice = await _db.Invoices
            .FirstOrDefaultAsync(i =>
                i.Id == invoiceId &&
                i.StudentId == studentId &&
                i.DeletedAt == null);

        if (invoice == null)
            return ApiResponse<bool>.Fail("Không tìm thấy hoá đơn.");

        if (invoice.Status != InvoiceStatus.PENDING)
            return ApiResponse<bool>.Fail("Chỉ có thể huỷ hoá đơn đang chờ thanh toán.");

        invoice.Status = InvoiceStatus.CANCELLED;
        await _db.SaveChangesAsync();

        return ApiResponse<bool>.Ok(true);
    }
}
```

---

## Layer 4 — Controller

### 4.1 Tạo `PaymentController`

**File:** `LearnmateSolution/Controllers/PaymentController.cs`

```csharp
using BusinessLogicLayer.DTOs.Payment;
using BusinessLogicLayer.Services.ClassManagement;
using LearnmateSolution.AppState;
using Microsoft.AspNetCore.Mvc;

namespace LearnmateSolution.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly UserSessionService _session;

    public PaymentController(IPaymentService paymentService, UserSessionService session)
    {
        _paymentService = paymentService;
        _session = session;
    }

    /// <summary>Student bấm Enroll → tạo Invoice → redirect PaymentPage</summary>
    [HttpPost("create-invoice")]
    public async Task<IActionResult> CreateInvoice([FromBody] int classId)
    {
        if (!_session.IsAuthenticated)
            return Unauthorized();

        var result = await _paymentService.CreateInvoiceAsync(_session.UserId, classId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>PaymentPage gọi để lấy thông tin hoá đơn hiển thị</summary>
    [HttpGet("invoice/{invoiceId:int}")]
    public async Task<IActionResult> GetInvoiceDetail(int invoiceId)
    {
        if (!_session.IsAuthenticated)
            return Unauthorized();

        var result = await _paymentService.GetInvoiceDetailAsync(invoiceId, _session.UserId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>Student xác nhận thanh toán (mock hoặc callback thật)</summary>
    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequestDto request)
    {
        if (!_session.IsAuthenticated)
            return Unauthorized();

        var result = await _paymentService.ConfirmPaymentAsync(request, _session.UserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Student huỷ hoá đơn (chỉ khi PENDING)</summary>
    [HttpPost("invoice/{invoiceId:int}/cancel")]
    public async Task<IActionResult> CancelInvoice(int invoiceId)
    {
        if (!_session.IsAuthenticated)
            return Unauthorized();

        var result = await _paymentService.CancelInvoiceAsync(invoiceId, _session.UserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
```

> **Lưu ý về `_session.UserId`:** Thêm property `UserId` vào `UserSessionService` nếu chưa có. Hiện tại `UserSessionService` chỉ lưu `Email` và `Role` — cần lưu thêm `UserId` (int) khi login để controller dùng được.

---

## Layer 5 — UI (Blazor)

### 5.1 Tạo `PaymentPage.razor`

**File:** `LearnmateSolution/Components/Pages/Student/Payment/PaymentPage.razor`

```razor
@page "/student/payment/{InvoiceId:int}"
@layout DashboardLayout
@inject IHttpClientFactory HttpFactory
@inject NavigationManager Nav
@inject UserSessionService Session
@using BusinessLogicLayer.DTOs.Payment
@using BusinessObject.Enum

<HeadContent>
    <link rel="stylesheet" href="/css/payment/payment.css" />
</HeadContent>

@if (_loading)
{
    <div class="payment-loading">Đang tải thông tin thanh toán...</div>
}
else if (_invoice == null)
{
    <div class="payment-error">Không tìm thấy hoá đơn.</div>
}
else if (_invoice.Status != InvoiceStatus.PENDING)
{
    <div class="payment-done">
        Hoá đơn này đã được xử lý (trạng thái: @_invoice.Status).
        <a href="/student/class/@_invoice.ClassId">Vào lớp học</a>
    </div>
}
else
{
    <div class="payment-container">
        <h2>Xác nhận đăng ký lớp học</h2>

        <div class="payment-summary">
            <div class="summary-row">
                <span>Lớp học</span>
                <span>@_invoice.ClassName</span>
            </div>
            <div class="summary-row">
                <span>Gia sư</span>
                <span>@_invoice.TeacherName</span>
            </div>
            <div class="summary-row total">
                <span>Học phí</span>
                <span>@_invoice.Amount.ToString("N0") VND</span>
            </div>
        </div>

        <div class="payment-method">
            <h3>Phương thức thanh toán</h3>
            <label class=@(_selectedMethod == PaymentMethod.VNPAY ? "method-card selected" : "method-card")>
                <input type="radio" name="method" value="VNPAY"
                       @onchange="() => _selectedMethod = PaymentMethod.VNPAY" />
                VNPay
            </label>
            <label class=@(_selectedMethod == PaymentMethod.MOMO ? "method-card selected" : "method-card")>
                <input type="radio" name="method" value="MOMO"
                       @onchange="() => _selectedMethod = PaymentMethod.MOMO" />
                MoMo
            </label>
            <label class=@(_selectedMethod == PaymentMethod.BANK_TRANSFER ? "method-card selected" : "method-card")>
                <input type="radio" name="method" value="BANK_TRANSFER"
                       @onchange="() => _selectedMethod = PaymentMethod.BANK_TRANSFER" />
                Chuyển khoản
            </label>
        </div>

        @if (!string.IsNullOrEmpty(_errorMessage))
        {
            <div class="payment-error-msg">@_errorMessage</div>
        }

        <div class="payment-actions">
            <button class="btn-cancel" @onclick="HandleCancel" disabled="@_processing">
                Huỷ
            </button>
            <button class="btn-pay" @onclick="HandlePayment" disabled="@_processing">
                @(_processing ? "Đang xử lý..." : $"Thanh toán {_invoice.Amount:N0} VND")
            </button>
        </div>
    </div>
}

@code {
    [Parameter] public int InvoiceId { get; set; }

    private InvoiceDetailDto? _invoice;
    private PaymentMethod _selectedMethod = PaymentMethod.VNPAY;
    private bool _loading = true;
    private bool _processing = false;
    private string _errorMessage = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        // Route guard
        if (!Session.IsAuthenticated || Session.Role != "STUDENT")
        {
            Nav.NavigateTo("/login");
            return;
        }

        await LoadInvoice();
    }

    private async Task LoadInvoice()
    {
        try
        {
            var http = HttpFactory.CreateClient("learnmate");
            http.BaseAddress = new Uri(Nav.BaseUri);

            var response = await http.GetFromJsonAsync<ApiResponse<InvoiceDetailDto>>(
                $"api/payments/invoice/{InvoiceId}");

            _invoice = response?.Data;
        }
        catch
        {
            _errorMessage = "Không thể tải thông tin hoá đơn.";
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    private async Task HandlePayment()
    {
        _processing = true;
        _errorMessage = string.Empty;
        StateHasChanged();

        try
        {
            var http = HttpFactory.CreateClient("learnmate");
            http.BaseAddress = new Uri(Nav.BaseUri);

            // Mock: TransactionId = "SUCCESS" để test thành công
            // Thực tế: redirect sang cổng thanh toán, nhận callback
            var request = new ConfirmPaymentRequestDto(InvoiceId, _selectedMethod, "SUCCESS");

            var response = await http.PostAsJsonAsync("api/payments/confirm", request);
            var result = await response.Content
                .ReadFromJsonAsync<ApiResponse<ConfirmPaymentResponseDto>>();

            if (result?.Data?.Success == true && result.Data.ClassId.HasValue)
            {
                Nav.NavigateTo($"/student/class/{result.Data.ClassId.Value}");
            }
            else
            {
                _errorMessage = result?.Data?.Message ?? "Thanh toán thất bại. Vui lòng thử lại.";
            }
        }
        catch
        {
            _errorMessage = "Lỗi kết nối. Vui lòng thử lại.";
        }
        finally
        {
            _processing = false;
            StateHasChanged();
        }
    }

    private async Task HandleCancel()
    {
        _processing = true;
        StateHasChanged();

        try
        {
            var http = HttpFactory.CreateClient("learnmate");
            http.BaseAddress = new Uri(Nav.BaseUri);

            await http.PostAsync($"api/payments/invoice/{InvoiceId}/cancel", null);
        }
        finally
        {
            Nav.NavigateTo("/student/find-tutor");
        }
    }
}
```

### 5.2 Tạo CSS cho PaymentPage

**File:** `LearnmateSolution/wwwroot/css/payment/payment.css`

```css
.payment-container {
    max-width: 560px;
    margin: 40px auto;
    padding: 32px;
    background: var(--bs-white, #fff);
    border-radius: 12px;
    border: 1px solid #e5e7eb;
}

.payment-container h2 {
    font-size: 1.25rem;
    font-weight: 600;
    margin-bottom: 24px;
}

.payment-summary {
    border: 1px solid #e5e7eb;
    border-radius: 8px;
    overflow: hidden;
    margin-bottom: 24px;
}

.summary-row {
    display: flex;
    justify-content: space-between;
    padding: 12px 16px;
    font-size: 0.9rem;
    border-bottom: 1px solid #f3f4f6;
}

.summary-row:last-child { border-bottom: none; }

.summary-row.total {
    font-weight: 600;
    background: #f9fafb;
}

.payment-method h3 {
    font-size: 0.95rem;
    font-weight: 600;
    margin-bottom: 12px;
}

.method-card {
    display: flex;
    align-items: center;
    gap: 10px;
    padding: 12px 16px;
    border: 1px solid #e5e7eb;
    border-radius: 8px;
    margin-bottom: 8px;
    cursor: pointer;
    font-size: 0.9rem;
    transition: border-color 0.15s;
}

.method-card.selected {
    border-color: #6366f1;
    background: #f5f3ff;
}

.payment-actions {
    display: flex;
    gap: 12px;
    margin-top: 24px;
}

.btn-cancel {
    flex: 1;
    padding: 12px;
    border: 1px solid #e5e7eb;
    border-radius: 8px;
    background: #fff;
    cursor: pointer;
    font-size: 0.9rem;
    transition: background 0.15s;
}

.btn-cancel:hover { background: #f9fafb; }

.btn-pay {
    flex: 2;
    padding: 12px;
    border: none;
    border-radius: 8px;
    background: #6366f1;
    color: #fff;
    font-weight: 600;
    cursor: pointer;
    font-size: 0.9rem;
    transition: background 0.15s;
}

.btn-pay:hover { background: #4f46e5; }
.btn-pay:disabled, .btn-cancel:disabled { opacity: 0.5; cursor: not-allowed; }

.payment-error-msg {
    color: #dc2626;
    font-size: 0.85rem;
    margin-top: 12px;
    padding: 10px;
    background: #fef2f2;
    border-radius: 6px;
}

.payment-loading, .payment-error, .payment-done {
    text-align: center;
    padding: 40px;
    color: #6b7280;
}
```

### 5.3 Sửa `BookingModal` — đổi nút Enroll

**File:** `LearnmateSolution/Components/Pages/Student/FindTutor/Components/BookingModal.razor`

Tìm đoạn code gọi `POST /api/tutors/{teacherId}/book` và thay bằng:

```csharp
// CŨ — xoá hoặc comment lại:
// var response = await http.PostAsJsonAsync($"api/tutors/{TeacherId}/book", ...);

// MỚI:
private async Task HandleEnroll()
{
    _processing = true;
    StateHasChanged();

    try
    {
        var http = HttpFactory.CreateClient("learnmate");
        http.BaseAddress = new Uri(Nav.BaseUri);

        var response = await http.PostAsJsonAsync("api/payments/create-invoice", ClassId);
        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse<CreateInvoiceResponseDto>>();

        if (result?.Success == true && result.Data != null)
        {
            // Redirect sang trang thanh toán — KHÔNG enroll ngay
            Nav.NavigateTo($"/student/payment/{result.Data.InvoiceId}");
        }
        else
        {
            _errorMessage = result?.Message ?? "Không thể tạo hoá đơn.";
        }
    }
    catch
    {
        _errorMessage = "Lỗi kết nối.";
    }
    finally
    {
        _processing = false;
        StateHasChanged();
    }
}
```

Tương tự với `ClassListCard.razor` nếu có nút Enroll trực tiếp ở đó.

---

## Layer 6 — DI Registration

**File:** `LearnmateSolution/Program.cs`

Thêm vào phần đăng ký services:

```csharp
// Payment
builder.Services.AddScoped<IPaymentService, PaymentService>();
```

---

## Lưu ý quan trọng cho Claude Code

### Về `UserSessionService`
Hiện tại `UserSessionService` lưu `Email` và `Role`. **Cần thêm `UserId` (int):**

```csharp
// AppState/UserSessionService.cs — thêm property:
public int UserId { get; set; }

// Và cập nhật chỗ set session sau khi login thành công:
Session.UserId = user.Id;
```

### Về `ApiResponse<T>`
File `BusinessLogicLayer/DTOs/ApiResponse.cs` hiện có. Đảm bảo có static factory methods:

```csharp
public static ApiResponse<T> Ok(T data) =>
    new() { Success = true, Data = data };

public static ApiResponse<T> Fail(string message) =>
    new() { Success = false, Message = message };
```

Nếu chưa có, thêm vào.

### Về namespace
Tất cả entity trong `BusinessObject/Models/` đều dùng `namespace BusinessObject.Models;` (không có sub-namespace theo CLAUDE.md). Giữ nguyên convention này.

### Về mock thanh toán
`TransactionId = "SUCCESS"` → thanh toán thành công.
`TransactionId = "FAILED"` → thanh toán thất bại (cho phép retry).
Thực tế sau này thay bằng SDK của VNPay/MoMo — chỉ cần sửa trong `PaymentService.ConfirmPaymentAsync()`, không cần đổi interface hay UI.

---

## Thứ tự thực thi được khuyến nghị

```
1. Enum (InvoiceStatus, PaymentStatus, PaymentMethod)
2. Entity update (Class.Price, Invoice, Payment)
3. DbContext OnModelCreating
4. Migration + Update DB
5. DTO (PaymentDtos.cs)
6. IPaymentService + PaymentService
7. PaymentController
8. Program.cs DI
9. PaymentPage.razor + payment.css
10. Sửa BookingModal / ClassListCard
11. Build toàn solution: dotnet build Learnmate.sln
12. Test flow: Search → Enroll → PaymentPage → Confirm → ClassPage
```