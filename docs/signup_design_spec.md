# Register / Sign-up Page – Design Specification (React → Blazor)

## 1. Bố cục tổng thể

```
┌──────────────────────────────────────────────────────────────────────┐
│  [LEFT PANEL – 50%, chỉ lg+]         │  [RIGHT PANEL – 50%]         │
│  Gradient slate-900→slate-800→slate-900 │  bg-white                 │
│                                       │                              │
│  Logo + "LearnMate"                   │  [Mobile logo]               │
│  h1: "Create your account"            │  h2: "Create an account"     │
│  p: "Join LearnMate to start…"        │  p: Already have an account? │
│                                       │                              │
│  [KHÔNG có RecentAccounts]            │  [Error message]             │
│                                       │  [Stepper – 3 bước]         │
│                                       │                              │
│                                       │  ━ Step 1: Email             │
│                                       │  ━ Step 2: Tên + Ngày sinh   │
│                                       │  ━ Step 3: Mật khẩu          │
│                                       │  ━ Step 4: OTP               │
│                                       │                              │
│                                       │  [Divider]                   │
│                                       │  [Google Button]             │
└──────────────────────────────────────────────────────────────────────┘
```

> Left panel **giống hệt** Login nhưng:
>
> - `heading` = **"Create your account"**
> - `subheading` = **"Join LearnMate to start your personalized learning journey."**
> - **Ẩn** phần RecentAccounts (`showRecent={false}`)

---

## 2. Luồng multi-step (4 bước)

| Step | Nội dung                      | Nút điều hướng                             |
| ---- | ----------------------------- | ------------------------------------------ |
| 1    | Email input                   | **Next** (disabled nếu email không hợp lệ) |
| 2    | Full name + Date of birth     | **Back** + **Next** (disabled nếu thiếu)   |
| 3    | Tạo mật khẩu + strength meter | **Back** + **Create account**              |
| 4    | Nhập mã OTP gửi về email      | **Back** + **Confirm**                     |

**Animation**: Mỗi step chuyển tiếp có slide animation (next → trượt sang trái, back → trượt sang phải) do `StepAnimator` component xử lý.

---

## 3. Component Stepper

```
○─────────╴●─────────╴○
Enter       Provide     Create
your        basic       your
email       info        password
```

### Visual States

| State    | Dot size/color                                       | Label style                                 |
| -------- | ---------------------------------------------------- | ------------------------------------------- |
| `todo`   | `14×14px`, bg white, ring `slate-300`                | `text-slate-400 text-sm`                    |
| `active` | `20×20px`, bg `slate-900`, ring `slate-900`          | `text-slate-800 font-semibold text-base/lg` |
| `done`   | `16×16px`, bg `green-600`, ring `green-600` + icon ✓ | `text-green-600 text-sm font-medium`        |

### Track line

- Background (inactive): `bg-slate-200`, height `2px`, `rounded`
- Progress (active): `bg-green-600`, width tính theo % = `(step-1)/(n-1)*100`
- Transition: `duration-700 ease-in-out`

### Label alignment

- First item: `text-left`
- Last item: `text-right`
- Middle items: `text-center`

### Step labels (mặc định)

1. "Enter your email"
2. "Provide basic info"
3. "Create your password"

---

## 4. Form theo từng Step

### Step 1 – Email

```
[What's your email?]
┌──────────────────────────────────┐
│ Enter your email address        │
└──────────────────────────────────┘
         [ Next ]
```

- Field: label **"What's your email?"**, type `email`, placeholder "Enter your email address"
- Button "Next": `rounded-full` (full circle radius), `size-lg` (`px-5 py-3 text-base`), full width
- **Disabled** (đổi sang `bg-slate-300`) khi email chưa pass regex `^[^\s@]+@[^\s@]+\.[^\s@]+$`

### Step 2 – Basic Info

```
[Your name]
┌──────────────────────────────────┐
│ Enter your full name            │
└──────────────────────────────────┘

[Date of birth]
📅  [Day ▾]   [Month ▾]   [Year ▾]
You must be between 13 and 100 years old.

[ Back ]  [ Next ]
```

- Field tên: label **"Your name"**, placeholder "Enter your full name"
- DateOfBirthField: 3 dropdown (Day / Month / Year) với Calendar icon bên trái
- Nút: `rounded-full px-6`, Back variant `outline`, Next variant `primary`
- Disabled Next nếu `!fullName.trim() || !dateOfBirth`

### Step 3 – Password

```
[Create a password]
┌──────────────────────────────────┐
│ ••••••••                    [👁] │
└──────────────────────────────────┘
Strength label (màu tương ứng)
[■■■□□]  ← 5 thanh strength bar

✓ 8+ characters        ✓ Contains number
○ Uppercase letter     ✓ Special character
✓ Lowercase letter

[ Back ]  [ Create account ]
```

