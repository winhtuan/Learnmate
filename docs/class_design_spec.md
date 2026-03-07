# Class Feature – Design Specification (React → Blazor)

## 1. Cấu trúc trang (6 pages dùng chung layout)

Mọi trang class đều dùng layout:

```
┌────────────────────────────────────────────────────────────┐
│ [Sidebar – fixed left, collapsible]                        │
│                                                            │
│  [main] flex-1 pl-32 pr-6~10 py-6                         │
│    [header] Breadcrumb + SearchBar + menu btn              │
│    [HeroBanner] (chỉ trên trang detail, asg, material…)   │
│    [TabNavigation] (chỉ trên trang detail trở đi)         │
│    [Content theo từng trang]                               │
└────────────────────────────────────────────────────────────┘
```

- Nền toàn trang: `bg-background-light font-display text-slate-900 min-h-screen flex overflow-x-hidden`

---

## 2. Sidebar (Collapsible Icon Rail)

```
 ┌──────┐          ← thu nhỏ: w-[88px], rounded-[3rem]
 │  🏫  │
 │  🏠  │  Dashboard
 │  📚  │  Classes  ← active
 │  📅  │  Schedule
 │ ───  │
 │  ⚙️  │  Settings
 │ [🧑] │  User
 └──────┘          → hover: w-64, label hiện ra
```

| Property          | Value                                        |
| ----------------- | -------------------------------------------- |
| Position          | `fixed left-6 top-1/2 -translate-y-1/2 z-50` |
| Width (collapsed) | `w-[88px]` → hover: `w-64`                   |
| Transition        | `transition-[width] duration-300 ease-out`   |
| Shape             | `rounded-[3rem]` (48px border-radius)        |
| Background        | `bg-white`                                   |
| Shadow            | `shadow-[0_8px_30px_rgb(0,0,0,0.12)]`        |
| Border            | `border border-slate-100`                    |
| Padding           | `py-8 px-4`                                  |

### Logo icon

- `size-10 bg-primary rounded-full text-white shadow-lg shadow-primary/30`
- Icon: `school` (Material Symbols), `text-[24px]`
- Brand text: `opacity-0 group-hover:opacity-100 transition-opacity duration-300 delay-75`

### Nav item (active)

- `bg-primary/10 text-primary`
- Icon: `text-[28px] shrink-0 w-8`
- Label: `font-bold text-base`

### Nav item (inactive)

- `hover:bg-slate-100 text-slate-500 hover:text-primary`
- Label: `font-medium text-base opacity-0 group-hover:opacity-100`
- Shape: `rounded-full p-3`

### Footer

- Divider: `h-px w-full bg-slate-100 mb-2`
- Settings link: `hover:bg-slate-100 text-slate-500 hover:text-primary rounded-full p-3`
- Avatar: `size-10 rounded-full border-2 border-white shadow-sm`
- User name: `font-bold text-sm text-slate-900`
- User role: `text-xs text-slate-500`

---

## 3. Header (Breadcrumb + Search)

```
[Current Term  /  Classes]          [🔍 Search…]   [☰ mobile]
```

- Container: `flex items-center justify-between mb-8 gap-4`
- Breadcrumb: standard component (inactive: `text-slate-500`, active: `text-slate-900 font-medium`)
- SearchBar: xem mục dưới
- Mobile menu btn: `md:hidden p-2 text-slate-600` + `menu` icon

### SearchBar

- `flex items-center gap-2 bg-white border border-slate-200 rounded-full px-4 py-2 text-sm text-slate-500 hover:border-primary/40 transition-colors shadow-sm`
- Icon `search` (Material Symbols) inline trái

---

## 4. ClassPage (Danh sách lớp)

```
[Breadcrumb + Search]

[ All ]  [ Active ]  [ Upcoming ]  ← FilterTabs
 ──────────────────────────────────────
[📘 Courses: 4]  [✅ Tasks: 12]  [📅 Next: Math – 09:00]
 ──────────────────────────────────────
 ┌────────┐  ┌────────┐  ┌────────┐
 │ClassCard│  │ClassCard│  │ClassCard│
 └────────┘  └────────┘  └────────┘
```

### ClassFilterTabs

- `flex gap-2`
- Tab active: `bg-primary text-white px-5 py-2 rounded-full text-sm font-bold`
- Tab inactive: `px-5 py-2 rounded-full text-sm font-medium text-slate-500 hover:bg-slate-100`

### ClassStatsCard (3 loại)

**courses / tasks** (nền trắng):

- `bg-white p-6 rounded-[2rem] shadow-sm border border-slate-100 flex items-center gap-4`
- Icon circle: `size-12 rounded-full bg-blue-100 text-primary` (`courses`) / `bg-emerald-100 text-emerald-600` (`tasks`)
- Label: `text-sm text-slate-500`
- Value: `text-2xl font-bold text-slate-900`

