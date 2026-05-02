# Walkthrough: Personalregister Implementation

This document traces the complete build of the Personalregister console app commit-by-commit, explaining the goal of each step, the key technique used, and why alternative approaches were deliberately avoided or deferred. A postmortem section at the end identifies what was left unfinished, known bugs, missing best practices, and risks.

---

## Background

The app is a Swedish-language-themed employee register for a small restaurant. The PRD calls for full CRUD — add, view, edit, soft-delete — via an interactive console loop, with validated input and clean separation between domain logic and UI. Data lives in memory only; no persistence is required.

The CODING_SCRIPT.md defines a deliberate pedagogy: certain beginner mistakes are introduced on purpose (public fields, raw `int.Parse`, `double` for money) and then corrected in follow-up commits, so that the refactor step has a real reason to exist. Each commit is intentionally atomic: one thing changes at a time.

---

## Phase 1 — `feature/employee-model`

### Commit `52d8829` — Add Employee model (with two intentional mistakes)

**Goal:** Get the class file on disk with the basic shape.

**What happened:** `Employee.cs` was created with public *fields* (not properties), `double` for `Salary`, and a nested class definition (`public class Employee { public class Employee { ... } }`). There was also a wrong field name: `public bool Email` instead of `public bool IsDeleted`.

**Key element:** Public mutable fields are the "Python/JS way" — natural for someone coming from a language where you don't distinguish fields from properties. The nested class is a simple structural mistake.

**Why not properties immediately?** The point is to create a real need for the very next commit. If the class is already correct, there is nothing to refactor.

---

### Commit `9fe2a5c` — Rename `Email` → `IsDeleted`

**Goal:** Correct the wrong field name before building further on top of it.

**Key element:** A pure rename — one character change in the field declaration. Keeps the diff minimal and the intent obvious.

---

### Commit `cc5f83a` — Add constructor

**Goal:** Make it possible to instantiate `Employee` with values in a single expression.

**Key element:** The constructor assigns all four fields: `firstName`, `lastName`, `salary`, and `isDeleted` (with default `false`). Validation is deliberately absent here — it will come later.

**Why no validation yet?** Adding validation before the setter infrastructure exists would mean validating in the constructor body, which leads to duplication once setters grow their own guards. Doing it after setters are in place (Step 1.8) means validation is in one canonical place.

---

### Commit `312b3f1` — Fields → properties, fix nested class, fix typo

**Goal:** Correct all three structural mistakes from `52d8829` in one refactor commit.

**Key element:** Auto-properties (`{ get; set; }`) replace bare public fields. The nested class is removed. The `issDeleted` typo in the constructor body is fixed.

**Why auto-properties?** C# convention. Fields bypass encapsulation — you cannot add validation logic to a field access, and they don't participate in data-binding or serialization the same way. Properties are the standard mechanism to expose state in a controlled way.

---

### Commit `3bdf597` — Add `FullName` computed property

**Goal:** Give callers a single expression for the display name instead of requiring them to concatenate `FirstName + " " + LastName` everywhere.

**Key element:** `public string FullName => $"{FirstName} {LastName}";` — a get-only expression-bodied property. No backing field, no setter. The value is always computed from the two source properties, so it is always consistent.

**Why not a method?** In C#, a method implies intent to do work; a property implies reading state. `FullName` is a derived view of existing state — it belongs as a property. A method call would be technically equivalent but semantically misleading.

---

### Commit `977f31f` — Add `ToString()` override (with intentional mistake)

**Goal:** Make `Console.WriteLine(employee)` produce a readable string automatically.

**What happened:** The method was written *without* the `override` keyword, i.e., `public string ToString()`. The compiler emits a warning (CS0114 / CS0108 depending on version) but does not error. The method shadows `object.ToString()` rather than overriding it.

**Key element:** The deliberate omission creates a teachable moment: the compiler warning is the discovery mechanism. Students learn that warnings are not just noise.

---

### Commit `305a2e7` — Fix missing `override` modifier

**Goal:** Fix the warning from the previous commit.

**Key element:** Adding `override` makes the method participate in virtual dispatch. Without it, a variable typed as `object` pointing to an `Employee` would call `object.ToString()` and print the fully-qualified type name instead of the employee's data.

---

### Commit `929a552` + `aa3e163` — Add input validation to setters

**Goal:** Enforce the PRD's validation rules at the domain boundary so that an `Employee` can never be in an invalid state after construction.

**Key element:** Both setters use the guard-and-throw pattern:

```csharp
set {
    if (string.IsNullOrWhiteSpace(value))
        throw new ArgumentException("...");
    _firstName = value.Trim();
}
```

Validation is in the setter, not the constructor. The constructor calls the setters implicitly by assigning to the properties, so there is no duplication.

