# Dashboard Page – Design Specification (React → Blazor)

## 1. Bố cục tổng thể

```
┌────────────────────────────────────────────────────────────────┐
│  [StudentHeader – sticky top, full width]                      │
├────────────────────────────────────────────────────────────────┤
│  <main> max-w-7xl, mx-auto, px-4~8, py-8                       │
│                                                                │
│  [WelcomeBanner – full width, mb-8]                            │
│                                                                │
│  ┌─────────────────────────────┬────────────────────────┐     │
│  │  Left col (lg:col-span-2)   │  Right col (lg:col-span-1)  │
│  │  space-y-8                  │  space-y-8             │     │
│  │                             │                        │     │
│  │  [WeeklySchedule]           │  [QuickActions]        │     │
│  │  [CurrentCourses]           │  [Notifications]       │     │
│  │                             │  [StudyStreak]         │     │
│  └─────────────────────────────┴────────────────────────┘     │
└────────────────────────────────────────────────────────────────┘
```

- Nền tổng thể: `bg-background-light` (light mode custom token – thường là `#f8fafc` hoặc trắng nhạt) + `font-display text-slate-900`
- Grid nội dung: `grid grid-cols-1 lg:grid-cols-3 gap-8`
- Left: `lg:col-span-2` | Right: `lg:col-span-1` (1/3)

---

## 2. StudentHeader (Sticky Nav)

```
┌──────────────────────────────────────────────────────────────────┐
│ [Logo SVG] LearnMate  [🔍 Search…]    Dashboard Tutors … [🔔] [Avatar] │
└──────────────────────────────────────────────────────────────────┘
```

| Property   | Value                               |
| ---------- | ----------------------------------- |
| Position   | `sticky top-0 z-50`                 |
| Background | `bg-white/80 backdrop-blur-md`      |
| Border     | `border-b border-slate-200`         |
| Padding    | `px-6 py-3 lg:px-10`                |
| Layout     | `flex items-center justify-between` |

### Logo

- SVG icon `size-8`, màu `text-primary`
- Text: `text-xl font-bold leading-tight tracking-tight text-slate-900`
- Khoảng cách logo → text: `gap-4`

### Search bar (hidden mobile, `md+`)

- Container: `rounded-full bg-slate-100 border border-transparent` h-10, w-64~80
- Focus: `border-primary/50 transition-colors`
- Search icon: `material-symbols-outlined text-[20px] text-slate-400`, pl-4
- Input: `rounded-r-full bg-transparent text-sm text-slate-900 placeholder:text-slate-400`

### Nav links (hidden mobile, `lg+`)

- Active: `text-primary`
- Inactive: `text-slate-600 hover:text-primary`
- Font: `text-sm font-medium transition-colors`
- Nav items: Dashboard, Tutors, Schedule, Resources

### Right side

- **Bell icon**: `material-symbols-outlined text-slate-500 hover:text-slate-800` + badge đỏ `size-2 bg-red-500 rounded-full border-2 border-white` (absolute top-right)
- **Avatar**: `size-9 rounded-full bg-cover ring-2 ring-slate-100 hover:ring-primary transition-all bg-slate-200` (bg-image nếu có, fallback màu slate)

---

## 3. WelcomeBanner

```
┌─────────────────────────────────────────────────────────────────┐
│  Good morning, [Name]           [📅 Term: Fall 2024-2025]       │
│  Here's your overview for Monday, March 7.                      │
├───────────────┬───────────────┬───────────────┬─────────────────┤
│  🏫 0 Active  │ ✅ 0 Items   │ 📅 0 Today    │  ⏱ 00:00:00   │
│  MY CLASSES   │ PENDING TASKS │   UPCOMING    │  NEXT SESSION   │
└───────────────┴───────────────┴───────────────┴─────────────────┘
```

### Header row

