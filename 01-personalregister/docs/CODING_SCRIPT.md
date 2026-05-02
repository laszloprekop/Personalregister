# Coding Script: Personalregister

Step-by-step guide for progressive implementation. Each step is a single atomic commit on a feature branch.
Mistakes are intentional where marked — they mimic common patterns from other languages and are corrected in subsequent refactor steps.

**Pair coding:** code and commands are shown — the user types and runs everything.

---

## Branch overview

| Branch | Usable after merge? |
|---|---|
| `feature/employee-model` | No — foundational model only |
| `feature/employee-register` | No — logic layer, no UI yet |
| `feature/console-list-view` | **Yes** — app runs, shows hardcoded list |
| `feature/add-employee` | **Yes** — can add real employees |
| `feature/edit-delete-employee` | **Yes** — full CRUD in the console |
| `refactor/console-helpers` | Yes — same features, cleaner code |

---

## Branch: `feature/employee-model`

### Step 1.1 — Bare `Employee` class
Create `Employee.cs` with public fields for `FirstName`, `LastName`, and `Salary`.
- **Mistake:** use `double` for `Salary` (natural choice coming from Python/JS)
- **Mistake:** use public fields instead of properties

### Step 1.2 — Refactor: fields → properties
Convert to auto-properties with public getters and setters.
- Reason: C# convention; fields are not encapsulated

### Step 1.3 — Add constructor
Constructor accepts `firstName`, `lastName`, `salary` and assigns them.
- No validation yet

### Step 1.4 — Add `FullName` computed property
`public string FullName => $"{FirstName} {LastName}";`
- Read-only, derived — no setter

### Step 1.5 — Override `ToString()`
- **Mistake:** forget the `override` keyword — compiler warning will catch it
- Fix immediately; use `FullName` and format salary with `:C`

### Step 1.6 — Refactor: `double` → `decimal` for Salary
- Reason: monetary values need exact decimal representation
- Update property type and constructor signature

### Step 1.7 — Add `IsDeleted` property
`bool IsDeleted { get; set; }` defaulting to `false`.

### Step 1.8 — Add input validation
Validation in setters (not constructor):
- `FirstName` / `LastName`: reject null/whitespace, trim
- `Salary`: reject values < 0
- Throw `ArgumentException` with a clear message

---

## Branch: `feature/ne`

### Step 2.1 — Bare `EmployeeRegister` class
- **Mistake:** expose the list as `public List<Employee> Employees`

### Step 2.2 — Add `Add(Employee)` method
Straightforward append to the list.

### Step 2.3 — Refactor: encapsulate the list
Make `Employees` private. Add `GetActive()` returning `IReadOnlyList<Employee>` of non-deleted employees.
- Reason: callers shouldn't mutate the internal list directly

### Step 2.4 — Add `GetByIndex(int)`
1-based index into the active list. Throw `ArgumentOutOfRangeException` on invalid index.

### Step 2.5 — Add `SoftDelete(Employee)`
Set `IsDeleted = true` on the given employee.

### Step 2.6 — Add `Update(Employee, string, string, decimal)`
Apply new `firstName`, `lastName`, `salary` to an existing employee. Validation happens in setters.

---

## Branch: `feature/console-list-view`

**Goal: first runnable state — app starts, shows a list, accepts Q to quit.**

### Step 3.1 — Hardcoded list view in `Program.cs`
Replace `Hello, World!` with static `Console.WriteLine` calls printing a fake menu.
- No logic yet

### Step 3.2 — Wire up `EmployeeRegister`, print real list
Instantiate register, add two hardcoded employees, print via `GetActive()`.
- **Mistake:** inline all logic in `Main` — will extract later

### Step 3.3 — Extract `PrintListView(EmployeeRegister)` method
Move display logic out of `Main`.

### Step 3.4 — Add input loop
Read input in a loop. Handle `A` (add — stub for now) and `Q` (quit). Unknown input reprints the view.

---

## Branch: `feature/add-employee`

**Goal: user can add real employees via console input.**

### Step 4.1 — Prompt for first name, last name, salary inline in `Main`
- **Mistake:** no input validation — `decimal.Parse` throws on bad input

### Step 4.2 — Refactor: extract `ReadNonEmptyString(string prompt)` helper
Re-prompts until valid non-empty input is given.

### Step 4.3 — Refactor: extract `ReadDecimal(string prompt, decimal min)` helper
Re-prompts until a valid decimal ≥ min is entered.

### Step 4.4 — Extract `AddEmployeeFlow(EmployeeRegister)` method
Encapsulate the full add flow.

---

## Branch: `feature/edit-delete-employee`

**Goal: full CRUD — select, edit, soft-delete with confirmation.**

### Step 5.1 — Handle numeric input in main loop
Parse entered number, call `GetByIndex`, show selection sub-menu.
- **Mistake:** use `int.Parse` directly — fix to `int.TryParse` after seeing it throw

### Step 5.2 — Add `DeleteFlow(EmployeeRegister, Employee)`
Ask for confirmation (y/n), call `SoftDelete` on confirm.

### Step 5.3 — Add `EditFlow(EmployeeRegister, Employee)`
Pre-fill current `FirstName`, `LastName`, `Salary`. Pressing Enter keeps existing value.
Call `Update` with new values.

### Step 5.4 — Wire selection sub-menu
Handle `E`, `D`, `B` in the selection screen.

---

## Branch: `refactor/console-helpers`

**Goal: cleaner code — same features, better structure.**

### Step 6.1 — Extract a static `ConsoleHelper` class
Move `ReadNonEmptyString`, `ReadDecimal`, and shared print utilities into it.
- Reason: `Program.cs` is getting long

### Step 6.2 — Review and tighten error messages
Ensure all invalid-input messages are clear and consistent.

### Step 6.3 — Final review pass
Remove dead code, check naming consistency with PRD, verify all edge cases are handled.
