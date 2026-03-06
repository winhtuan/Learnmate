# Entities Documentation

Mô tả chi tiết các bảng trong database hệ thống **LearnMate**.

---

## Table: users

**Mục đích**

Lưu thông tin xác thực của người dùng. Không lưu thông tin cá nhân — tất cả được delegate sang `teacher_profiles` hoặc `student_profiles`.

**Các cột**

| Cột             | Kiểu dữ liệu | Mô tả                                      |
| --------------- | ------------ | ------------------------------------------ |
| `id`            | BIGINT       | Khóa chính                                 |
| `email`         | VARCHAR      | Email đăng nhập (unique, NOT NULL)         |
| `password_hash` | VARCHAR      | Mật khẩu đã hash (NOT NULL)                |
| `role`          | ENUM         | `STUDENT` / `TEACHER` / `ADMIN` (NOT NULL) |
| `is_active`     | BOOLEAN      | Trạng thái tài khoản (NOT NULL)            |
| `created_at`    | TIMESTAMPTZ  | Thời điểm tạo tài khoản (NOT NULL)         |
| `updated_at`    | TIMESTAMPTZ  | Thời điểm cập nhật gần nhất (NOT NULL)     |
| `deleted_at`    | TIMESTAMPTZ  | Soft delete — `NULL` nếu chưa bị xoá       |

**Ràng buộc**

```sql
DEFAULT is_active = true
UNIQUE  (email)
```

**Quan hệ**

- `teacher_profiles.user_id → users.id`
- `student_profiles.user_id → users.id`
- `classes.teacher_id → users.id`
- `class_members.student_id → users.id`
- `assignments.teacher_id → users.id`
- `submissions.student_id → users.id`
- `materials.uploaded_by → users.id`
- `payments.student_id → users.id`
- `invoices.teacher_id → users.id`
- `notifications.user_id → users.id`
- `teacher_ratings.student_id → users.id`
- `teacher_ratings.teacher_id → users.id`

---

## Table: teacher_profiles

**Mục đích**

Lưu thông tin cá nhân và nghề nghiệp của giáo viên.

**Các cột**

| Cột                  | Kiểu dữ liệu  | Mô tả                                                  |
| -------------------- | ------------- | ------------------------------------------------------ |
| `id`                 | BIGINT        | Khóa chính                                             |
| `user_id`            | BIGINT        | Khóa ngoại đến `users.id` (unique, NOT NULL)           |
| `full_name`          | VARCHAR       | Họ tên đầy đủ (NOT NULL)                               |
| `avatar_url`         | VARCHAR       | Đường dẫn ảnh đại diện                                 |
| `bio`                | TEXT          | Giới thiệu bản thân                                    |
| `subjects`           | VARCHAR       | Môn học giảng dạy (NOT NULL)                           |
| `hourly_rate`        | NUMERIC(12,2) | Học phí theo giờ (NOT NULL)                            |
| `rating_avg`         | NUMERIC(5,2)  | Điểm đánh giá trung bình (cached từ `teacher_ratings`) |
| `total_rating_count` | INTEGER       | Tổng số lượt đánh giá (cached từ `teacher_ratings`)    |
| `bank_account`       | VARCHAR       | Số tài khoản ngân hàng để nhận thanh toán              |
| `created_at`         | TIMESTAMPTZ   | Thời điểm tạo (NOT NULL)                               |
| `updated_at`         | TIMESTAMPTZ   | Thời điểm cập nhật gần nhất (NOT NULL)                 |

> `rating_avg` và `total_rating_count` là giá trị cache — được cập nhật mỗi khi có rating mới trong `teacher_ratings`.

**Ràng buộc**

```sql
DEFAULT rating_avg         = 0
DEFAULT total_rating_count = 0
CHECK   hourly_rate > 0
CHECK   rating_avg BETWEEN 0 AND 5
UNIQUE  (user_id)
```

**Quan hệ**

- `teacher_profiles.user_id → users.id`

---

## Table: student_profiles

**Mục đích**

Lưu thông tin cá nhân của học sinh.

**Các cột**

