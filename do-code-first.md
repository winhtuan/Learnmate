# Các lệnh Entity Framework Core (Code First) Cơ Bản

**Lưu ý:** Các lệnh này nên được chạy tại thư mục gốc của Solution (`d:\CODE\dotNet\LearnMate-Project\learnmate-api`).

## 1. Tạo Migration mới

Mỗi khi bạn thay đổi các lớp Models (Entity) hoặc DbContext, bạn cần tạo một bản Migration để ghi nhận lại sự thay đổi đó.

```bash
dotnet ef migrations add learnmate-api-v1.0 --project DataAccess --startup-project learnmate-api

dotnet ef database update --project DataAccess --startup-project learnmate-api
```

## 2. Cập nhật Database (Apply Migration)

Lệnh này sẽ lấy các file Migration chưa được chạy và thực thi chúng dưới dạng SQL vào Database thực tế (PostgreSQL).

```bash
dotnet ef database update learnmate-api-v1.0 --project DataAccess --startup-project learnmate-api
```

## 3. Xóa Migration gần nhất (Undo)

```bash
dotnet ef migrations remove --project DataAccess --startup-project learnmate-api
```

## Giải thích tham số:

- `--project DataAccess`: Chỉ định dự án chứa file cấu hình Migration (chứa DbContext và thư mục Migrations).
- `--startup-project learnmate-api`: Chỉ định dự án chạy lúc bắt đầu (nhằm khởi tạo cấu hình từ `appsettings.json`, giúp EF Core lấy được Connection String).