**nextClass** (nền primary):

- `bg-primary p-6 rounded-[2rem] shadow-lg shadow-primary/20 flex items-center justify-between text-white`
- Label: `text-sm text-blue-100`
- Title: `text-xl font-bold mt-1 text-white`
- Time: `text-sm text-blue-100 mt-1`
- Arrow circle: `size-10 bg-white/20 rounded-full backdrop-blur-sm` + `arrow_forward` icon
- Decorative blob: `absolute -right-4 -bottom-4 size-24 bg-white/10 rounded-full blur-2xl`

### ClassCard

```
┌──────────────────────────────────────────────┐
│  [Image bg h-48 rounded-[2rem]]              │
│    [CODE badge top-left]   [⚠️ Due badge top-right] │
│    [Professor name bottom-left]              │
├──────────────────────────────────────────────┤
│  Course Title              [⋯ menu]          │
│  Description (2 lines max)                   │
│                                              │
│  📅 Next: Monday 09:00                       │
│  [  Join Class  ]   [📁]                    │
└──────────────────────────────────────────────┘
```

- Card: `group relative flex flex-col bg-white rounded-[2.5rem] p-4 shadow-sm hover:shadow-xl hover:shadow-slate-200/50 transition-all duration-300 border border-slate-100`
- Upcoming state: `opacity-60 hover:opacity-100 grayscale hover:grayscale-0`
- Image: `h-48 rounded-[2rem] overflow-hidden` + overlay `bg-gradient-to-t from-black/60 to-transparent`
- Code badge: `bg-white/90 backdrop-blur px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider text-slate-800 shadow-sm`
- Warning badge: `bg-rose-500 text-white px-3 py-1 rounded-full text-xs font-bold shadow-lg shadow-rose-500/30 flex items-center gap-1` + `warning` icon
- Title: `text-xl font-bold text-slate-900 group-hover:text-primary transition-colors`
- Description: `text-slate-500 text-sm mb-6 line-clamp-2`
- Next class box: `bg-slate-50 p-3 rounded-2xl border border-slate-100 flex items-center gap-3`
  - Icon: `calendar_month text-primary text-[20px]`
  - Label: `text-slate-500 font-medium text-sm` + value: `text-slate-900 font-bold`
- Primary CTA: `flex-1 bg-slate-900 text-white py-3 rounded-2xl font-semibold text-sm hover:opacity-90`
- "Submit Task" variant: `border-2 border-primary/20 hover:border-primary hover:bg-primary hover:text-white py-3 rounded-2xl font-bold text-sm transition-all`
- Folder btn: `size-11 rounded-2xl border border-slate-200 hover:bg-slate-50 text-slate-600` + `folder_open` icon
- Dropdown menu: `bg-white rounded-2xl shadow-xl border border-slate-100 py-2 min-w-[180px]`
  - "Open Class": `text-sm text-slate-700 hover:bg-slate-50 px-4 py-2.5`
  - "Leave Class": `text-sm text-rose-600 hover:bg-rose-50 px-4 py-2.5`

---

## 5. ClassDetailPage (Overview tab)

```
[HeroBanner]
[TabNavigation: Overview | Assignments | Materials | Schedule | Video Sessions]

 ┌──────────────────────────┬────────────────────┐
 │  [NextUpSection]         │  [InstructorCard]  │
 │  [ActiveAssignments]     │  [ResourcesList]   │
 └──────────────────────────┴────────────────────┘
```

Grid: `grid grid-cols-1 lg:grid-cols-3 gap-8` → left: `lg:col-span-2`, right: 1 col

### HeroBanner

```
┌────────────────────────────────────────────────────────┐
│  [bg image + gradient layers]                          │
│  [CODE badge]  [● ACTIVE badge]                        │
│  Course Title (text-4xl~6xl extrabold text-white)      │
│  Description (text-slate-200 text-lg~xl font-light)    │
│                            [▶ Resume] [⋮ more]          │
└────────────────────────────────────────────────────────┘
```

- Container: `relative w-full h-[280px] md:h-[320px] rounded-[3rem] overflow-hidden mb-8 shadow-2xl shadow-slate-200`
- Layers:
  1. Base: `absolute inset-0 bg-slate-900`
  2. Image: `absolute inset-0 bg-cover bg-center opacity-70 mix-blend-overlay`
  3. Color wash: `absolute inset-0 bg-gradient-to-r from-primary/90 to-purple-600/80 mix-blend-multiply`
  4. Bottom fade: `absolute inset-0 bg-gradient-to-t from-black/80 via-black/20 to-transparent`