| Cột              | Kiểu dữ liệu | Mô tả                                        |
| ---------------- | ------------ | -------------------------------------------- |
| `id`             | BIGINT       | Khóa chính                                   |
| `user_id`        | BIGINT       | Khóa ngoại đến `users.id` (unique, NOT NULL) |
| `full_name`      | VARCHAR      | Họ tên đầy đủ (NOT NULL)                     |
| `avatar_url`     | VARCHAR      | Đường dẫn ảnh đại diện                       |
| `date_of_birth`  | DATE         | Ngày sinh                                    |
| `grade_level`    | VARCHAR      | Lớp học (ví dụ: 10, 11, 12)                  |
| `parent_contact` | VARCHAR      | Số điện thoại phụ huynh                      |
| `created_at`     | TIMESTAMPTZ  | Thời điểm tạo (NOT NULL)                     |
| `updated_at`     | TIMESTAMPTZ  | Thời điểm cập nhật gần nhất (NOT NULL)       |

**Ràng buộc**

```sql
UNIQUE (user_id)
```

**Quan hệ**

- `student_profiles.user_id → users.id`

---

## Table: classes

**Mục đích**

Lưu thông tin lớp học do giáo viên tạo ra. Là trung tâm kết nối học sinh, lịch học, bài tập và tài liệu.

**Các cột**

| Cột            | Kiểu dữ liệu | Mô tả                                          |
| -------------- | ------------ | ---------------------------------------------- |
| `id`           | BIGINT       | Khóa chính                                     |
| `teacher_id`   | BIGINT       | Khóa ngoại đến `users.id` (NOT NULL)           |
| `name`         | VARCHAR      | Tên lớp học (NOT NULL)                         |
| `description`  | TEXT         | Mô tả lớp học                                  |
| `subject`      | VARCHAR      | Môn học (NOT NULL)                             |
| `status`       | ENUM         | `ACTIVE` / `INACTIVE` / `COMPLETED` (NOT NULL) |
| `max_students` | INTEGER      | Số học sinh tối đa (NOT NULL)                  |
| `created_at`   | TIMESTAMPTZ  | Thời điểm tạo (NOT NULL)                       |
| `updated_at`   | TIMESTAMPTZ  | Thời điểm cập nhật gần nhất (NOT NULL)         |
| `deleted_at`   | TIMESTAMPTZ  | Soft delete — `NULL` nếu chưa bị xoá           |

**Ràng buộc**

```sql
DEFAULT status      = 'ACTIVE'
DEFAULT max_students = 30
CHECK   max_students > 0
```

**Quan hệ**

- `classes.teacher_id → users.id`
- `class_members.class_id → classes.id`
- `schedules.class_id → classes.id`
- `assignments.class_id → classes.id`
- `materials.class_id → classes.id`
- `payments.class_id → classes.id`
- `invoices.class_id → classes.id`
- `teacher_ratings.class_id → classes.id`

---

## Table: class_members

**Mục đích**

Bảng trung gian thể hiện quan hệ Many-to-Many giữa học sinh và lớp học. Lưu trạng thái tham gia của từng học sinh.

**Các cột**

| Cột          | Kiểu dữ liệu | Mô tả                                       |
| ------------ | ------------ | ------------------------------------------- |
| `id`         | BIGINT       | Khóa chính                                  |
| `class_id`   | BIGINT       | Khóa ngoại đến `classes.id` (NOT NULL)      |
| `student_id` | BIGINT       | Khóa ngoại đến `users.id` (NOT NULL)        |
| `status`     | ENUM         | `PENDING` / `ACTIVE` / `DROPPED` (NOT NULL) |
| `joined_at`  | TIMESTAMPTZ  | Thời điểm học sinh tham gia lớp (NOT NULL)  |

**Ràng buộc**

```sql
DEFAULT status    = 'PENDING'
DEFAULT joined_at = NOW()
UNIQUE  (class_id, student_id)
```

**Quan hệ**

- `class_members.class_id → classes.id`
- `class_members.student_id → users.id`

---

## Table: schedules

**Mục đích**

Lưu lịch học của từng lớp. Phân biệt buổi học thường và buổi học thử qua cột `is_trial`.

**Các cột**