- PasswordField: label **"Create a password"**, `showForgot={false}`
- Strength label: hiển thị text + màu theo score (thấy bên dưới)
- StrengthBar: 5 segments, `h-1.5`, `bg-green-600` nếu filled / `bg-slate-200` nếu empty
- Requirements grid: 2 cột, icon ✓ (`CheckCircle2, green-600`) / ○ (`Circle, slate-300`)
- Disabled "Create account" nếu password chưa pass tất cả checks hoặc đang loading

### Step 4 – OTP

```
[Enter the verification code]
┌──────────────────────────────────┐
│ Enter code sent to your email   │
└──────────────────────────────────┘
A 6-digit code has been sent to user@example.com. Please check your inbox.

[ Back ]  [ Confirm ]
```

- Field: label **"Enter the verification code"**, placeholder "Enter code sent to your email"
- Text hint: `text-sm text-slate-500 mt-2`, email hiển thị in đậm `<b>`
- Disabled Confirm nếu `!otpCode.trim() || loading`

---

## 5. Password Strength Details

| Score | Label text    | Label class (tailwind color) |
| ----- | ------------- | ---------------------------- |
| 0–1   | "Too weak"    | `text-rose-500`              |
| 2     | "Weak"        | `text-orange-500`            |
| 3     | "Fair"        | `text-yellow-500`            |
| 4     | "Strong"      | `text-emerald-500`           |
| 5     | "Very strong" | `text-green-600`             |

**Password checks** (tất cả phải pass mới cho submit):

- `len`: độ dài ≥ 8
- `upper`: có ít nhất 1 chữ hoa
- `lower`: có ít nhất 1 chữ thường
- `digit`: có ít nhất 1 chữ số
- `special`: có ít nhất 1 ký tự đặc biệt

---

## 6. DateOfBirth Field

```
📅  [Day ▾]      [Month ▾]     [Year ▾]
    (1-31)       (January…)    (maxYear…minYear)
You must be between 13 and 100 years old.
```

| Property   | Value                                                        |
| ---------- | ------------------------------------------------------------ |
| Icon       | `Calendar` (Lucide) 20×20px, `text-slate-400`, absolute left |
| Layout     | `pl-10 grid grid-cols-3 gap-3`                               |
| Dropdowns  | Dùng `Select` component – cùng style với input               |
| Year range | `currentYear - 100` → `currentYear - 13`                     |
| Hint text  | `mt-2 text-xs text-slate-500`                                |
| Validation | Auto-clamp ngày khi thay đổi tháng/năm                       |
| Output     | String ISO `"YYYY-MM-DD"`                                    |

---

## 7. Button styles cho Register

| Nút              | Variant   | Size | Extra classes                  |
| ---------------- | --------- | ---- | ------------------------------ |
| "Next" (Step 1)  | `primary` | `lg` | `rounded-full` full-width      |
| "Next" (Step 2+) | `primary` | `md` | `rounded-full px-6`            |
| "Back"           | `outline` | `md` | `rounded-full px-6`            |
| "Create account" | `primary` | `md` | `rounded-full px-6`            |
| "Confirm"        | `primary` | `md` | `rounded-full px-6`            |
| Google (Social)  | `outline` | `md` | `rounded-full h-12` full-width |

> **Khác biệt so với Login**: Register dùng `rounded-full` thay vì `rounded-lg` cho tất cả các button.

---

## 8. Color Palette (bổ sung so với Login)

| Màu         | HEX       | Dùng ở                                                                    |
| ----------- | --------- | ------------------------------------------------------------------------- |
| `green-600` | `#16a34a` | Stepper done dot, progress track, strength bar filled, requirement ✓ icon |
| `slate-300` | `#cbd5e1` | Stepper todo ring, requirement ○ icon                                     |

_(Các màu slate và rose giống Login – xem login_design_spec.md)_

---

## 9. Spacing

| Vùng                       | Giá trị       |
| -------------------------- | ------------- |
| h2 → subtitle              | `mb-2` (8px)  |
| Subtitle → error/stepper   | `mb-8` (32px) |
| Stepper → form content     | `mb-8` (32px) |
| Strength label → bar       | `mt-2` (8px)  |
| Bar → requirements grid    | `mt-3` (12px) |
| Requirements → nav buttons | `mt-6` (24px) |
| Form → divider             | `my-8` (32px) |
| OTP hint text              | `mt-2` (8px)  |

---

## 10. Validation Logic

| Field         | Rule                          | Disable button khi    |
| ------------- | ----------------------------- | --------------------- |
| Email         | regex email chuẩn             | email không hợp lệ    |
| Full name     | `fullName.trim()` không rỗng  | thiếu name            |
| Date of birth | cả 3 dropdown phải có giá trị | thiếu ngày sinh       |
| Password      | tất cả 5 checks phải pass     | password chưa đủ mạnh |
| OTP           | `otpCode.trim()` không rỗng   | OTP rỗng              |