- Container: `bg-white rounded-xl border border-slate-200 shadow-sm mb-8`
- Top section: `p-6 md:p-8 border-b border-slate-100 flex flex-col md:flex-row justify-between`
- h1: `text-3xl font-bold text-slate-900 mb-1`
- Subtitle: `text-slate-500`
- Term badge: `hidden sm:flex items-center gap-2 px-4 py-2 bg-slate-50 rounded-lg border border-slate-100` + icon + `text-sm font-semibold text-slate-700`

### StatCard (3 cột màu)

| Color    | Background hover                 | Text icon         | HEX accent |
| -------- | -------------------------------- | ----------------- | ---------- |
| `blue`   | `bg-blue-50/50 → bg-blue-50`     | `text-blue-600`   | #2563eb    |
| `orange` | `bg-orange-50/50 → bg-orange-50` | `text-orange-600` | #ea580c    |
| `red`    | `bg-red-50/50 → bg-red-50`       | `text-red-600`    | #dc2626    |

- Layout mỗi card: `flex-1 p-6 flex flex-col items-center text-center transition-colors`
- Icon: `material-symbols-outlined text-3xl mb-3`
- Label: `text-xs font-bold uppercase tracking-widest mb-1 opacity-70`
- Value: `text-2xl font-bold text-slate-900`
- Divider giữa cards: `w-px bg-slate-100 hidden md:block`

### Next Session card (1 cột, flex-[1.2])

- Nền: `bg-slate-50 hover:bg-slate-100 transition-colors`
- Icon: `material-symbols-outlined text-primary text-3xl mb-3` (`timer`)
- Label: `text-xs font-bold text-primary/70 uppercase tracking-widest mb-1`
- Countdown: `text-2xl font-bold text-slate-900`
- Detail: `text-xs text-slate-500 mt-1`

### Loading skeleton

```html
bg-white rounded-xl border border-slate-200 shadow-sm animate-pulse h-48 ├──
header: flex gap, div h-8 w-64 bg-slate-200 rounded + div h-8 w-32 └── body:
flex gap-4 → 3× (flex-1 h-24 bg-slate-100 rounded)
```

---

## 4. WeeklySchedule

```
┌──────────────────────────────────────────────────────────────┐
│ 📅 Weekly Schedule                     [View Full Calendar]  │
├────────┬────────┬────────┬────────┬────────┬────────┬────────┤
│  MON   │  TUE   │  WED   │  THU   │  FRI   │  SAT   │  SUN  │
│   7    │   8    │ [●9]   │  10    │  11    │  12    │  13   │
│[09:00] │        │[09:00] │        │        │        │       │
│ Math   │        │English │        │        │        │       │
│ R.201  │  —     │ R.301  │  —     │  —     │  —     │  —    │
└────────┴────────┴────────┴────────┴────────┴────────┴────────┘
```

### Container

- `bg-white rounded-lg border border-slate-200 overflow-hidden`
- Header: `px-6 py-4 border-b border-slate-200 flex justify-between items-center bg-slate-50/50`
- Header title: `font-bold text-slate-900 flex items-center gap-2` + icon `text-slate-400`
- "View Full Calendar": `text-xs font-medium text-primary hover:text-primary/80`
- Body: `p-6 overflow-x-auto` → inner: `min-w-[500px] flex gap-4`

### DayCard

| State  | Border                                    | Background    | Day label color  |
| ------ | ----------------------------------------- | ------------- | ---------------- |
| Normal | `border-transparent hover:bg-slate-50`    | transparent   | `text-slate-400` |
| Today  | `border-primary/20 ring-1 ring-primary/5` | `bg-slate-50` | `text-primary`   |

- Mỗi card: `flex-1 min-w-[140px] rounded-lg p-3 transition-colors`
- Header day: `text-center mb-4 pb-2 border-b` (slate-200 today / slate-100 normal)
- Day label: `text-xs font-semibold uppercase`
- Date number: `text-lg font-bold text-slate-900`

### Class item trong DayCard

