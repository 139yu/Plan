---
name: study-plan
description: Generate detailed study plans with daily tracking, progress recording, and status queries. Use when the user asks to "create study plan", "制定学习计划", "学习计划", "check study progress", "查看学习进度", "start today's study", "开始今天学习", "今天学什么", "end today's study", "结束今天学习", "update study progress", "更新学习进度", "我今天学了", "我完成了", "我学完了", "查看进度", "我的进度", "本周总结", "这周学了什么", "调整计划", "我落后了", "不想学", "休息一天", or discusses learning schedules, progress, and adjustments.
---

# Study Plan Generator & Tracker

Generates detailed, executable study plan documents with daily breakdowns, goal tracking, and live progress recording. The plan file lives in the current working directory and is updated on EVERY progress-related interaction.

## Core Principles

1. **One plan, one file** — Always use the fixed filename `study-plan.md` in the current working directory. No topic-slug variants. If `study-plan.md` already exists when creating a new plan, ask the user whether to archive or overwrite.
2. **Every update is persisted** — Any time the user mentions study progress (formal or casual), immediately update `study-plan.md` to reflect the latest state. NEVER defer a file update.
3. **Date-aware** — Always compare the actual current date against the plan's start date to determine which "Day N" is today. Handle gaps and skipped days gracefully.
4. **Fault-tolerant** — If `study-plan.md` is malformed or missing expected sections, attempt to recover what data you can and prompt the user about what's lost. Never silently fail.

---

## Capabilities

| # | Capability | Trigger Examples |
|---|-----------|------------------|
| 1 | Create a new study plan | "制定学习计划", "create study plan" |
| 2 | Show today's tasks | "开始今天学习", "今天学什么" |
| 3 | Record progress (casual) | "我学完了第三章", "今天完成了前两项" |
| 4 | End-of-day summary | "结束今天学习", "今天学完了" |
| 5 | Query overall progress | "查看学习进度", "我的进度" |
| 6 | Weekly review | "本周总结", "这周学了什么" |
| 7 | Adjust plan | "调整计划", "我落后了" |
| 8 | Rest/skip day | "今天不想学", "休息一天", "请假" |

---

## Phase 1: Create a New Study Plan

### Step 1.1 — Gather Requirements

Ask the user for these 5 pieces of information. If the initial message already contains all of them, skip directly to Step 1.2.

1. **学习内容/主题** (topic) — e.g. "Python数据分析", "雅思7分", "系统设计面试"
2. **学习周期** (duration) — total days or weeks (e.g. 30 days)
3. **每天学习时长** (daily time) — e.g. 2 hours, 1.5 hours
4. **学习目的/目标** (goal) — what the user should be able to do after completion
5. **当前水平** (current level) — 零基础 / 入门 / 中级 / 高级

Ask missing items one at a time or as a batch if multiple are missing. Be conversational, not robotic.

### Step 1.2 — Check for Existing Plan

Before generating: check if `study-plan.md` exists in the current directory.
- If it exists: "当前目录已有学习计划，是否要覆盖？(y) 覆盖 / (n) 归档旧计划后创建新计划"
- If the user chooses archive: rename old file to `study-plan-archive-YYYY-MM-DD.md`

### Step 1.3 — Design the Plan Content

Break the topic into modules, distribute across days with these rules:

- **Module grouping**: 3-6 modules total, each spanning several consecutive days
- **Daily tasks**: 3-6 specific tasks per day, each with a clear actionable description
- **Time alignment**: Task times sum to the user's daily study time (allow ~10min buffer)
- **Progressive difficulty**: Foundation → Application → Advanced → Project
- **Active learning ratio**: At least 40% hands-on practice (exercises, coding, writing, projects)
- **Review days**: Every 5-7 days, insert a review/catch-up day
- **Buffer zone**: Last 2-3 days reserved for final review, mock exam, or capstone project
- **Day 1 preview**: First day includes a 10-15min overview task to preview the entire journey

### Step 1.4 — Generate the Plan File

Create `study-plan.md` in the current directory using this exact template:

```markdown
# 📚 学习计划：[主题]

> **创建时间**: YYYY-MM-DD
> **开始日期**: YYYY-MM-DD
> **结束日期**: YYYY-MM-DD (共 N 天)
> **每日时长**: X 小时
> **当前水平**: [水平]
> **学习目标**: [目标描述]

---

## 📊 总体进度

| 指标 | 数值 |
|------|------|
| 总天数 | N |
| 已完成 | 0 / N |
| 当前天数 | Day 1 |
| 完成率 | 0% |
| 连续学习 | 0 天 |
| 最后更新 | YYYY-MM-DD |

```
进度条: ░░░░░░░░░░░░░░░░░░░░ 0% (0/N)
```

---

## 🗺️ 学习路线图

| 模块 | 内容概要 | 对应天数 | 日期范围 | 状态 |
|------|----------|----------|----------|------|
| 模块1 | [名称+简介] | Day 1–X | MM/DD–MM/DD | ⬜ |
| 模块2 | [名称+简介] | Day X–Y | MM/DD–MM/DD | ⬜ |
| ... | ... | ... | ... | ... |

---

## 📅 每日计划

### Day 1: YYYY-MM-DD — [主题名称]

**今日目标**: [一句话：今天学完能做什么]

| # | 时长 | 内容 | 任务详情 | 状态 |
|---|------|------|----------|------|
| 1 | XXmin | [名称] | [具体可执行的学习内容] | ⬜ |
| 2 | XXmin | [名称] | [具体可执行的学习内容] | ⬜ |
| ... | ... | ... | ... | ⬜ |

**📝 今日小结**: —

---

### Day 2: YYYY-MM-DD — [主题名称]

(repeat for all days)

---

## 🏆 里程碑

- [ ] 模块1完成 (Day X): [验收标准 — 能做什么]
- [ ] 模块2完成 (Day Y): [验收标准]
- [ ] ...
- [ ] 🎯 最终目标: [学习目的]

---

## 📝 学习日志

> 每次学习后自动更新，记录每天的完成情况和心得。

| 日期 | 天数 | 完成 | 得分 | 备注 |
|------|------|------|------|------|
| — | — | — | — | 暂无记录 |

---
```

**Status icons:**
- `⬜` — Not started yet
- `🚧` — In progress (partial completion)
- `✅` — Fully completed
- `❌` — Skipped / not completed today
- `⏸️` — Rest day (deliberately skipped with intent to catch up)

### Step 1.5 — Present Summary

After generating the file, present a concise summary:

```
✅ 学习计划已创建：study-plan.md

📋 概览：
- 主题：[topic]
- 周期：N 天（MM/DD — MM/DD）
- 模块：X 个（列出模块名称）
- 每日：X 小时

💡 你可以：
- 说"开始今天学习"查看今日任务
- 学完说"结束今天学习"记录进度
- 随时说"查看学习进度"了解整体情况
```

---

## Phase 2: Start Today's Study ("开始今天学习" / "今天学什么")

When the user wants to see what to study today:

1. Read `study-plan.md`
2. Determine today's date
3. Match to the corresponding Day in the plan (based on start date)
4. If today is past the end date → "学习计划已结束。需要调整计划延长周期吗？"
5. If today is before start date → "计划还未开始，开始日期是 X。要提前开始吗？"
6. If there's a gap (user skipped days) → Use the "next incomplete day" rule below

**Next incomplete day rule:**

```
Find the earliest day where any task is not ✅ and not ⏸️:
  - If that day == today → normal flow
  - If that day < today → "上次完成是 Day X，今天应该是 Day Y，中间有 Z 天未记录。
    要 (1) 从 Day Y 继续 / (2) 补 Day Y 之前的遗漏 / (3) 跳过进入追赶模式？"
  - If that day > today → user is ahead of schedule, show the next day
```

If the user chooses "catch-up mode" (追赶模式), compact 2 days of tasks into today with essentials only.

**Output format:**

```
📅 今日学习 | Day N: [日期] — [主题]

🎯 今日目标：[目标描述]

任务清单：
┌────┬─────────┬──────────────────────────────────┐
│ #  │ 时长    │ 任务                             │
├────┼─────────┼──────────────────────────────────┤
│ 1  │ 30min   │ [任务名称] — [详情]              │
│ 2  │ 45min   │ [任务名称] — [详情]              │
│ ...│ ...     │ ...                              │
└────┴─────────┴──────────────────────────────────┘

总时长：X 小时

💪 加油！完成后告诉我"结束今天学习"来记录进度。
```

---

## Phase 3: Record Progress (Casual) — "我学完了X" / "我完成了X"

**CRITICAL**: ANY time the user reports any learning progress — even casually — immediately update `study-plan.md`. Do NOT wait for a formal "end today's study."

When the user says something like:
- "我学完了第三章"
- "今天完成了前两项任务"
- "刚看完了视频，做了练习"
- "今天状态不好，只学了1小时"

### Fuzzy Matching Strategy

Match user statements to plan tasks using these strategies in order:

1. **Index matching**: "前两项" → tasks 1 and 2. "第3个" → task 3.
2. **Keyword matching**: Extract key terms from user message (topic names, verbs, nouns) and compare against task names and details. Match if ≥2 significant words overlap, or if a unique distinct word matches.
3. **Content matching**: If user mentions a specific activity ("看了视频", "做了练习", "读完第三章"), look for tasks whose details contain those activities.
4. **Time-based inference**: If user says "学了1小时" and only some tasks add up to that time, mark those as done.
5. **Ask when uncertain**: If exactly one task partially matches (1 keyword overlap), confirm: "你指的是 'XXX' 这项吗？" If zero or multiple equally ambiguous tasks match, list options: "你指的是哪一项？1) XX 还是 2) YY？"

### Actions After Matching

1. Read `study-plan.md` to identify today's day and tasks
2. Match what the user said to specific tasks using the strategy above
3. Update the matched task statuses (✅ / 🚧)
4. If all tasks for a day are now ✅, mark the day complete
5. If a full module's days are all ✅, auto-update the module status in the roadmap
6. Write the updated file
7. Briefly confirm: "已更新：Day N 任务1 ✅ 已完成。今日进度：2/5。"

### Handling Partial Completion

If the user describes partial work on a task (e.g., "看了一半视频"):
- Mark as 🚧 (in progress)
- Note in the day's "今日小结" what was done and what remains
- Prompt: "这部分明天继续？还是等会接着完成？"

---

## Phase 4: End Today's Study ("结束今天学习" / "今天学完了")

Full end-of-day flow. Read the plan, then go through each remaining task:

```
📋 今日学习总结 — Day N: [主题]

逐项确认完成情况：

1. [任务1名称] — ✅ 已完成 / 🚧 部分完成 / ❌ 未做
2. [任务2名称] — ✅ 已完成 / 🚧 部分完成 / ❌ 未做
...

对部分完成的任务：完成了多少？有什么遗留？
今天学习有什么困难或心得？
```

After the user responds:
1. Update ALL task statuses in the plan
2. Calculate today's score: completed tasks / total tasks
3. Fill in "今日小结" with user's notes
4. Update the learning log table with today's entry
5. Update overall progress stats (completion rate, streak, etc.)
6. **Auto-update module status**: If all days in a module have all tasks ✅, mark module as ✅ in the roadmap. If any day has at least one ✅, mark as 🚧.
7. **Auto-update milestones**: If a module just became fully complete, check off the corresponding milestone
8. Show a brief preview of tomorrow
9. If all tasks completed → congratulate. If not → encourage and note what rolls to tomorrow.

**Streak calculation**: Count consecutive days (including today) where at least 50% of tasks were completed. A ⏸️ rest day does NOT break the streak, but does NOT count toward it either.

---

## Phase 5: Query Progress ("查看学习进度" / "我的进度")

Read `study-plan.md` and present:

```
📊 学习进度总览

[进度条]
完成率：X% (M/N 天)

🔥 连续学习：X 天
📅 当前：Day N — [主题]
📆 计划结束：MM/DD（剩余 Y 天）

模块进度：
┌──────────┬──────────┬──────────┐
│ 模块     │ 状态     │ 完成度   │
├──────────┼──────────┼──────────┤
│ 模块1    │ ✅ 完成  │ 5/5      │
│ 模块2    │ 🚧 进行中│ 3/7      │
│ 模块3    │ ⬜ 未开始│ 0/6      │
└──────────┴──────────┴──────────┘

📝 最近记录：
- Day X: 完成 N/M 项 — "笔记内容"
- Day Y: 完成 N/M 项 — "笔记内容"
```

**Motivational messaging based on streak:**
- Streak ≥ 7 days: "🔥 太厉害了！连续7天+，习惯正在养成！"
- Streak 3–6 days: "💪 势头不错，保持下去！"
- Streak 1–2 days: "👏 好的开始！"
- Streak 0 (missed today): "📅 今天还没记录，现在开始也不晚！"

---

## Phase 6: Weekly Review ("本周总结")

Summarize the past 7 calendar days (or since plan start, if less than 7 days have passed):

- Days studied vs days missed
- Tasks completed vs planned
- Best day / worst day
- Current streak
- Average completion rate for the week
- If completion rate < 50%: suggest specific adjustments (reducing daily tasks, extending deadline, or changing study time)
- If completion rate > 90%: suggest whether to accelerate (e.g., skip ahead or add challenge tasks)

---

## Phase 7: Adjust Plan ("调整计划" / "我落后了")

When the user is behind or wants to change pace:

1. Show current state: completed days, remaining days, overdue tasks
2. Offer options:
   ```
   你想：
   (1) 压缩剩余内容保持结束日期 — 合并相似任务，减少复习天数
   (2) 后延结束日期 — 按当前节奏顺延所有未完成日期
   (3) 跳过部分内容 — 你指定要跳过的模块/天数
   (4) 增加每日时长 — 临时或长期提高每天学习量
   ```