| Cột          | Kiểu dữ liệu | Mô tả                                                          |
| ------------ | ------------ | -------------------------------------------------------------- |
| `id`         | BIGINT       | Khóa chính                                                     |
| `class_id`   | BIGINT       | Khóa ngoại đến `classes.id` (NOT NULL)                         |
| `title`      | VARCHAR      | Tiêu đề buổi học (NOT NULL)                                    |
| `start_time` | TIMESTAMPTZ  | Thời gian bắt đầu (NOT NULL)                                   |
| `end_time`   | TIMESTAMPTZ  | Thời gian kết thúc (NOT NULL)                                  |
| `type`       | ENUM         | `REGULAR` / `EXTRA` (NOT NULL)                                 |
| `status`     | ENUM         | `SCHEDULED` / `ONGOING` / `COMPLETED` / `CANCELLED` (NOT NULL) |
| `is_trial`   | BOOLEAN      | `true` nếu là buổi học thử (NOT NULL)                          |
| `created_at` | TIMESTAMPTZ  | Thời điểm tạo (NOT NULL)                                       |
| `updated_at` | TIMESTAMPTZ  | Thời điểm cập nhật gần nhất (NOT NULL)                         |

**Ràng buộc**

```sql
DEFAULT is_trial = false
DEFAULT status   = 'SCHEDULED'
CHECK   end_time > start_time
```

**Quan hệ**

- `schedules.class_id → classes.id`
- `video_sessions.schedule_id → schedules.id`

---

## Table: video_sessions

**Mục đích**

Lưu thông tin phòng học online cho mỗi buổi học. Quan hệ One-to-One với `schedules`.

**Các cột**

| Cột           | Kiểu dữ liệu | Mô tả                                            |
| ------------- | ------------ | ------------------------------------------------ |
| `id`          | BIGINT       | Khóa chính                                       |
| `schedule_id` | BIGINT       | Khóa ngoại đến `schedules.id` (unique, NOT NULL) |
| `provider`    | ENUM         | `ZOOM` / `GOOGLE_MEET` (NOT NULL)                |
| `meeting_url` | VARCHAR      | Đường link vào phòng học (NOT NULL)              |
| `meeting_id`  | VARCHAR      | ID phòng học của nhà cung cấp                    |
| `status`      | ENUM         | `WAITING` / `LIVE` / `ENDED` (NOT NULL)          |
| `started_at`  | TIMESTAMPTZ  | Thời điểm buổi học thực sự bắt đầu               |
| `ended_at`    | TIMESTAMPTZ  | Thời điểm buổi học kết thúc                      |
| `created_at`  | TIMESTAMPTZ  | Thời điểm tạo (NOT NULL)                         |

**Ràng buộc**

```sql
DEFAULT status = 'WAITING'
UNIQUE  (schedule_id)
```

**Quan hệ**

- `video_sessions.schedule_id → schedules.id`

---

## Table: materials

**Mục đích**

Lưu tài liệu học tập do giáo viên upload cho từng lớp.

**Các cột**

| Cột           | Kiểu dữ liệu | Mô tả                                      |
| ------------- | ------------ | ------------------------------------------ |
| `id`          | BIGINT       | Khóa chính                                 |
| `class_id`    | BIGINT       | Khóa ngoại đến `classes.id` (NOT NULL)     |
| `uploaded_by` | BIGINT       | Khóa ngoại đến `users.id` (NOT NULL)       |
| `title`       | VARCHAR      | Tên tài liệu (NOT NULL)                    |
| `description` | TEXT         | Mô tả nội dung tài liệu                    |
| `file_url`    | VARCHAR      | Đường dẫn file (NOT NULL)                  |
| `file_type`   | VARCHAR      | Loại file (PDF, DOCX, MP4, ...) (NOT NULL) |
| `status`      | ENUM         | `ACTIVE` / `HIDDEN` (NOT NULL)             |
| `created_at`  | TIMESTAMPTZ  | Thời điểm upload (NOT NULL)                |
| `updated_at`  | TIMESTAMPTZ  | Thời điểm cập nhật gần nhất (NOT NULL)     |

**Ràng buộc**

```sql
DEFAULT status = 'ACTIVE'
```

**Quan hệ**

