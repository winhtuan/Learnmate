-- ============================================================
-- Seed: Student user_id = 3
-- Classes : PRN222, PRU213
-- Schedule: 2 buổi/môn trong tuần hiện tại
-- Streak  : 8 ngày (thêm cột study_streak_days nếu chưa có)
-- ============================================================
-- Chạy: psql -U <user> -d <db> -f scripts/seed/seed_student3.sql
-- Idempotent: có thể chạy lại nhiều lần mà không bị lỗi
-- ============================================================

BEGIN;

-- ── 0. Thêm cột study_streak_days vào student_profiles nếu chưa có ──────────
ALTER TABLE student_profiles
    ADD COLUMN IF NOT EXISTS study_streak_days INTEGER NOT NULL DEFAULT 0;

-- ── 1. Teacher user ──────────────────────────────────────────────────────────
INSERT INTO users (email, password_hash, role, is_active, created_at, updated_at)
VALUES (
    'teacher.prn@learnmate.vn',
    -- BCrypt hash của "Teacher@123" (placeholder — không dùng để login thật)
    '$2a$11$DummyHashPlaceholderXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXu',
    'TEACHER',
    true,
    NOW(), NOW()
)
ON CONFLICT (email) DO NOTHING;

-- ── 2. Teacher profile ───────────────────────────────────────────────────────
DO $$
DECLARE
    v_teacher_id    BIGINT;
    v_class_prn_id  BIGINT;
    v_class_pru_id  BIGINT;
    v_student_id    BIGINT := 3;

    -- Thứ 2 của tuần hiện tại (00:00 UTC+7 = 17:00 UTC ngày hôm trước)
    v_mon           TIMESTAMPTZ := date_trunc('week', NOW() AT TIME ZONE 'Asia/Ho_Chi_Minh')
                                   AT TIME ZONE 'Asia/Ho_Chi_Minh';

    v_asgn1_id      BIGINT;
    v_asgn2_id      BIGINT;
