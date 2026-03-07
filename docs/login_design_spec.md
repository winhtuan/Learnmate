# Login Page – Design Specification (React → Blazor)

## 1. Bố cục tổng thể

```
┌────────────────────────────────────────────────────────────────────┐
│  [LEFT PANEL – 50% width, chỉ hiện ở lg+]  │  [RIGHT PANEL – 50%] │
│  bg gradient slate-900→slate-800→slate-900  │  bg-white            │
│                                             │                      │
│  Logo (L) + "LearnMate"                     │  [Mobile logo]       │
│  h1: "Welcome back"                         │  h2: "Sign in"       │
│  p: "Access your account securely..."       │  p: "Enter your …"   │
│                                             │                      │
│  [RecentAccounts section]                   │  [Email Field]       │
│                                             │  [Password Field]    │
│                                             │  [Sign in Button]    │
│                                             │  [Divider]           │
│                                             │  [Google Button]     │
│                                             │  [Sign up link]      │
└────────────────────────────────────────────────────────────────────┘
```

- Container ngoài cùng: `min-h-screen flex` (full viewport, flex row)
- Responsive: left panel **ẩn** trên mobile (`hidden lg:flex`), right panel luôn hiển thị

---

## 2. Color Palette

| Tên màu      | Giá trị CSS (Tailwind → HEX) | Dùng ở đâu                                                                                |
| ------------ | ---------------------------- | ----------------------------------------------------------------------------------------- |
| `slate-900`  | `#0f172a`                    | Left panel bg, logo bg (right), heading h2, button primary, input focus border, text link |
| `slate-800`  | `#1e293b`                    | Left panel gradient mid, button hover                                                     |
| `slate-700`  | `#334155`                    | Label text, outline button text, "Forgot" link                                            |
| `slate-600`  | `#475569`                    | "Don't have account" text, password toggle icon                                           |
| `slate-500`  | `#64748b`                    | Subtitle / hint text                                                                      |
| `slate-400`  | `#94a3b8`                    | Icon bên trong input, placeholder hint                                                    |
| `slate-300`  | `#cbd5e1`                    | Subheading text trên left panel                                                           |
| `slate-200`  | `#e2e8f0`                    | Input border (normal), divider line, avatar ring                                          |
| `slate-100`  | `#f1f5f9`                    | Secondary button bg                                                                       |
| `slate-50`   | `#f8fafc`                    | Input background                                                                          |
| `white`      | `#ffffff`                    | Right panel bg, logo icon bg (left panel), divider label bg                               |
| `rose-400`   | `#fb7185`                    | Input border khi có lỗi                                                                   |
| `rose-500`   | `#f43f5e`                    | Error border focus, danger button                                                         |
| `rose-50/30` | `rgba(255,241,242,0.3)`      | Input bg khi có lỗi                                                                       |
| `rose-600`   | `#e11d48`                    | Error message text                                                                        |
| `red-600`    | `#dc2626`                    | Error message text (global error)                                                         |

> **Tóm tắt**: design dùng **monochrome slate** hoàn toàn + **rose** chỉ cho lỗi. Không có màu accent nào khác.

---

## 3. Typography

| Element                 | Font weight      | Size                    | Color                       |
| ----------------------- | ---------------- | ----------------------- | --------------------------- |
| Logo brand text         | `semibold` (600) | `text-xl` (1.25rem)     | white / slate-900           |
| h1 (left panel)         | `bold` (700)     | `text-5xl` (3rem)       | white                       |
| h2 "Sign in"            | `bold` (700)     | `text-3xl` (1.875rem)   | slate-900                   |
| Paragraph / subtitle    | `normal`         | `text-base` / `text-lg` | slate-500 / slate-300       |
| Label input             | `medium` (500)   | `text-sm` (0.875rem)    | slate-700                   |
| Error message           | `normal`         | `text-sm`               | rose-600                    |
| "Forgot password?" link | `normal`         | `text-sm`               | slate-600                   |
| "Don't have account"    | `normal`         | `text-sm`               | slate-600                   |
| "Sign up" link          | `semibold` (600) | `text-sm`               | slate-900 + hover underline |