- `materials.class_id → classes.id`
- `materials.uploaded_by → users.id`

---

## Table: assignments

**Mục đích**

Lưu bài tập do giáo viên tạo ra cho một lớp học.

**Các cột**

| Cột           | Kiểu dữ liệu | Mô tả                                       |
| ------------- | ------------ | ------------------------------------------- |
| `id`          | BIGINT       | Khóa chính                                  |
| `class_id`    | BIGINT       | Khóa ngoại đến `classes.id` (NOT NULL)      |
| `teacher_id`  | BIGINT       | Khóa ngoại đến `users.id` (NOT NULL)        |
| `title`       | VARCHAR      | Tiêu đề bài tập (NOT NULL)                  |
| `description` | TEXT         | Nội dung / đề bài                           |
| `status`      | ENUM         | `DRAFT` / `PUBLISHED` / `CLOSED` (NOT NULL) |
| `due_date`    | TIMESTAMPTZ  | Hạn nộp bài                                 |
| `created_at`  | TIMESTAMPTZ  | Thời điểm tạo (NOT NULL)                    |
| `updated_at`  | TIMESTAMPTZ  | Thời điểm cập nhật gần nhất (NOT NULL)      |
| `deleted_at`  | TIMESTAMPTZ  | Soft delete — `NULL` nếu chưa bị xoá        |

**Ràng buộc**

```sql
DEFAULT status = 'DRAFT'
```

**Quan hệ**

- `assignments.class_id → classes.id`
- `assignments.teacher_id → users.id`
- `assignment_questions.assignment_id → assignments.id`
- `submissions.assignment_id → assignments.id`

---

## Table: assignment_questions

**Mục đích**

Lưu từng câu hỏi trong một bài tập. Hỗ trợ cả câu hỏi trắc nghiệm và tự luận.

**Các cột**

| Cột             | Kiểu dữ liệu | Mô tả                                      |
| --------------- | ------------ | ------------------------------------------ |
| `id`            | BIGINT       | Khóa chính                                 |
| `assignment_id` | BIGINT       | Khóa ngoại đến `assignments.id` (NOT NULL) |
| `content`       | TEXT         | Nội dung câu hỏi (NOT NULL)                |
| `type`          | ENUM         | `MULTIPLE_CHOICE` / `ESSAY` (NOT NULL)     |
| `order`         | INTEGER      | Thứ tự câu hỏi trong bài (NOT NULL)        |
| `points`        | NUMERIC(5,2) | Điểm tối đa của câu hỏi (NOT NULL)         |
| `created_at`    | TIMESTAMPTZ  | Thời điểm tạo (NOT NULL)                   |

**Ràng buộc**

```sql
CHECK points > 0
CHECK "order" > 0
```

**Quan hệ**

- `assignment_questions.assignment_id → assignments.id`
- `assignment_options.question_id → assignment_questions.id`
- `submission_answers.question_id → assignment_questions.id`

---

## Table: assignment_options

**Mục đích**

Lưu các đáp án lựa chọn của câu hỏi trắc nghiệm. Chỉ dùng cho câu hỏi có type `MULTIPLE_CHOICE`.

**Các cột**

| Cột           | Kiểu dữ liệu | Mô tả                                               |
| ------------- | ------------ | --------------------------------------------------- |
| `id`          | BIGINT       | Khóa chính                                          |
| `question_id` | BIGINT       | Khóa ngoại đến `assignment_questions.id` (NOT NULL) |
| `content`     | TEXT         | Nội dung đáp án (NOT NULL)                          |
| `is_correct`  | BOOLEAN      | `true` nếu đây là đáp án đúng (NOT NULL)            |
| `order`       | INTEGER      | Thứ tự hiển thị đáp án (NOT NULL)                   |

**Ràng buộc**

```sql
DEFAULT is_correct = false
CHECK   "order" > 0
```

**Quan hệ**

- `assignment_options.question_id → assignment_questions.id`
- `submission_answer_options.option_id → assignment_options.id`

---

## Table: submissions

**Mục đích**

Lưu bài nộp của học sinh cho một bài tập. Mỗi học sinh chỉ được nộp một lần cho mỗi bài tập.

**Các cột**