- Nền: `bg-white` (today) / `bg-slate-50 border border-slate-200` (other)
- Today: thêm `border-l-4 border-{cls.color} shadow-sm` (màu từ data)
- Non-today: `opacity-75`
- Time: `text-xs font-bold text-slate-900`
- Subject: `text-xs text-slate-600 font-medium`
- Room: `text-xs text-slate-400 mt-1`
- Empty state: `flex items-center justify-center h-24 text-slate-400 text-xs italic`

---

## 5. CurrentCourses

```
Current Courses                                    [View All →]
┌───────────────────────────────────────────────────────────┐
│ [🎨] Course Title                               65%   [>] │
│      ████████████░░░░░░ (progress bar)                    │
│      Next: Assignment • Due: Mon, Mar 9                   │
└───────────────────────────────────────────────────────────┘
```

- Header: `flex justify-between items-end mb-4 px-1`
- Title: `text-xl font-bold text-slate-900`
- "View All" link: `text-sm font-medium text-slate-500 hover:text-primary`
- List: `space-y-3`

### CourseCard

- `group bg-white p-4 rounded-lg border border-slate-200 hover:shadow-md transition-shadow`
- Layout: `flex flex-col sm:flex-row gap-4 items-start sm:items-center`
- Icon box: `size-12 rounded-lg {iconBg} flex items-center justify-center shrink-0`
  - Icon: `material-symbols-outlined {iconColor}`
- Title: `font-semibold text-slate-900 truncate`
- Progress %: `text-xs font-bold text-slate-500`
- Progress bar: track `bg-slate-100 rounded-full h-1.5` → fill `{progressColor} h-1.5 rounded-full`
- Footer text: `text-xs text-slate-500 mt-2`
- Due date: `text-orange-500`
- Arrow button: `shrink-0 p-2 rounded hover:bg-slate-50 text-slate-400 hover:text-primary` + `chevron_right` icon

---

## 6. QuickActions

```
┌──────────────────────────────────────┐
│ ⚡ Quick Actions                     │
│  ┌──────────┬──────────┐             │
│  │ 📝 Join  │ 📚 Find  │             │
│  │ Session  │ Tutor    │             │
│  ├──────────┼──────────┤             │
│  │ 📖 My    │ 📅 Sche- │             │
│  │ Classes  │ dule     │             │
│  └──────────┴──────────┘             │
└──────────────────────────────────────┘
```

- Card: `bg-white rounded-lg border border-slate-200 p-5`
- Title: `font-bold text-slate-900 mb-4 flex items-center gap-2` + `bolt` icon `text-slate-400`
- Grid: `grid grid-cols-2 gap-3`
- Mỗi button: `flex flex-col items-center justify-center p-4 rounded-lg bg-slate-50 hover:bg-primary hover:text-white text-slate-600 transition-all group border border-slate-100`
- Icon: `material-symbols-outlined mb-2 group-hover:scale-110 transition-transform`
- Label: `text-xs font-semibold`

---

## 7. RecentNotifications

```
┌──────────────────────────────────────┐
│ 🔔 Recent                     [✓]   │
├──────────────────────────────────────┤
│ ● New homework posted          5m   │
│ ● Tutor confirmed session      1h   │
│ ● Reminder: Math test tomorrow 2h   │
├──────────────────────────────────────┤
│         View all notifications       │
└──────────────────────────────────────┘
```

- Card: `bg-white rounded-lg border border-slate-200`
- Header: `px-5 py-4 border-b border-slate-200 flex justify-between items-center`
- Title: `font-bold text-slate-900 flex items-center gap-2` + `notifications` icon `text-slate-400`
- Mark-read icon button: `mark_email_read text-[20px] text-slate-400 hover:text-slate-600`
- List: `divide-y divide-slate-100`
- Item: `p-4 flex gap-3 hover:bg-slate-50 transition-colors cursor-pointer`
  - Dot: `mt-1 size-2 rounded-full {notification.color} shrink-0` (màu từ data, vd `bg-blue-500`)
  - Text: `text-sm font-medium text-slate-900 leading-tight`
  - Time: `text-xs text-slate-500 mt-1`
- Footer link: `p-2 text-center border-t border-slate-200` + `text-xs font-semibold text-primary py-1 hover:underline`