3. Based on choice, rewrite affected days in the plan file
4. Recalculate all dates and progress stats
5. Show a before/after summary: "原计划 30 天 → 调整后 35 天，结束日期从 06/15 延至 06/20"

---

## Phase 8: Rest / Skip Day ("今天不想学" / "休息一天" / "请假")

When the user wants to take a day off:

1. Mark ALL incomplete tasks for today as ⏸️ (paused, not ❌)
2. Update learning log with entry: "⏸️ 休息日"
3. Show encouragement: "好好休息！明天继续。当前连续学习 X 天（休息日不中断连续记录）"
4. All subsequent days shift forward by 1 (dates only, day numbers stay, but dates shift)
5. Update overall progress dates accordingly
6. If the user rest 2+ consecutive days, gently ask: "已经休息了2天了，要不要调整一下计划让节奏更轻松？"

---

## File Management Rules

- **Fixed filename**: Always `./study-plan.md` in the current working directory
- **No variants**: Never create `study-plan-xxx.md` variants for new plans. Only `study-plan.md`.
- **Archiving**: When overwriting, rename old file to `study-plan-archive-YYYY-MM-DD.md`
- **Always update after progress**: Every user message about learning progress MUST result in a file write. No exceptions.
- **When directory is unclear**: If not in a project directory, save to `~/Documents/study-plans/study-plan.md`
- **Corruption recovery**: If `study-plan.md` exists but critical sections are missing (no `## 📅 每日计划`, no `## 📊 总体进度`), tell the user: "学习计划文件似乎损坏了（缺少XX部分）。我能从现有内容恢复 YY 数据，但 ZZ 可能需要重建。要继续吗？"

---

## Date-to-Day Mapping

Calculate which "Day N" corresponds to today:

```
elapsed_days = today - start_date
current_day = min(elapsed_days + 1, total_days)
```

- If `elapsed_days < 0` → Plan hasn't started yet
- If `current_day > total_days` → Plan period has ended
- If previous days are incomplete → Current day stays at the first incomplete day; show a gap warning

When a rest day (⏸️) shifts dates, adjust the date-to-day mapping accordingly: each ⏸️ day pushes subsequent dates forward by 1.

---

## Module & Milestone Auto-Sync

After EVERY file update, run this check:

1. For each module in the roadmap, count completed days (all tasks ✅) vs total days
2. Update module status: 100% → ✅ | 1-99% → 🚧 | 0% → ⬜
3. For each milestone, check if its condition is met (all constituent days complete)
4. If a milestone is newly met, check it off: `- [x]`
5. Recalculate overall completion rate and update the progress bar

---

## Example Conversations

### Example 1: Full Creation Flow

```
User: 制定学习计划，学React，30天，每天1.5小时，目标是独立开发一个后台管理系统

Claude: 好的。请问你目前的编程基础如何？
        ( ) 零基础，HTML/CSS/JS 都不会
        ( ) 有 JS 基础，但没接触过框架
        ( ) 用过 Vue/Angular，想转 React

User: 有JS基础，没用过框架

Claude: [Generates study-plan.md with 30 days across 5 modules]
        ✅ 学习计划已创建...
```

### Example 2: Casual Progress Update

```
User: 刚看完JSX和组件的视频，还写了几个小demo

Claude: [Reads study-plan.md, identifies Day 1 tasks via keyword matching]
        已更新：
        ✅ 任务1: JSX语法和组件概念 — 已完成
        ✅ 任务2: 编写第一个组件demo — 已完成
        今日进度：2/4，继续加油！
```

### Example 3: End of Day

```
User: 结束今天学习

Claude: [Shows task list, user confirms]
        已记录今日学习：
        Day 3: React状态管理 — 得分 4/5 ✅
        📝 小结：useState和useReducer理解了，useContext还需要多练

        🔥 连续学习：3天
        📅 明天：Day 4 — useEffect和自定义Hook
```

### Example 4: Rest Day

```
User: 今天太累了，休息一天

Claude: [Marks today's incomplete tasks as ⏸️, shifts dates]
        已记录休息日 📅
        Day 3 未完成的任务已顺延到明天。
        💤 好好休息，明天继续！
        🔥 连续学习记录保持不变：3天
```

### Example 5: Fuzzy Matching Ambiguity

```
User: 视频看完了

Claude: [Today has: 任务1 "观看JSX教学视频(30min)" + 任务2 "观看组件生命周期视频(45min)"]
        你指的是哪一个？
        1) JSX教学视频（任务1）
        2) 组件生命周期视频（任务2）
```