**Font mặc định**: system font (Tailwind default – không import Google Fonts đặc biệt).

---

## 4. Components chi tiết

### 4.1 Left Panel (`AuthLeft`)

```
┌─────────────────────────────┐
│  ┌──┐  LearnMate            │  ← Logo: 40×40px, bg-white, rounded-lg, chữ "L" slate-900 bold
│  └──┘                       │
│                             │
│  Welcome back               │  ← text-5xl bold text-white, mb-6
│  Access your account...     │  ← text-slate-300 text-lg leading-relaxed
│                             │
│  [Recent Accounts section]  │  ← phần dưới cùng
└─────────────────────────────┘
```

- Background: `linear-gradient(to bottom-right, #0f172a, #1e293b, #0f172a)` (from-slate-900 via-slate-800 to-slate-900)
- Padding: `4rem` (p-16 = 64px) tất cả 4 phía
- Width: `50%` (lg:w-1/2)
- Flex column + justify-between (logo+text ở trên, recent accounts ở dưới)

### 4.2 Right Panel

- Background: `white`
- Flex, căn giữa cả ngang lẫn dọc: `flex items-center justify-center`
- Padding: `2rem` (p-8)
- Form container: `w-full max-w-md` (448px)

### 4.3 Input Field (`Field`)

```
[Label text]                   [*nếu required]
┌──────────────────────────────────┐
│  [leftIcon?]  placeholder text  [rightIcon?] │
└──────────────────────────────────┘
[error message / helper text]
```

