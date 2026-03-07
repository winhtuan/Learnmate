# Các lệnh Entity Framework Core (Code First) Cơ Bản

**Lưu ý:** Các lệnh này nên được chạy tại thư mục gốc của Solution (`d:\CODE\dotNet\Learnmate`).

## 1. Tạo Migration mới

Mỗi khi bạn thay đổi các lớp Models (Entity) hoặc DbContext, bạn cần tạo một bản Migration để ghi nhận lại sự thay đổi đó.

```bash
dotnet ef migrations add <TênMigration> --project DataAccessLayer --startup-project LearnmateSolution
```

## 2. Cập nhật Database (Apply Migration)

Lệnh này sẽ lấy các file Migration chưa được chạy và thực thi chúng dưới dạng SQL vào Database thực tế (PostgreSQL).

```bash
dotnet ef database update --project DataAccessLayer --startup-project LearnmateSolution
```

## 3. Xóa Migration gần nhất (Undo)

```bash
dotnet ef migrations remove --project DataAccessLayer --startup-project LearnmateSolution
```

## 4. Xem danh sách migrations

```bash
dotnet ef migrations list --project DataAccessLayer --startup-project LearnmateSolution
```

## Giải thích tham số:

- `--project DataAccessLayer`: Chỉ định dự án chứa file cấu hình Migration (chứa DbContext và thư mục Migrations).
- `--startup-project LearnmateSolution`: Chỉ định dự án chạy lúc bắt đầu (nhằm khởi tạo cấu hình từ `appsettings.json`, giúp EF Core lấy được Connection String).