| Cột             | Kiểu dữ liệu | Mô tả                                       |
| --------------- | ------------ | ------------------------------------------- |
| `id`            | BIGINT       | Khóa chính                                  |
| `assignment_id` | BIGINT       | Khóa ngoại đến `assignments.id` (NOT NULL)  |
| `student_id`    | BIGINT       | Khóa ngoại đến `users.id` (NOT NULL)        |
| `status`        | ENUM         | `DRAFT` / `SUBMITTED` / `GRADED` (NOT NULL) |
| `score`         | NUMERIC(5,2) | Điểm số sau khi chấm (`NULL` nếu chưa chấm) |
| `submitted_at`  | TIMESTAMPTZ  | Thời điểm nộp bài                           |
| `created_at`    | TIMESTAMPTZ  | Thời điểm tạo (NOT NULL)                    |
| `updated_at`    | TIMESTAMPTZ  | Thời điểm cập nhật gần nhất (NOT NULL)      |
| `deleted_at`    | TIMESTAMPTZ  | Soft delete — `NULL` nếu chưa bị xoá        |

**Ràng buộc**

```sql
DEFAULT status = 'DRAFT'
CHECK   score >= 0
UNIQUE  (assignment_id, student_id)
```

**Quan hệ**

- `submissions.assignment_id → assignments.id`
- `submissions.student_id → users.id`
- `submission_answers.submission_id → submissions.id`
- `feedbacks.submission_id → submissions.id`

---

## Table: submission_answers

**Mục đích**

Lưu câu trả lời của học sinh cho từng câu hỏi trong bài nộp. Với câu tự luận, lưu trực tiếp vào `answer_text`. Với câu trắc nghiệm, đáp án được chọn lưu trong `submission_answer_options`.

**Các cột**

| Cột             | Kiểu dữ liệu | Mô tả                                                           |
| --------------- | ------------ | --------------------------------------------------------------- |
| `id`            | BIGINT       | Khóa chính                                                      |
| `submission_id` | BIGINT       | Khóa ngoại đến `submissions.id` (NOT NULL)                      |
| `question_id`   | BIGINT       | Khóa ngoại đến `assignment_questions.id` (NOT NULL)             |
| `answer_text`   | TEXT         | Nội dung trả lời (dùng cho câu tự luận, `NULL` với trắc nghiệm) |
| `created_at`    | TIMESTAMPTZ  | Thời điểm tạo (NOT NULL)                                        |

**Quan hệ**

- `submission_answers.submission_id → submissions.id`
- `submission_answers.question_id → assignment_questions.id`
- `submission_answer_options.submission_answer_id → submission_answers.id`

---

## Table: submission_answer_options

**Mục đích**

Lưu các đáp án mà học sinh đã chọn trong câu hỏi trắc nghiệm. Chỉ dùng khi `assignment_questions.type = MULTIPLE_CHOICE`.

**Các cột**

| Cột                    | Kiểu dữ liệu | Mô tả                                             |
| ---------------------- | ------------ | ------------------------------------------------- |
| `id`                   | BIGINT       | Khóa chính                                        |
| `submission_answer_id` | BIGINT       | Khóa ngoại đến `submission_answers.id` (NOT NULL) |
| `option_id`            | BIGINT       | Khóa ngoại đến `assignment_options.id` (NOT NULL) |

**Ràng buộc**

```sql
UNIQUE (submission_answer_id, option_id)
```

**Quan hệ**

- `submission_answer_options.submission_answer_id → submission_answers.id`
- `submission_answer_options.option_id → assignment_options.id`

---

## Table: feedbacks

**Mục đích**

Lưu nhận xét và điểm số của giáo viên cho một bài nộp. Quan hệ One-to-One với `submissions`.

**Các cột**

| Cột             | Kiểu dữ liệu | Mô tả                                              |
| --------------- | ------------ | -------------------------------------------------- |
| `id`            | BIGINT       | Khóa chính                                         |
| `submission_id` | BIGINT       | Khóa ngoại đến `submissions.id` (unique, NOT NULL) |
| `comment`       | TEXT         | Nhận xét của giáo viên                             |
| `score`         | NUMERIC(5,2) | Điểm giáo viên chấm (NOT NULL)                     |
| `created_at`    | TIMESTAMPTZ  | Thời điểm tạo (NOT NULL)                           |
| `updated_at`    | TIMESTAMPTZ  | Thời điểm cập nhật gần nhất (NOT NULL)             |