| Property      | Value                                                          |
| ------------- | -------------------------------------------------------------- |
| Margin bottom | `1.5rem` (mb-6)                                                |
| Label color   | `text-slate-700` medium font-medium text-sm mb-2               |
| Required star | `text-rose-500`                                                |
| Input bg      | `bg-slate-50` (#f8fafc)                                        |
| Input border  | `border-2 border-slate-200` → focus: `border-slate-900`        |
| Error border  | `border-rose-400` → focus: `border-rose-500` + `bg-rose-50/30` |
| Border radius | `rounded-lg` (8px)                                             |
| Padding       | `py-3 pl-4 pr-4` (12px top/bottom, 16px sides)                 |
| Focus ring    | `focus:outline-none focus:border-slate-900`                    |
| Disabled      | `opacity-50 cursor-not-allowed`                                |
| Error text    | `mt-1.5 text-sm text-rose-600`                                 |
| Transition    | `transition-all`                                               |

### 4.4 Password Field (`PasswordField`)

- Dùng `Field` với `type="password"` / `type="text"` toggle
- Label row: flex justify-between → bên trái là "Password", bên phải là link "Forgot your password?"
- Right slot: nút Eye/EyeOff icon (Lucide), `w-5 h-5`, màu `slate-400` → hover `slate-600`
- Placeholder: `••••••••`

### 4.5 Primary Button (`Button` – variant primary)

| Property      | Value                                                                                     |
| ------------- | ----------------------------------------------------------------------------------------- |
| Background    | `bg-slate-900` (#0f172a)                                                                  |
| Text color    | `white`                                                                                   |
| Hover bg      | `bg-slate-800`                                                                            |
| Border radius | `rounded-lg` (8px)                                                                        |
| Padding (md)  | `px-4 py-2.5` (16px / 10px)                                                               |
| Font          | `font-medium text-sm`                                                                     |
| Full width    | `w-full`                                                                                  |
| Disabled      | `opacity-50 cursor-not-allowed`                                                           |
| Active        | `scale-[0.98]` (slight press effect)                                                      |
| Loading state | Spinner: `h-4 w-4 animate-spin rounded-full border-2 border-current border-t-transparent` |
| Transition    | `transition duration-200`                                                                 |
| Focus ring    | `focus-visible:ring-2 focus-visible:ring-offset-2 focus-visible:ring-slate-900`           |

### 4.6 Google Button (`SocialButton`)

- Variant: `outline`
- Border: `border border-slate-200`
- Background: `white` → hover `bg-slate-50`
- Text: `slate-700`
- Icon: Google color logo (SVG) 20×20px bên trái text
- Full width, text "Google"
- Font: `font-medium`

### 4.7 Divider

```
─────────────────── Or continue with ───────────────────
```

- Line: `border-t border-slate-200` (1px, slate-200)
- Label: `bg-white text-slate-500 text-sm px-4`
- Margin: `my-8` (32px trên/dưới)

### 4.8 AccountLoginModal

Modal popup khi click recent account:

- Title: "Sign in"
- Avatar: `h-12 w-12 rounded-full object-cover ring-1 ring-slate-200`
- Tên user: `text-slate-900 font-semibold`
- Email (masked): `text-slate-500 text-sm`
- Input password inline (không dùng Field component): `rounded-lg border border-slate-200 bg-slate-50 px-3 py-2 pr-10`
- Focus: `focus:ring-2 focus:ring-slate-900 focus:border-transparent`
- Nút "Continue as [Name]": `bg-slate-900 text-white px-4 py-2 rounded-lg hover:bg-slate-800`
- Nút "Not you?": `text-sm font-medium text-slate-700 hover:underline`
- Link "Use another account": `text-sm text-slate-600 hover:text-slate-900`

---

## 5. Spacing & Layout

| Vùng                     | Giá trị                  |
| ------------------------ | ------------------------ |
| Left panel padding       | `64px` tất cả 4 phía     |
| Logo → heading gap       | `mb-16` (64px)           |
| h1 → paragraph gap       | `mb-6` (24px)            |
| h2 "Sign in" → subtitle  | `mb-2` (8px)             |
| Subtitle → form          | `mb-10` (40px)           |
| Giữa các Field           | `mb-6` (24px) mỗi field  |
| Sign in button → divider | divider có `my-8` (32px) |
| Google button → footer   | `pb-6` (24px)            |

---

## 6. Responsive

| Breakpoint     | Hành vi                                        |
| -------------- | ---------------------------------------------- |
| Mobile         | Chỉ hiển thị right panel; logo mini ở đầu form |
| `lg` (1024px+) | Left panel hiện ra (`hidden lg:flex lg:w-1/2`) |

**Mobile logo** (chỉ hiện khi `< lg`):

- Container: `flex items-center gap-3 mb-12`
- Icon: `w-10 h-10 bg-slate-900 rounded-lg` + chữ "L" trắng bold
- Text: `text-slate-900 text-xl font-semibold`

---

## 7. Interaction States

| Trạng thái          | Hành vi                                                |
| ------------------- | ------------------------------------------------------ |
| Input focus         | Border đổi thành `slate-900` (2px), no outline         |
| Input error         | Border `rose-400`, bg nhạt rose, text lỗi đỏ phía dưới |
| Button loading      | Text ẩn, spinner quay, disabled                        |
| Button disabled     | `opacity-50 cursor-not-allowed`                        |
| Button hover        | bg nhạt hơn 1 bước (900→800)                           |
| Button active/press | Scale down nhẹ `scale-[0.98]`                          |
| Link hover          | `hover:underline` hoặc `hover:text-slate-900`          |
| Eye icon hover      | `slate-400` → `slate-600`                              |

---

## 8. Gợi ý Blazor CSS Variables

```css
:root {
  --color-primary: #0f172a; /* slate-900 */
  --color-primary-hover: #1e293b; /* slate-800 */
  --color-bg: #ffffff;
  --color-input-bg: #f8fafc; /* slate-50 */
  --color-border: #e2e8f0; /* slate-200 */
  --color-border-focus: #0f172a; /* slate-900 */
  --color-text-main: #0f172a; /* slate-900 */
  --color-text-sub: #64748b; /* slate-500 */
  --color-text-label: #334155; /* slate-700 */
  --color-text-hint: #94a3b8; /* slate-400 */
  --color-error: #e11d48; /* rose-600 */
  --color-error-border: #fb7185; /* rose-400 */
  --color-panel-from: #0f172a;
  --color-panel-via: #1e293b;
  --color-panel-to: #0f172a;
  --radius-input: 8px;
  --radius-button: 8px;
  --radius-logo: 8px;
}
```