---

## 8. StudyStreak

```
┌─────────────────────────────────────┐  ← gradient amber→orange
│  STUDY STREAK         [📈 circle]  │
│  🔥 15 days                        │
└─────────────────────────────────────┘
```

- Container: `rounded-lg p-5 text-white flex items-center justify-between shadow-lg hover:shadow-xl transition-shadow`
- **Gradient**: `linear-gradient(to bottom right, #f59e0b, #ea580c)` (amber-500 → orange-600)
- Label: `text-xs text-amber-100 font-medium uppercase tracking-wide`
- Value: `text-2xl font-bold mt-1 text-white`
- Icon circle: `h-10 w-10 rounded-full bg-white/20 backdrop-blur-sm flex items-center justify-center` + `trending_up` icon

---

## 9. Colors sử dụng (bổ sung so với Login/Signup)

| Token         | HEX              | Dùng ở                                                              |
| ------------- | ---------------- | ------------------------------------------------------------------- |
| `primary`     | (custom CSS var) | Nav active, links, stat badge, button hover bg, search border-focus |
| `blue-600`    | `#2563eb`        | StatCard My Classes icon + hover                                    |
| `orange-600`  | `#ea580c`        | StatCard Pending Tasks icon + gradient streak                       |
| `amber-500`   | `#f59e0b`        | Streak gradient start                                               |
| `red-500`     | `#ef4444`        | Notification bell badge                                             |
| `red-600`     | `#dc2626`        | StatCard Upcoming icon                                              |
| `green-600`   | `#16a34a`        | Progress bar màu (tùy data)                                         |
| `slate-50/50` | rgba f8fafc 50%  | StatCard bg, header section                                         |
| `white/80`    | rgba fff 80%     | Header sticky bg (+ blur)                                           |

> **`primary` color cần khai báo trong CSS root**. Trong React app dùng custom Tailwind token. Khi port Blazor cần set CSS variable `--color-primary`.

---

## 10. Icons

Toàn bộ dashboard dùng **Material Symbols Outlined** (Google Fonts):

```html
<link
  href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined"
  rel="stylesheet"
/>
```

| Icon name         | Dùng ở                     |
| ----------------- | -------------------------- |
| `event_upcoming`  | Term badge                 |
| `school`          | My Classes stat            |
| `checklist`       | Pending Tasks stat         |
| `event_busy`      | Upcoming stat              |
| `timer`           | Next Session               |
| `calendar_month`  | Weekly Schedule title      |
| `bolt`            | Quick Actions title        |
| `notifications`   | Notifications title + bell |
| `mark_email_read` | Mark all read btn          |
| `trending_up`     | Study Streak icon          |
| `search`          | Search bar                 |
| `chevron_right`   | Course card arrow          |

---

## 11. Loading Skeletons

Mỗi widget có skeleton riêng khi `loading=true`, dùng `animate-pulse`:

| Widget         | Skeleton                                            |
| -------------- | --------------------------------------------------- |
| WelcomeBanner  | `h-48`, header 2 bars (`h-8`), body 3 blocks `h-24` |
| WeeklySchedule | (xử lý bởi widget wrapper, data rỗng → empty state) |
| CurrentCourses | (xử lý bởi widget wrapper)                          |
| Notifications  | (xử lý bởi widget wrapper)                          |

**Skeleton style**: `bg-slate-200 rounded` cho text, `bg-slate-100 rounded` cho blocks lớn hơn.

---

## 12. Spacing tổng thể

| Vùng                        | Giá trị                      |
| --------------------------- | ---------------------------- |
| Main padding                | `px-4 sm:px-6 lg:px-8 py-8`  |
| WelcomeBanner bottom margin | `mb-8` (32px)                |
| Grid gap                    | `gap-8` (32px)               |
| Giữa widgets (left/right)   | `space-y-8` (32px)           |
| Card padding                | `p-5` hoặc `p-6`             |
| Section header padding      | `px-5 py-4` hoặc `px-6 py-4` |
