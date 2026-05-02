# PRD: Personalregister — Employee Register Console Application

## Overview

A console application for a small restaurant company to manage an employee register. Employees can be added, viewed, edited, and soft-deleted via an interactive console UI. Data is in-memory only; no persistence is required.

---

## Goals

- Allow staff to maintain a list of employees with name and salary
- Provide a simple, navigable console interface
- Be robust and future-proof through clean separation of concerns and validated input

---

## Out of Scope

- Persistent storage (database, file)
- Authentication or access control
- Operation history or undo/redo
- Reporting or export

---

## Classes

### `Employee`

Represents a single employee.

| Member | Type | Notes |
|---|---|---|
| `Name` | `string` | Mutable; rejects null or whitespace; trimmed on set |
| `Salary` | `decimal` | Mutable; must be ≥ 0 |
| `IsDeleted` | `bool` | Default `false`; set to `true` on soft-delete |
| `ToString()` | override | Formats as `{Name} - {Salary:C}` |

Constructor accepts `name` and `salary`, validates both.

### `EmployeeRegister`

Owns and manages the collection of employees.

| Method | Notes |
|---|---|
| `Add(Employee)` | Appends to internal list |
| `GetActive()` | Returns employees where `IsDeleted == false` |
| `GetByIndex(int)` | 1-based index into active list; throws on out-of-range |
| `Update(Employee, string, decimal)` | Validates and applies name + salary edits in-place |
| `SoftDelete(Employee)` | Sets `IsDeleted = true` after confirmation |

Internal list is private; not exposed directly.

### `Program`

Entry point. Owns the console I/O loop.

---

## User Interface

### List View (main screen)

Displayed after every operation. Shows:

```
=== Employee Register ===

1. Jane Doe - $52,000.00
2. John Smith - $48,000.00

[A] Add employee
[Q] Quit
Enter number to select employee:
```

If the register is empty:

```
=== Employee Register ===

(No employees registered)

[A] Add employee
[Q] Quit
```

### Add Employee

Prompts for name, then salary. Validates each field. On success, returns to list view with the new employee included.

### Select Employee

User enters the list number. Invalid input shows an error and re-prompts.

On valid selection, shows:

```
Selected: Jane Doe - $52,000.00

[E] Edit
[D] Delete
[B] Back
```

### Edit Employee

Prompts for each field, pre-filled with the current value. Pressing Enter keeps the existing value.

```
Name [Jane Doe]: 
Salary [52000]: 
```

On success, returns to list view with updated data.

### Delete Employee

Confirms before soft-deleting:

```
Are you sure you want to delete Jane Doe? (y/n):
```

On confirmation, sets `IsDeleted = true`. Employee no longer appears in the list view. Returns to list view.

---

## Validation Rules

| Field | Rule |
|---|---|
| Name | Not null, not whitespace; trimmed before storing |
| Salary | Decimal ≥ 0; reject non-numeric input |

Invalid input displays a clear error message and re-prompts; it never crashes the application.

---

## Non-Functional Requirements

- All input validated at the console boundary; domain classes throw `ArgumentException` on invalid data
- No raw list exposure from `EmployeeRegister`; encapsulated behind methods
- Soft-delete preserves employee data in memory for the session lifetime
