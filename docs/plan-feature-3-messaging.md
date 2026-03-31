# Kế hoạch: Chức năng 3 — Nhắn tin với giáo viên / gia sư

## Tổng quan

Hiện tại `ChatDrawer.razor` là **UI hoàn toàn giả lập** — messages chỉ lưu trong memory. Cần implement:
1. DB schema: `conversations` + `messages`
2. REST API để gửi/nhận messages
3. SignalR Hub để push real-time (đã có trong ASP.NET Core, không cần package mới)
4. Rewrite `ChatDrawer.razor` + tạo `MessagesPage.razor`

---

## Thứ tự implement

```
1. Tạo Conversation + Message entities
2. Viết migration AddMessaging
3. Tạo IConversationRepository + ConversationRepository
4. Tạo DTOs (Messaging/)
5. Tạo IMessagingService + MessagingService
6. Đăng ký DI
7. Tạo MessagingController (REST endpoints)
8. Tạo ChatHub (SignalR)
9. Đăng ký SignalR trong Program.cs
10. Rewrite ChatDrawer.razor với real data + SignalR client
11. Tạo MessagesPage.razor (/student/messages)
```

---

## 1. Database

### Entity: Conversation

**File:** `BusinessObject/Models/System/Conversation.cs`

```csharp
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("conversations")]
public class Conversation : SoftDeletableEntity
{
    // Normalized: ParticipantAId = min(userA, userB)
    public long ParticipantAId { get; set; }
    public long ParticipantBId { get; set; }

    public DateTime? LastMessageAt { get; set; }

    [ForeignKey("ParticipantAId")]
    public User ParticipantA { get; set; } = null!;

    [ForeignKey("ParticipantBId")]
    public User ParticipantB { get; set; } = null!;

    public ICollection<Message> Messages { get; set; } = [];
}
```

### Entity: Message

**File:** `BusinessObject/Models/System/Message.cs`

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Models.Base;

namespace BusinessObject.Models;

[Table("messages")]
public class Message : AuditableEntity
{
    public long ConversationId { get; set; }
    public long SenderId { get; set; }

