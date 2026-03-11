# tutor-ui-improvement.md

## Objective

Refactor the Tutor listing UI to look like a production-level marketplace interface.
Improve visual hierarchy, card layout, interaction design, and consistency across components.

Affected files:

* `FindTutorPage.razor`
* `TutorListCard.razor`
* `TutorQuickView.razor`

Goal:

* Modern marketplace-style UI
* Better scanability
* Clear CTA
* Consistent spacing and colors
* Smooth interactions

---

# 1. Tutor Card Layout Refactor

Refactor `TutorListCard.razor` into a **3-column layout**.

Structure:

```
| Avatar | Tutor Info | Price + Actions |
```

### Left Section

Contains:

* Tutor avatar
* Rating
* Online indicator

Example:

```
Avatar
⭐ 4.9 (124)
```

Avatar style:

```
rounded-full
w-16 h-16
object-cover
```

---

### Center Section

Contains main tutor information.

Order:

1. Tutor name
2. Verified badge
3. Subjects
4. Short description
5. Tags

Example structure:

```
Name + Verified

Subjects

Short description

Tags
```

Name style:

```
text-lg
font-semibold
text-slate-900
```

Subjects should be displayed as **tags**.

Example tag style:

```
px-2 py-1
text-xs
bg-slate-100
rounded-md
```

Example:

```
Math   Physics   IELTS
```

Description style:

```
text-sm
text-slate-600
line-clamp-2
```

---

### Right Section

Contains pricing and action buttons.

Layout:

```
$18
/hour

Availability Badge

[View Profile]
[Book Lesson]
```

Price style:

```
text-xl
font-bold
text-slate-900
```

Hour text:

```
text-xs
text-slate-500
```

---

# 2. Availability Badge

Add tutor availability state.

States:

### Available

```
bg-emerald-100
text-emerald-700
```

### Few slots left

```
bg-amber-100
text-amber-700
```

### Fully booked

```
bg-gray-100
text-gray-500
```

Badge style:

```
text-xs
px-2
py-1
rounded-md
```

---

# 3. Card Visual Design

Upgrade card styling.

Card container:

```
bg-white
rounded-2xl
shadow-sm
border border-slate-200
p-6
```

Hover interaction:

```
hover:shadow-lg
hover:-translate-y-0.5
transition-all duration-200
```

Spacing:

```
gap-6
```

---

# 4. Rating Display

Display rating with review count.

Example:

```
⭐ 4.9 (124)
```

Style:

```
text-sm
text-slate-600
```

---

# 5. Call-To-Action Buttons

Each tutor card should have clear actions.

Primary action:

```
Book Lesson
```

Secondary:

```
View Profile
```

Primary button style:

```
bg-primary
text-white
rounded-lg
px-4
py-2
font-medium
hover:bg-primary/90
```

Secondary button style:

```
border
border-slate-200
rounded-lg
px-4
py-2
```

---

# 6. Hover Micro Interaction

Add subtle hover animation.

Card hover:

```
transform
hover:-translate-y-0.5
transition duration-200
```

Avatar hover:

```
group-hover:scale-105
transition-transform
```

---

# 7. Grid Layout for Tutor List

In `FindTutorPage.razor`, change tutor list layout.

Use responsive grid.

```
grid
grid-cols-1
md:grid-cols-2
xl:grid-cols-3
gap-6
```

---

# 8. Filter Sidebar

Add a left filter panel.

Sections:

* Subjects
* Price Range
* Rating
* Availability
* Language

Sidebar style:

```
bg-white
rounded-xl
shadow-sm
p-4
sticky top-24
```

Filter title style:

```
text-sm
font-semibold
text-slate-800
```

---

# 9. Search Bar

Add marketplace-style search at top of page.

Structure:

```
[ Search subject ] [ Price ] [ Rating ] [ Availability ] [ Search ]
```

Style:

```
rounded-full
border
shadow-sm
px-5
py-3
bg-white
```

---

# 10. Tutor Quick View Modal

Refactor `TutorQuickView.razor`.

Structure:

```
Avatar + Name
Rating
Subjects
Bio
Experience
Price
[Book Lesson]
```

Modal container:

```
bg-white
rounded-2xl
shadow-xl
p-6
max-w-2xl
```

Animation:

```
scale-95 -> scale-100
opacity-0 -> opacity-100
transition duration-150
```

---

# 11. Skeleton Loading

When tutor list is loading, show skeleton cards.

Skeleton styles:

```
animate-pulse
bg-slate-200
rounded-lg
```

Skeleton card contains:

* Avatar circle
* Name line
* 2 description lines
* Button placeholder

---

# 12. Color System

Define consistent color palette.

### Neutral

```
Background  #F8FAFC
Card        #FFFFFF
Border      #E2E8F0
Text        #0F172A
Subtext     #64748B
```

### Primary

```
Primary     #6366F1
Hover       #4F46E5
Light       #EEF2FF
```

### Status

```
Success     #10B981
Warning     #F59E0B
Danger      #EF4444
```

---

# 13. Spacing System

Use consistent spacing scale.

```
gap-2   small spacing
gap-4   element spacing
gap-6   card layout spacing
gap-8   section spacing
```

Padding scale:

```
p-2
p-4
p-6
```

---

# 14. Micro UX Improvements

Add small details that improve perceived quality.

Include:

* Verified tutor badge
* Online status indicator
* Tooltip for verified icon
* Smooth hover animations
* Consistent icon system

---

# Expected Result

After refactoring, the UI should resemble a modern tutor marketplace like:

* Preply
* Italki
* Cambly

Characteristics:

* Clean card layout
* Clear pricing
* Easy tutor comparison
* Clear booking action
* Smooth interactions