**Why `ArgumentException`?** It is the standard .NET exception for invalid argument values. It signals a *programming error* at the call site, not a runtime condition to be caught and recovered — the caller should have validated before calling.

**Why not validate in the constructor?** Already addressed above. The property setter is the single authoritative place. Validation in the constructor body in addition to the setter would be redundant.

---

## Phase 2 — `feature/employee-register`

### Commit `2f107d7` — Bare `EmployeeRegister` class (with intentional mistake)

**Goal:** Create the collection container.

**What happened:** The internal list was exposed as a public field: `public List<Employee> Employees = new List<Employee>();`. This is the same mistake as Phase 1 with fields — intentional.

**Key element:** A bare public field on a collection means any caller can call `Employees.Add(...)`, `Employees.Clear()`, `Employees.RemoveAt(...)`, etc. The register class has no control over its own state.

---

### Commit `9670973` — Add `AddEmployee(Employee)`

**Goal:** Provide the intended mutation point.

**Key element:** `_employees.Add(employee)` inside a public method. Still using the public field at this point — the encapsulation fix comes next.

---

### Commits `2ef714c` + `c672d59` — Encapsulate the list

**Goal:** Remove external access to the raw list; expose a read-only view.

**Key element:**
- `Employees` field becomes `private List<Employee> _employees` (the `_` prefix follows C# private field convention).
- `GetActiveEmployees()` returns `IReadOnlyList<Employee>` — a filtered, read-only snapshot of non-deleted employees.

**Why `IReadOnlyList` instead of `List`?** The caller cannot call `Add`, `Remove`, or `Clear` on an `IReadOnlyList`. Returning `List<Employee>` would give callers a mutable reference to the internal collection through the return value, defeating the encapsulation. `IReadOnlyList` communicates intent clearly and prevents accidents.

**Why not `IEnumerable`?** `IEnumerable<T>` would also be read-only, but `IReadOnlyList<T>` additionally exposes `Count` and indexed access by position — both are used by callers (for displaying item numbers and for `GetByIdNumber`).

---

### Commit `1e3f6ea` — Add `GetByIdNumber(int)`

**Goal:** Let the UI resolve a 1-based display number to a concrete `Employee` object.

**Key element:** 1-based means `active[idNumber - 1]`. The method throws `ArgumentOutOfRangeException` on invalid input. Returns the active-list employee directly — callers hold a reference to the live object, so mutations to it (via `Update`) are reflected everywhere.

---

### Commit `1903ebc` — Add `SoftDeleteEmployee(Employee)`

**Goal:** Implement deletion without destroying data.

**Key element:** `employee.IsDeleted = true`. The employee remains in `_employees` but will no longer appear in `GetActiveEmployees()`. The session's data is preserved in memory for potential recovery, even though no recovery UI exists.

**Why soft-delete?** The PRD explicitly requires it: "data preserved for the session lifetime." Hard-delete would remove the object from the list, making recovery impossible without persistence.

---

### Commit `3675c18` — Add `UpdateEmployee(Employee, string, string, double)`

**Goal:** Apply edited values to an existing employee in-place.

**Key element:** Direct property assignment. Validation fires through the setters. The method does not need to find the employee in the list — the caller already holds a reference via `GetByIdNumber`.

---

## Phase 3 — `feature/console-list-view`

### Commit `a423c54` — Static placeholder UI

**Goal:** Replace `Hello, World!` with something that looks like the final app, even before any logic is wired.

**Why:** Shows the finished form immediately. The student can see the target layout before writing a single line of business logic. It also verifies the project builds and runs.

**Key element:** Pure `Console.WriteLine` calls with hardcoded Swedish names. No loops, no state, no classes.

---

### Commit `48a14db` — Wire up `EmployeeRegister`, print real list

**Goal:** First integration of domain layer with UI.

**Key element:** Instantiate `EmployeeRegister`, add two hardcoded `Employee` objects, call `GetActiveEmployees()`, and print via `foreach`. All logic is inline in `Main` — deliberately messy, because the next step will clean it up.

---

### Commit `489c45b` — Extract `PrintListView(EmployeeRegister)`

**Goal:** Move display logic out of `Main` into its own named method.

**Key element:** The LINQ idiom `active.Select((e, i) => (e, i + 1))` produces a tuple of `(employee, 1-based index)` in a single pass, avoiding a manual counter variable.

**Why extract now?** `Main` will grow significantly in the next phases. Extracting `PrintListView` early keeps `Main` readable as an orchestration loop, not a dump of interleaved display logic and control flow.

---

### Commit `ee03534` — Add input loop with placeholder responses

**Goal:** Make the app interactive — it stays alive and re-prompts after each action.

**Key element:** `while (true) { ... break on Q; }`. The Q/A branches stub out with `Console.WriteLine("... not implemented yet")` — the loop mechanics are proven before the logic is filled in.

---

## Phase 4 — `feature/add-employee`

### Commit `5023dc7` — Add employee via console input (with intentional mistake)

**Goal:** First real user-driven data entry.

**What happened:** `double.Parse(Console.ReadLine())` is used for salary. This throws `FormatException` if the user types anything non-numeric, crashing the loop.

**Key element:** The crash is the lesson. `double.Parse` is the "obvious" first attempt from any language background. The fix (extraction of a validated helper) follows immediately.

---

### Commit `c0bbe0b` — Extract `ReadNonEmptyString(string prompt)`

**Goal:** Create a reusable input helper that loops until the user provides non-blank input.

**Key element:** `while (true)` with `if (!string.IsNullOrWhiteSpace(value)) return value;`. The method never returns an empty string — the guarantee is enforced at the call site, not in the domain class.

**Why not validate in the domain class only?** Domain validation throws exceptions. Catching exceptions in an input loop is unidiomatic and hides the intent. The console layer should be the first line of defense; the domain layer is the safety net.

---

### Commit `695cc9e` — Extract `ReadDouble(string prompt, double min)`

**Goal:** Replace `double.Parse` with `double.TryParse` inside a re-prompting loop.

**Key element:** `double.TryParse(Console.ReadLine(), out var value) && value >= min`. The `out var` syntax keeps the check and assignment in one expression. `TryParse` returns `false` instead of throwing on bad input.

**Why `double` and not `decimal`?** This is a known gap — the CODING_SCRIPT planned a `double` → `decimal` migration (Step 1.6) that was never executed. See the postmortem section.

---

### Commit `cdd9eb9` — Extract `AddEmployeeFlow(EmployeeRegister)`

**Goal:** Move the add logic out of the main loop body.

**Key element:** The extracted method calls `ReadNonEmptyString` twice and `ReadDouble` once, then creates and adds the employee. The main loop becomes a single `AddEmployeeFlow(register)` call.

---

## Phase 5 — `feature/edit-delete-employee`

### Commit `e98132c` — Implement employee selection and stub action menu (with intentional mistake)

**Goal:** Allow the user to type a number to select an employee, show a sub-menu.

**What happened:** `int.Parse(input)` is used directly. If the user types something other than `Q`, `A`, or a valid integer, the application throws `FormatException` and crashes. The sub-menu reads a choice but ignores it (`Console.ReadLine()` result discarded).

**Key element:** The commit is purely additive — the structure is established (select → show menu → do nothing yet) before the logic is filled in.

---

### Commit `63b1f94` — Add `DeleteFlow(EmployeeRegister, Employee)`

**Goal:** Implement soft-deletion with a yes/no confirmation gate.

**Key element:** Checks `confirm == "y"` (lowercased). Any other input — including `"Y"` before `.ToLower()` was applied — is treated as cancellation. This is a common defensive pattern: require an explicit affirmative rather than treating anything non-negative as yes.

---

### Commit `aa6ce92` — Add `EditFlow(EmployeeRegister, Employee)`

**Goal:** Allow in-place editing of an existing employee.

**Key element:** The prompt displays the current value in brackets — e.g., `First name [Sven]:` — as a visual hint. However, pressing Enter does **not** keep the existing value; `ReadNonEmptyString` will re-prompt until actual text is entered. This is a known deviation from the PRD (see postmortem).

---

### Commit `516189f` — Wire up action sub-menu

**Goal:** Connect `E` and `D` branches to their respective flow methods.

**Key element:** The `Console.ReadLine()` that was previously discarded now captures `action`, and `if (action == "E")` / `else if (action == "D")` dispatch to the flows. The `B` branch is not wired — pressing Back silently falls through and the main loop re-renders the list.

---

## Postmortem

### Unfinished steps from CODING_SCRIPT

| Planned step | Status | Impact |
|---|---|---|
| Step 1.6 — `double` → `decimal` for `Salary` | **Not done** | All monetary values use IEEE 754 floating point; `52000.0 + 100.0` may not equal `52100.0` exactly. `decimal` gives exact base-10 arithmetic, which is mandatory for money. |
| Step 5.1 — fix `int.Parse` → `int.TryParse` | **Not done** | Any non-numeric input that isn't `Q` or `A` crashes the app with an unhandled `FormatException`. Reproducible by typing a letter other than A or Q at the main prompt. |
| Phase 6 — `refactor/console-helpers` | **Not done** | `Program.cs` holds all static methods. A `ConsoleHelper` class was planned but never extracted. |

---

### Bugs

**`int.Parse` crash at main input loop** (`Program.cs:22`)
```csharp
var index = int.Parse(input);  // throws FormatException on non-numeric
```
Type `"foo"` at the main prompt and the app exits ungracefully. Fix: `int.TryParse(input, out var index)` inside the else branch.

**`GetByIdNumber` off-by-one in bounds check** (`EmployeeRegister.cs:22`)
```csharp
if (idNumber > active.Count || idNumber < 0)
```
The check should be `idNumber < 1`. Passing `0` satisfies neither condition, so the method falls through to `active[0 - 1]` = `active[-1]`, which throws `ArgumentOutOfRangeException` from the list — a different exception than the one the method itself would throw, with a confusing message.

**`EditFlow` ignores the PRD's "press Enter to keep current value" requirement** (`Program.cs:103–108`)
The prompt shows `[current value]` as a hint, but `ReadNonEmptyString` treats an empty response as invalid and re-prompts. Pressing Enter loops forever rather than accepting the existing value. Fix: the edit variant needs a separate helper, e.g. `ReadNonEmptyStringOrDefault(string prompt, string defaultValue)`, that returns `defaultValue` when the user enters nothing.

**`Console.ReadLine()` without null check** (`Program.cs:11`)
```csharp
var input = Console.ReadLine().Trim().ToUpper();
```
`Console.ReadLine()` returns `null` when stdin is closed (e.g., piped input reaches EOF). Calling `.Trim()` on `null` throws `NullReferenceException`. Fix: use `Console.ReadLine()?.Trim() ?? ""`.

---

### Deviations from PRD

| PRD spec | Actual implementation |
|---|---|
| Single `Name` property; constructor takes `(name, salary)` | Two properties `FirstName` + `LastName`; constructor takes `(firstName, lastName, salary)` |
| Method named `Add`, `GetActive`, `GetByIndex`, `Update`, `SoftDelete` | Methods named `AddEmployee`, `GetActiveEmployees`, `GetByIdNumber`, `UpdateEmployee`, `SoftDeleteEmployee` |
| `[B] Back` is a handled action | `B` silently falls through — no explicit branch |
| `ToString` formats as `{Name} - {Salary:C}` | Formats as `{FullName} - {Salary:C}` (equivalent in intent, but name structure differs from PRD) |

The `FirstName`/`LastName` split is a reasonable and arguably better design than a single `Name` field, but it means the `Update` signature doesn't match the PRD, and the display logic uses `FullName` rather than `Name`.

---

### Missing best practices

**No tests.** The entire codebase has zero test files. The domain classes (`Employee`, `EmployeeRegister`) are pure C# with no I/O dependencies — they are trivially unit-testable. The absence of tests means the off-by-one bug in `GetByIdNumber` and the `int.Parse` crash can only be discovered by running the app manually.

**Validation error messages are inconsistent.** `FirstName` setter uses `nameof(value)` as the parameter name in `ArgumentException`, while `LastName` and `Salary` do not:
```csharp
// FirstName:
throw new ArgumentException("First name cannot be empty", nameof(value));
// LastName:
throw new ArgumentException("Last name cannot be empty.");
// Salary:
throw new ArgumentException("Salary cannot be negative.");
```
This inconsistency doesn't cause bugs but would confuse anyone catching these exceptions programmatically.

**`IsDeleted` is publicly settable.** `public bool IsDeleted { get; set; }` means any code can set `employee.IsDeleted = false` and resurrect a deleted employee without going through `EmployeeRegister`. The setter should be `private set` (or `init`-only), with `SoftDeleteEmployee` as the sole mutation point.

**No `gitignore` for `bin/` and `obj/`.** The generated `bin/` and `obj/` directories appear in the file tree alongside source files. The `.gitignore` file (added in the initial scaffold commit) is the standard Visual Studio one, which should cover these — but confirming they are not tracked would be prudent.

---

### Risks

**Floating-point money representation.** `Salary` typed as `double` is the highest-risk issue in the domain layer. If the app were extended to sum salaries across employees, floating-point rounding errors would accumulate. Changing `double` to `decimal` now requires updating `Employee.cs`, `EmployeeRegister.UpdateEmployee`, `Program.ReadDouble` (rename to `ReadDecimal`), and all call sites — a manageable but multi-file change.

**No input sanitisation beyond empty-check.** An employee can be given a name of `"   A   "` (multiple spaces and a single letter) — the trim normalises leading/trailing whitespace but interior whitespace is unconstrained. Not a security issue in an in-memory console app, but worth noting if data ever flows into a storage backend.

**Global state via top-level statements.** `Program.cs` uses C# top-level statements and module-level static methods. All static methods share the same file scope. As the file grows, there is no namespacing or class boundary to stop any method from calling any other method. The planned `ConsoleHelper` extraction (Phase 6) would address this structurally.

**No cancellation on long input loops.** `ReadNonEmptyString` and `ReadDouble` loop forever until valid input is provided. There is no escape hatch — the user cannot press Ctrl+C gracefully (it aborts the process entirely) or type a special keyword to cancel back to the main menu. This is acceptable for a learning project but would be a usability defect in a production console tool.