- Code badge: `bg-white/20 backdrop-blur-md text-white px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider border border-white/10`
- Status badge (Active): `bg-emerald-500/20 backdrop-blur-md text-emerald-300 px-3 py-1 rounded-full text-xs font-bold uppercase border border-emerald-500/20`
  - Dot: `w-1.5 h-1.5 rounded-full bg-emerald-400 animate-pulse`
- Resume btn: `px-5 py-3 bg-white text-slate-900 rounded-full font-bold text-sm hover:bg-slate-100 shadow-lg flex items-center gap-2`
  - Icon: `play_circle text-[20px]`
- More btn: `w-12 h-12 bg-white/10 backdrop-blur-md text-white rounded-full border border-white/20 hover:bg-white/20`

### TabNavigation

- Container: `flex items-center gap-1 mb-8 overflow-x-auto pb-2 border-b border-slate-200`
- Active tab: `text-primary border-b-2 border-primary font-bold px-5 py-3 text-sm`
- Inactive tab: `text-slate-500 hover:text-slate-900 font-medium px-5 py-3 text-sm`
- Badge: `ml-1.5 px-2 py-0.5 rounded-full bg-slate-100 text-xs font-bold text-slate-600`
- Tabs: Overview, Assignments, Materials, Schedule, Video Sessions

---

## 6. ClassAsgPage (Assignments tab)

```
[Upcoming]  ← section label: text-xs font-bold text-slate-400 uppercase tracking-wider
  [AssignmentCard]
  [AssignmentCard]
[Missing]
  [AssignmentCard]
[Completed] ← opacity-60 hover:opacity-100 transition-opacity
  [AssignmentCard]

                        [AssignmentDetailPanel]  ← right col
```

### AssignmentCard

```
┌────────────────────────────────────────────────────────┐
│ [│] [🎨 16×16 icon]  Title (bold lg)    DUE   [Status] │
│ ●  highlighted: border-2 border-primary shadow-primary  │
│     Metadata (sm text-slate-500)        Mon, Mar 9      │
└────────────────────────────────────────────────────────┘
```

- Base: `bg-white p-4 rounded-2xl border cursor-pointer flex gap-4 transition-all`
- Normal border: `border-slate-200 hover:border-slate-300`
- Highlighted: `border-2 border-primary shadow-lg shadow-primary/5` + left bar: `absolute left-0 top-0 bottom-0 w-1 bg-primary`
- Completed: `opacity-60 hover:opacity-100` + title `line-through decoration-slate-400`
- Icon box: `size-12 rounded-xl {iconBg} {iconColor} flex items-center justify-center border {color-border-100}`
- Title: `font-bold text-slate-900 text-lg group-hover:text-primary transition-colors`
- Due label: `text-xs font-bold text-slate-500 uppercase tracking-wide`
- Due date: `font-semibold text-sm {dueColor}`
- Status badge: `px-2.5 py-1 rounded-lg {statusBg} {statusColor} text-xs font-bold border {color-border-200}`

---

## 7. ClassMaterialPage (Materials tab)

```
┌──────────────────────────────────────────────────────────┐  ← rounded-[2.5rem]
│  Course Materials                [toolbar: Sort/Filter]  │
├──────────────────────────────────────────────────────────┤
│  FOLDERS  [⊞ grid] [≡ list]                              │
│  ┌────────┐  ┌────────┐  ┌────────┐                      │
│  │FolderCard│  │FolderCard│  │FolderCard│                │
│  └────────┘  └────────┘  └────────┘                      │
│                                                          │
│  RECENT FILES                                            │
│  ┌────────────────────────────────────────────────────┐  │
│  │  Name         │ Date Added │  Size  │      [↓]     │  │
│  │  file row     │           │        │              │  │
│  └────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────┘
```

- Card container: `bg-white rounded-[2.5rem] border border-slate-100 shadow-sm min-h-[600px]`
- Section title `h2`: `text-2xl font-bold text-slate-900`
- Section label `h3`: `text-xs font-bold text-slate-400 uppercase tracking-wider`
- View toggle: grid_view active = `text-primary`, list = `text-slate-300 hover:text-primary`
- Folder grid: `grid grid-cols-1 md:grid-cols-3 gap-6 mb-10`
- Table header: `bg-slate-50 text-xs font-bold text-slate-500 uppercase py-4`
- Table: `rounded-xl border border-slate-100 overflow-x-auto w-full`

---

## 8. ClassVideoPage (Video Sessions tab)

```
[LiveSessionCard – dark bg]

Recorded Lectures                    [⊞] [≡]
┌──────────┐  ┌──────────┐  ┌──────────┐
│VideoCard │  │VideoCard │  │VideoCard │
└──────────┘  └──────────┘  └──────────┘
```