    [Required]
    [MaxLength(4000)]
    public string Content { get; set; } = null!;

    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }

    [ForeignKey("ConversationId")]
    public Conversation Conversation { get; set; } = null!;

    [ForeignKey("SenderId")]
    public User Sender { get; set; } = null!;
}
```

### EF Configuration

**File:** `DataAccessLayer/Data/Configurations/MessagingConfiguration.cs`

```csharp
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Data.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> b)
    {
        // Đảm bảo không có 2 conversation giữa cùng 1 cặp user
        b.HasIndex(x => new { x.ParticipantAId, x.ParticipantBId }).IsUnique();

        b.HasOne(x => x.ParticipantA)
            .WithMany()
            .HasForeignKey(x => x.ParticipantAId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.ParticipantB)
            .WithMany()
            .HasForeignKey(x => x.ParticipantBId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> b)
    {
        b.Property(x => x.Content).HasMaxLength(4000);
        b.HasIndex(x => x.ConversationId);
        b.HasIndex(x => x.SenderId);

        b.HasOne(x => x.Sender)
            .WithMany()
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

### AppDbContext

Thêm vào `DataAccessLayer/Data/AppDbContext.cs`:
```csharp
public DbSet<Conversation> Conversations => Set<Conversation>();
public DbSet<Message> Messages => Set<Message>();
```

### Migration

```bash
dotnet ef migrations add AddMessaging --project DataAccessLayer --startup-project LearnmateSolution
dotnet ef database update --project DataAccessLayer --startup-project LearnmateSolution
```

---

## 2. Repository layer

**File:** `DataAccessLayer/Repositories/Interfaces/System/IConversationRepository.cs`

```csharp
using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IConversationRepository
{
    /// <summary>Tìm hoặc tạo conversation giữa 2 user. Normalize: A = min(id), B = max(id).</summary>
    Task<Conversation> GetOrCreateAsync(long userAId, long userBId, CancellationToken ct = default);

    /// <summary>Danh sách conversations của user, kèm last message preview.</summary>
    Task<IReadOnlyList<Conversation>> GetByUserIdAsync(long userId, CancellationToken ct = default);

    /// <summary>Lấy messages của conversation (paged). Kiểm tra requesting user là participant.</summary>
    Task<Conversation?> GetWithMessagesAsync(
        long conversationId, long requestingUserId,
        int skip = 0, int take = 50,
        CancellationToken ct = default);

    /// <summary>Lưu message mới.</summary>
    Task<Message> AddMessageAsync(Message message, CancellationToken ct = default);

    /// <summary>Đánh dấu tất cả messages chưa đọc trong conversation là đã đọc.</summary>
    Task MarkReadAsync(long conversationId, long readByUserId, CancellationToken ct = default);
}
```

**File:** `DataAccessLayer/Repositories/System/ConversationRepository.cs`

Key implementation notes:
- `GetOrCreateAsync`: normalize với `Math.Min/Max`, dùng `FirstOrDefaultAsync` rồi create nếu null, dùng try/catch unique constraint cho concurrent requests
- `GetByUserIdAsync`: filter `ParticipantAId == userId || ParticipantBId == userId`, include last message
- `MarkReadAsync`: bulk update `IsRead = true, ReadAt = UtcNow` cho messages trong conversation mà `SenderId != readByUserId`

---

## 3. BLL layer

### DTOs

**File:** `BusinessLogicLayer/DTOs/Messaging/MessagingDto.cs`

```csharp
namespace BusinessLogicLayer.DTOs.Messaging;

public record ConversationSummaryDto(
    long Id,
    long OtherUserId,
    string OtherUserName,
    string? OtherUserAvatar,
    string? LastMessagePreview,
    DateTime? LastMessageAtLocal,
    int UnreadCount
);

public record MessageDto(
    long Id,
    long SenderId,
    bool IsMine,        // true nếu SenderId == requesting user
    string Content,
    bool IsRead,
    DateTime SentAtLocal
);

public record SendMessageDto
{
    public string Content { get; set; } = null!;
}
```

### Interface + Service

**File:** `BusinessLogicLayer/Services/Interfaces/System/IMessagingService.cs`

```csharp
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Messaging;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IMessagingService
{
    Task<ApiResponse<ConversationSummaryDto>> GetOrCreateConversationAsync(
        long requestingUserId, long otherUserId, CancellationToken ct = default);

    Task<ApiResponse<IReadOnlyList<ConversationSummaryDto>>> GetConversationsAsync(
        long userId, CancellationToken ct = default);

    Task<ApiResponse<IReadOnlyList<MessageDto>>> GetMessagesAsync(
        long conversationId, long requestingUserId,
        int skip = 0, int take = 50,
        CancellationToken ct = default);

    Task<ApiResponse<MessageDto>> SendMessageAsync(
        long conversationId, long senderId, SendMessageDto dto, CancellationToken ct = default);

    Task<ApiResponse<bool>> MarkReadAsync(
        long conversationId, long userId, CancellationToken ct = default);
}
```

**MessagingService** chỉ xử lý DB — **không biết gì về SignalR**. Controller sẽ push SignalR sau khi gọi service.

### DI Registration

Thêm vào `BusinessLogicLayer/ServiceCollectionExtensions.cs`:
```csharp
services.AddScoped<IConversationRepository, ConversationRepository>();
services.AddScoped<IMessagingService, MessagingService>();
```

---

## 4. API + SignalR

### REST Controller

**File:** `LearnmateSolution/Controllers/MessagingController.cs`

```
POST /api/conversations/{otherUserId}          — Get or create conversation
GET  /api/conversations                         — Danh sách conversations
GET  /api/conversations/{id}/messages           — Lấy messages (query: skip, take)
POST /api/conversations/{id}/messages           — Gửi message → lưu DB + push SignalR
POST /api/conversations/{id}/read               — Mark as read
```

`SendMessage` action:
```csharp
// 1. Gọi messagingService.SendMessageAsync(...)
// 2. Sau khi thành công, push SignalR:
await _hubContext.Clients
    .Group($"conv_{conversationId}")
    .SendAsync("ReceiveMessage", messageDto);
// 3. Return Ok(result)
```

### SignalR Hub

**File:** `LearnmateSolution/Hubs/ChatHub.cs`

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LearnmateSolution.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task JoinConversation(long conversationId) =>
        await Groups.AddToGroupAsync(Context.ConnectionId, $"conv_{conversationId}");

    public async Task LeaveConversation(long conversationId) =>
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conv_{conversationId}");
}
```

### Program.cs

```csharp
// Thêm sau builder.Services.AddControllers():
builder.Services.AddSignalR();

// Thêm sau app.MapControllers():
app.MapHub<ChatHub>("/hubs/chat");
```

---

## 5. UI / Blazor

### ChatDrawer.razor — rewrite với real data

**File:** `LearnmateSolution/Components/Pages/Student/FindTutor/Components/ChatDrawer.razor`

Thay toàn bộ hardcode logic:

```csharp
// Parameters
[Parameter] public long TeacherUserId { get; set; }
[Parameter] public string TeacherName { get; set; } = "";
[Parameter] public EventCallback OnClose { get; set; }

// State
private long _conversationId;
private List<MessageDto> _messages = [];
private string _input = "";
private HubConnection? _hub;

// Lifecycle
// OnParametersSet: gọi POST /api/conversations/{TeacherUserId} → lấy conversationId
//                  gọi GET /api/conversations/{id}/messages → load messages
// Kết nối SignalR: _hub.On("ReceiveMessage", (MessageDto msg) => { _messages.Add(msg); StateHasChanged(); })
// _hub.InvokeAsync("JoinConversation", _conversationId)

// Send: POST /api/conversations/{id}/messages body={Content: _input}
// IAsyncDisposable: await _hub?.DisposeAsync()
```

**Lưu ý SignalR trong Blazor Server:**
```csharp
_hub = new HubConnectionBuilder()
    .WithUrl(Nav.ToAbsoluteUri("/hubs/chat"), opts =>
    {
        opts.AccessTokenProvider = () => Task.FromResult<string?>(Session.AccessToken);
    })
    .WithAutomaticReconnect()
    .Build();
await _hub.StartAsync();
```

### MessagesPage.razor (tạo mới)

**File:** `LearnmateSolution/Components/Pages/Student/Messages/MessagesPage.razor`
**Route:** `/student/messages`
**Layout:** `DashboardLayout`

```
UI layout:
┌─────────────────────────────────────────────────────────────┐
│ Left sidebar (1/3)         │  Right: Chat pane (2/3)        │
│ ─────────────────────────  │ ──────────────────────────────  │
│ Search conversations       │  [selected conversation]        │
│ ─────────────────────────  │  Header: avatar, name          │
│ [ConvCard] Teacher A       │  ─────────────────────────────  │
│   "Bài tập tuần sau..."    │  Messages (scrollable)          │
│   14:30                    │  [My message bubble right]      │
│ ─────────────────────────  │  [Other bubble left]            │
│ [ConvCard] Teacher B       │  ─────────────────────────────  │
│   ...                      │  Input box + Send button        │
└─────────────────────────────────────────────────────────────┘
```

Load: `GET /api/conversations` khi trang mở. Khi click conversation → load messages + join SignalR group.

---

## Files cần tạo / sửa

| Action | File |
|--------|------|
| Tạo | `BusinessObject/Models/System/Conversation.cs` |
| Tạo | `BusinessObject/Models/System/Message.cs` |
| Sửa | `DataAccessLayer/Data/AppDbContext.cs` — thêm 2 DbSets |
| Tạo | `DataAccessLayer/Data/Configurations/MessagingConfiguration.cs` |
| Tạo | `DataAccessLayer/Repositories/Interfaces/System/IConversationRepository.cs` |
| Tạo | `DataAccessLayer/Repositories/System/ConversationRepository.cs` |
| Tạo | `BusinessLogicLayer/DTOs/Messaging/MessagingDto.cs` |
| Tạo | `BusinessLogicLayer/Services/Interfaces/System/IMessagingService.cs` |
| Tạo | `BusinessLogicLayer/Services/System/MessagingService.cs` |
| Sửa | `BusinessLogicLayer/ServiceCollectionExtensions.cs` — đăng ký DI |
| Tạo | `LearnmateSolution/Controllers/MessagingController.cs` |
| Tạo | `LearnmateSolution/Hubs/ChatHub.cs` |
| Sửa | `LearnmateSolution/Program.cs` — AddSignalR + MapHub |
| Sửa | `LearnmateSolution/Components/Pages/Student/FindTutor/Components/ChatDrawer.razor` — rewrite |
| Tạo | `LearnmateSolution/Components/Pages/Student/Messages/MessagesPage.razor` |
| Tạo | Migration `AddMessaging` |

---

## Lưu ý kỹ thuật quan trọng

### Conversation uniqueness
Khi tạo conversation giữa user A và B: normalize trước khi insert.
```csharp
var (aId, bId) = (Math.Min(userAId, userBId), Math.Max(userAId, userBId));
```
Unique index `(participant_a_id, participant_b_id)` enforce ở DB.

### BLL không dùng Microsoft.AspNetCore.SignalR
`MessagingService` chỉ lưu DB và trả DTO. `MessagingController` inject `IHubContext<ChatHub>` và tự push — BLL hoàn toàn không biết về SignalR.

### SignalR trong Blazor Server
Blazor Server đã có SignalR circuit riêng. Hub chat là một connection **khác** — dùng `HubConnectionBuilder` với `AccessTokenProvider` truyền JWT.

### Message pagination
Default `take = 50`, load thêm khi scroll lên đầu (infinite scroll ngược). Client gửi `?skip=50&take=50` cho page tiếp theo.

### Seed data gợi ý
Có thể seed 1 conversation + vài messages mẫu giữa student@learnmate.vn (id=3) và teacher@learnmate.vn (id=2) để test UI ngay lập tức.