BEGIN

    -- ── Teacher profile ──────────────────────────────────────────────────────
    SELECT id INTO v_teacher_id FROM users WHERE email = 'teacher.prn@learnmate.vn';

    INSERT INTO teacher_profiles (user_id, full_name, subjects, hourly_rate, bio, created_at, updated_at)
    VALUES (
        v_teacher_id,
        'Trần Văn Giảng',
        'Cross-Platform Development, Game Development',
        200000,
        'Giảng viên có 8+ năm kinh nghiệm giảng dạy .NET và Unity tại FPT.',
        NOW(), NOW()
    )
    ON CONFLICT (user_id) DO NOTHING;

    -- ── Classes ──────────────────────────────────────────────────────────────
    -- PRN222
    IF NOT EXISTS (SELECT 1 FROM classes WHERE name = 'PRN222' AND teacher_id = v_teacher_id) THEN
        INSERT INTO classes (teacher_id, name, description, subject, status, max_students, created_at, updated_at)
        VALUES (
            v_teacher_id,
            'PRN222',
            'Cross-Platform Mobile App Development With .NET MAUI',
            'PRN222',
            'ACTIVE',
            30,
            NOW() - INTERVAL '60 days', NOW()
        )
        RETURNING id INTO v_class_prn_id;
    ELSE
        SELECT id INTO v_class_prn_id FROM classes WHERE name = 'PRN222' AND teacher_id = v_teacher_id LIMIT 1;
    END IF;

    -- PRU213
    IF NOT EXISTS (SELECT 1 FROM classes WHERE name = 'PRU213' AND teacher_id = v_teacher_id) THEN
        INSERT INTO classes (teacher_id, name, description, subject, status, max_students, created_at, updated_at)
        VALUES (
            v_teacher_id,
            'PRU213',
            'Unity Game Development — Scripting & Physics',
            'PRU213',
            'ACTIVE',
            30,
            NOW() - INTERVAL '60 days', NOW()
        )
        RETURNING id INTO v_class_pru_id;
    ELSE
        SELECT id INTO v_class_pru_id FROM classes WHERE name = 'PRU213' AND teacher_id = v_teacher_id LIMIT 1;
    END IF;

    -- ── Class members ─────────────────────────────────────────────────────────
    INSERT INTO class_members (class_id, student_id, status, joined_at)
    VALUES (v_class_prn_id, v_student_id, 'ACTIVE', NOW() - INTERVAL '55 days')
    ON CONFLICT (class_id, student_id) DO UPDATE SET status = 'ACTIVE';

    INSERT INTO class_members (class_id, student_id, status, joined_at)
    VALUES (v_class_pru_id, v_student_id, 'ACTIVE', NOW() - INTERVAL '55 days')
    ON CONFLICT (class_id, student_id) DO UPDATE SET status = 'ACTIVE';

    -- ── Schedules tuần này ────────────────────────────────────────────────────
    -- PRN222: Thứ 2 (07:30–09:30) và Thứ 4 (07:30–09:30)
    INSERT INTO schedules (class_id, title, start_time, end_time, type, status, is_trial, created_at, updated_at)
    VALUES
        (v_class_prn_id,
         'PRN222 — Buổi 1: Giới thiệu .NET MAUI & XAML',
         v_mon + INTERVAL '7 hours 30 minutes',
         v_mon + INTERVAL '9 hours 30 minutes',
         'REGULAR', 'SCHEDULED', false, NOW(), NOW()),

        (v_class_prn_id,
         'PRN222 — Buổi 2: Data Binding & MVVM',
         v_mon + INTERVAL '2 days 7 hours 30 minutes',
         v_mon + INTERVAL '2 days 9 hours 30 minutes',
         'REGULAR', 'SCHEDULED', false, NOW(), NOW())
    ON CONFLICT DO NOTHING;

    -- PRU213: Thứ 3 (13:00–15:00) và Thứ 5 (13:00–15:00)
    INSERT INTO schedules (class_id, title, start_time, end_time, type, status, is_trial, created_at, updated_at)
    VALUES
        (v_class_pru_id,
         'PRU213 — Buổi 1: Unity Editor & Scene Setup',
         v_mon + INTERVAL '1 day 13 hours',
         v_mon + INTERVAL '1 day 15 hours',
         'REGULAR', 'SCHEDULED', false, NOW(), NOW()),

        (v_class_pru_id,
         'PRU213 — Buổi 2: Rigidbody Physics & Colliders',
         v_mon + INTERVAL '3 days 13 hours',
         v_mon + INTERVAL '3 days 15 hours',
         'REGULAR', 'SCHEDULED', false, NOW(), NOW())
    ON CONFLICT DO NOTHING;

    -- ── Assignments ───────────────────────────────────────────────────────────
    -- PRN222
    IF NOT EXISTS (SELECT 1 FROM assignments WHERE class_id = v_class_prn_id AND title = 'Lab 1: MAUI Navigation') THEN
        INSERT INTO assignments (class_id, teacher_id, title, description, status, due_date, created_at, updated_at)
        VALUES (
            v_class_prn_id, v_teacher_id,
            'Lab 1: MAUI Navigation',
            'Xây dựng ứng dụng MAUI có ít nhất 3 màn hình với Shell Navigation.',
            'PUBLISHED',
            NOW() + INTERVAL '7 days',
            NOW() - INTERVAL '3 days', NOW()
        )
        RETURNING id INTO v_asgn1_id;
    ELSE
        SELECT id INTO v_asgn1_id FROM assignments WHERE class_id = v_class_prn_id AND title = 'Lab 1: MAUI Navigation';
    END IF;

    -- PRU213
    IF NOT EXISTS (SELECT 1 FROM assignments WHERE class_id = v_class_pru_id AND title = 'Lab 1: Unity Scene') THEN
        INSERT INTO assignments (class_id, teacher_id, title, description, status, due_date, created_at, updated_at)
        VALUES (
            v_class_pru_id, v_teacher_id,
            'Lab 1: Unity Scene',
            'Tạo một scene Unity có ánh sáng, mặt phẳng, và ít nhất 1 GameObject có Rigidbody.',
            'PUBLISHED',
            NOW() + INTERVAL '5 days',
            NOW() - INTERVAL '2 days', NOW()
        )
        RETURNING id INTO v_asgn2_id;
    ELSE
        SELECT id INTO v_asgn2_id FROM assignments WHERE class_id = v_class_pru_id AND title = 'Lab 1: Unity Scene';
    END IF;

    -- ── Materials ─────────────────────────────────────────────────────────────
    INSERT INTO materials (class_id, uploaded_by, title, description, file_url, file_type, status, created_at, updated_at)
    VALUES
        (v_class_prn_id, v_teacher_id,
         'Slide Buổi 1 — .NET MAUI Introduction',
         'Tổng quan về .NET MAUI, XAML cơ bản và cấu trúc project.',
         'https://storage.learnmate.vn/materials/prn222-slide-1.pdf',
         'PDF', 'ACTIVE', NOW() - INTERVAL '7 days', NOW()),

        (v_class_pru_id, v_teacher_id,
         'Slide Buổi 1 — Unity Editor Overview',
         'Hướng dẫn cài đặt Unity Hub, tạo project và làm quen với Editor.',
         'https://storage.learnmate.vn/materials/pru213-slide-1.pdf',
         'PDF', 'ACTIVE', NOW() - INTERVAL '5 days', NOW())
    ON CONFLICT DO NOTHING;

    -- ── Notifications ─────────────────────────────────────────────────────────
    INSERT INTO notifications (user_id, title, content, is_read, created_at)
    VALUES
        (v_student_id,
         'Bài tập mới — PRN222',
         'Lab 1: MAUI Navigation đã được đăng. Hạn nộp: ' || to_char(NOW() + INTERVAL '7 days', 'DD/MM/YYYY'),
         false, NOW() - INTERVAL '3 days'),

        (v_student_id,
         'Bài tập mới — PRU213',
         'Lab 1: Unity Scene đã được đăng. Hạn nộp: ' || to_char(NOW() + INTERVAL '5 days', 'DD/MM/YYYY'),
         false, NOW() - INTERVAL '2 days'),

        (v_student_id,
         'Lịch học tuần này',
         'PRN222: Thứ 2 & Thứ 4 lúc 07:30. PRU213: Thứ 3 & Thứ 5 lúc 13:00.',
         false, NOW() - INTERVAL '30 minutes'),

        (v_student_id,
         'Chào mừng đến PRN222',
         'Bạn đã tham gia lớp Cross-Platform Mobile App Development thành công!',
         true,  NOW() - INTERVAL '55 days'),

        (v_student_id,
         'Chào mừng đến PRU213',
         'Bạn đã tham gia lớp Unity Game Development thành công!',
         true,  NOW() - INTERVAL '55 days')
    ON CONFLICT DO NOTHING;

    -- ── Study streak ──────────────────────────────────────────────────────────
    UPDATE student_profiles
    SET study_streak_days = 8,
        updated_at        = NOW()
    WHERE user_id = v_student_id;

    RAISE NOTICE 'Seed hoàn tất. Teacher id=%, PRN222 id=%, PRU213 id=%',
        v_teacher_id, v_class_prn_id, v_class_pru_id;
END $$;

COMMIT;