**Ràng buộc**

```sql
CHECK  score >= 0
UNIQUE (submission_id)
```

**Quan hệ**

- `feedbacks.submission_id → submissions.id`

---

## Table: teacher_ratings

**Mục đích**

Lưu đánh giá của học sinh dành cho giáo viên trong một lớp học. Mỗi học sinh chỉ đánh giá một giáo viên một lần trong mỗi lớp.

**Các cột**

| Cột          | Kiểu dữ liệu | Mô tả                                  |
| ------------ | ------------ | -------------------------------------- |
| `id`         | BIGINT       | Khóa chính                             |
| `student_id` | BIGINT       | Khóa ngoại đến `users.id` (NOT NULL)   |
| `teacher_id` | BIGINT       | Khóa ngoại đến `users.id` (NOT NULL)   |
| `class_id`   | BIGINT       | Khóa ngoại đến `classes.id` (NOT NULL) |
| `rating`     | NUMERIC(5,2) | Điểm đánh giá (NOT NULL)               |
| `comment`    | TEXT         | Nhận xét của học sinh                  |
| `created_at` | TIMESTAMPTZ  | Thời điểm đánh giá (NOT NULL)          |

**Ràng buộc**

```sql
CHECK  rating BETWEEN 1.0 AND 5.0
UNIQUE (student_id, teacher_id, class_id)
```

**Quan hệ**

- `teacher_ratings.student_id → users.id`
- `teacher_ratings.teacher_id → users.id`
- `teacher_ratings.class_id → classes.id`

---

## Table: payments

**Mục đích**

Lưu thông tin thanh toán học phí của học sinh cho một lớp học.

**Các cột**

| Cột          | Kiểu dữ liệu  | Mô tả                                                          |
| ------------ | ------------- | -------------------------------------------------------------- |
| `id`         | BIGINT        | Khóa chính                                                     |
| `student_id` | BIGINT        | Khóa ngoại đến `users.id` (NOT NULL)                           |
| `class_id`   | BIGINT        | Khóa ngoại đến `classes.id` (NOT NULL)                         |
| `invoice_id` | BIGINT        | Khóa ngoại đến `invoices.id` (`NULL` nếu chưa gộp vào invoice) |
| `amount`     | NUMERIC(12,2) | Số tiền thanh toán (NOT NULL)                                  |
| `type`       | ENUM          | `TUITION` / `TRIAL` (NOT NULL)                                 |
| `status`     | ENUM          | `PENDING` / `COMPLETED` / `FAILED` / `REFUNDED` (NOT NULL)     |
| `paid_at`    | TIMESTAMPTZ   | Thời điểm thanh toán thành công                                |
| `created_at` | TIMESTAMPTZ   | Thời điểm tạo (NOT NULL)                                       |
| `updated_at` | TIMESTAMPTZ   | Thời điểm cập nhật gần nhất (NOT NULL)                         |
| `deleted_at` | TIMESTAMPTZ   | Soft delete — `NULL` nếu chưa bị xoá                           |

**Ràng buộc**

```sql
DEFAULT status = 'PENDING'
CHECK   amount > 0
```

**Quan hệ**

- `payments.student_id → users.id`
- `payments.class_id → classes.id`
- `payments.invoice_id → invoices.id`

---

## Table: invoices

**Mục đích**

Lưu hóa đơn thanh toán cho giáo viên, tổng hợp từ các payment của học sinh trong một lớp theo từng kỳ.

**Các cột**