### LiveSessionCard

```
┌── white outer rounded-[2.5rem] p-1 ──────────────────────┐
│  ┌── bg-slate-900 rounded-[2.2rem] ──────────────────────┐│
│  │  [glow blob primary/30 blur-100]                      ││
│  │  ● LIVE Now   📅 Monday, March 10                     ││
│  │                                                       ││
│  │  Session Title (text-3xl~4xl bold white)              ││
│  │  Description (text-slate-400 text-lg)                 ││
│  │                                                       ││
│  │  [📹 Join Room]  [🧑🧑🧑 N+ Students waiting]          ││
│  │                                    [🧑 Hosted by …]   ││
│  └───────────────────────────────────────────────────────┘│
└──────────────────────────────────────────────────────────┘
```

- Outer: `bg-white rounded-[2.5rem] p-1 border border-slate-100 shadow-xl shadow-slate-200/50`
- Inner: `rounded-[2.2rem] bg-slate-900 overflow-hidden relative`
- Glow: `absolute top-0 right-0 w-[500px] h-[500px] bg-primary/30 rounded-full blur-[100px]`
- LIVE badge: `bg-rose-500/20 text-rose-300 border border-rose-500/30 px-3 py-1 rounded-full text-xs font-bold uppercase` + ping dot
- Join Room btn: `px-8 py-4 bg-primary text-white rounded-2xl font-bold shadow-lg shadow-primary/40 hover:scale-[1.02] active:scale-[0.98] flex items-center gap-3`
  - Icon: `videocam`
  - Ping badge: `animate-ping absolute -top-1 -right-1 h-4 w-4 bg-white opacity-40 rounded-full`
- Avatar stack: `flex -space-x-3`, size-8 circles `border-2 border-slate-900`
- Host card: `bg-white/10 backdrop-blur-md border border-white/10 p-5 rounded-2xl`

- View toggle (Recorded section): `flex bg-white p-1 rounded-xl border border-slate-200`
  - Active btn: `p-2 rounded-lg hover:bg-slate-100 text-primary`
  - Inactive btn: `p-2 rounded-lg hover:bg-slate-100 text-slate-400 hover:text-slate-600`
- Grid: `grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6`

---

## 9. ClassSchedulePage (Schedule tab)

- Layout: `flex flex-col gap-6 pb-10`
- ScheduleHeader: tháng/năm navigation
- ScheduleCalendar: calendar grid view
- EventTypeLegend: danh sách loại event với màu

---

## 10. Color Palette bổ sung

| Token            | HEX                     | Dùng ở                                            |
| ---------------- | ----------------------- | ------------------------------------------------- |
| `primary`        | CSS var                 | Active tab, nav active, badges, buttons, progress |
| `purple-600/80`  | rgba 147,51,234 80%     | HeroBanner gradient tint                          |
| `emerald-500/20` | rgba 16,185,129 20%     | Status badge Active                               |
| `emerald-300`    | `#6ee7b7`               | Active status text                                |
| `rose-500`       | `#f43f5e`               | Warning badge, Live status                        |
| `blue-100`       | `#dbeafe`               | Courses icon bg                                   |
| `emerald-100`    | `#d1fae5`               | Tasks icon bg                                     |
| `amber-200`      | `#fde68a`               | Assignment upcoming border                        |
| `white/20`       | rgba 100%,100%,100% 20% | Frosted glass buttons trên dark bg                |

---

## 11. Border radius "design language"

Các trang Class dùng border-radius lớn hơn Login/Dashboard:

| Element               | Border radius             |
| --------------------- | ------------------------- |
| Sidebar               | `rounded-[3rem]` (48px)   |
| HeroBanner            | `rounded-[3rem]`          |
| ClassCard             | `rounded-[2.5rem]` (40px) |
| LiveSessionCard outer | `rounded-[2.5rem]`        |
| LiveSessionCard inner | `rounded-[2.2rem]`        |
| Materials container   | `rounded-[2.5rem]`        |
| ClassStatsCard        | `rounded-[2rem]` (32px)   |
| AssignmentCard        | `rounded-2xl` (16px)      |
| Dropdown menu         | `rounded-2xl`             |
| Join Room btn         | `rounded-2xl`             |
| Resume btn            | `rounded-full`            |

---

## 12. Routing

| Route                      | Page                       |
| -------------------------- | -------------------------- |
| `/classes`                 | ClassPage                  |
| `/classes/:id`             | ClassDetailPage (Overview) |
| `/classes/:id/assignments` | ClassAsgPage               |
| `/classes/:id/materials`   | ClassMaterialPage          |
| `/classes/:id/schedule`    | ClassSchedulePage          |
| `/classes/:id/videos`      | ClassVideoPage             |