| Cột            | Kiểu dữ liệu  | Mô tả                                       |
| -------------- | ------------- | ------------------------------------------- |
| `id`           | BIGINT        | Khóa chính                                  |
| `teacher_id`   | BIGINT        | Khóa ngoại đến `users.id` (NOT NULL)        |
| `class_id`     | BIGINT        | Khóa ngoại đến `classes.id` (NOT NULL)      |
| `total_amount` | NUMERIC(12,2) | Tổng số tiền hóa đơn (NOT NULL)             |
| `status`       | ENUM          | `PENDING` / `PAID` / `CANCELLED` (NOT NULL) |
| `period_start` | DATE          | Ngày bắt đầu kỳ tính hóa đơn (NOT NULL)     |
| `period_end`   | DATE          | Ngày kết thúc kỳ tính hóa đơn (NOT NULL)    |
| `issued_at`    | TIMESTAMPTZ   | Thời điểm phát hành hóa đơn (NOT NULL)      |
| `paid_at`      | TIMESTAMPTZ   | Thời điểm thanh toán cho giáo viên          |
| `created_at`   | TIMESTAMPTZ   | Thời điểm tạo (NOT NULL)                    |
| `updated_at`   | TIMESTAMPTZ   | Thời điểm cập nhật gần nhất (NOT NULL)      |
| `deleted_at`   | TIMESTAMPTZ   | Soft delete — `NULL` nếu chưa bị xoá        |

**Ràng buộc**

```sql
DEFAULT status = 'PENDING'
CHECK   total_amount > 0
CHECK   period_start < period_end
```

**Quan hệ**

- `invoices.teacher_id → users.id`
- `invoices.class_id → classes.id`
- `payments.invoice_id → invoices.id`

---

## Table: notifications

**Mục đích**

Lưu thông báo hệ thống gửi đến người dùng.

**Các cột**

| Cột          | Kiểu dữ liệu | Mô tả                                |
| ------------ | ------------ | ------------------------------------ |
| `id`         | BIGINT       | Khóa chính                           |
| `user_id`    | BIGINT       | Khóa ngoại đến `users.id` (NOT NULL) |
| `title`      | VARCHAR      | Tiêu đề thông báo (NOT NULL)         |
| `content`    | TEXT         | Nội dung thông báo (NOT NULL)        |
| `is_read`    | BOOLEAN      | Trạng thái đã đọc (NOT NULL)         |
| `created_at` | TIMESTAMPTZ  | Thời điểm tạo thông báo (NOT NULL)   |

**Ràng buộc**

```sql
DEFAULT is_read = false
```

**Quan hệ**

- `notifications.user_id → users.id`

---

## Indexes

Các index cần tạo thêm ngoài PK (PostgreSQL tự tạo index cho PK và UNIQUE constraints).

```sql
-- class_members: tìm tất cả lớp của một học sinh
CREATE INDEX idx_class_members_student_id ON class_members (student_id);

-- schedules: tìm lịch theo lớp và lọc theo thời gian
CREATE INDEX idx_schedules_class_id   ON schedules (class_id);
CREATE INDEX idx_schedules_start_time ON schedules (start_time);

-- assignments: tìm bài tập theo lớp
CREATE INDEX idx_assignments_class_id ON assignments (class_id);

-- submissions: tìm bài nộp theo bài tập hoặc theo học sinh
CREATE INDEX idx_submissions_assignment_id ON submissions (assignment_id);
CREATE INDEX idx_submissions_student_id    ON submissions (student_id);

-- submission_answers: lấy tất cả câu trả lời của một bài nộp
CREATE INDEX idx_submission_answers_submission_id ON submission_answers (submission_id);

-- materials: tìm tài liệu theo lớp
CREATE INDEX idx_materials_class_id ON materials (class_id);

-- payments: tìm thanh toán theo học sinh, lớp, hoặc invoice
CREATE INDEX idx_payments_student_id  ON payments (student_id);
CREATE INDEX idx_payments_class_id    ON payments (class_id);
CREATE INDEX idx_payments_invoice_id  ON payments (invoice_id);

-- invoices: tìm hóa đơn theo giáo viên hoặc lớp
CREATE INDEX idx_invoices_teacher_id ON invoices (teacher_id);
CREATE INDEX idx_invoices_class_id   ON invoices (class_id);

-- teacher_ratings: tính rating trung bình theo giáo viên
CREATE INDEX idx_teacher_ratings_teacher_id ON teacher_ratings (teacher_id);

-- notifications: lấy thông báo chưa đọc của một user
CREATE INDEX idx_notifications_user_id_is_read ON notifications (user_id, is_read);
```
