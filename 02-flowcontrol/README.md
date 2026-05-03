# Flow Control

A console application demonstrating C# flow control fundamentals: loops, switch-case, nested if-else, and string manipulation.

## Features

- Menu-driven interface with an infinite loop and clean exit
- Cinema ticket pricing by age (youth / standard / pensioner / free)
- Group ticket calculator with a formatted summary table
- Text repetition via `for` loop
- Third-word extraction via `string.Split`

## Running the app

```
dotnet run --project src/FlowControl.csproj
```

## Screens

### Main menu

```
★ ★ ★  Flow Control Exercises  ★ ★ ★ 

[1] Single ticket price
[2] Group ticket price
[3] Repeat text
[4] Third word
[0] Quit

Choose an option:
```

---

### [1] Single ticket price

Prices are determined by age: free (≤ 5 or ≥ 100) · youth (6–19) · standard (20–64) · senior (65–99).

```
★ ★ ★  Single ticket price  ★ ★ ★ 

Enter age: 4
Ticket price: Free
```

```
Enter age: 17
Ticket price: 80 kr (Youth (-33%))
```

```
Enter age: 34
Ticket price: 120 kr (Standard)
```

```
Enter age: 71
Ticket price: 90 kr (Senior (-25%))
```

---

### [2] Group ticket price

Prompts for each person's age, then renders a summary table.

```
★ ★ ★  Group ticket price  ★ ★ ★ 

How many people? 4
Age of person 1: 9
Age of person 2: 24
Age of person 3: 64
Age of person 4: 82

╭────────────┬────────────┬────────────────┬──────────╮
│ Person     │ Age        │       Category │    Price │
├────────────┼────────────┼────────────────┼──────────┤
│          1 │          9 │   Youth (-33%) │    80 kr │
│          2 │         24 │       Standard │   120 kr │
│          3 │         64 │       Standard │   120 kr │
│          4 │         82 │  Senior (-25%) │    90 kr │
├────────────┴────────────┼────────────────┴──────────┤
│ Group size: 4           │             Total: 410 kr │
╰─────────────────────────┴───────────────────────────╯

Press any key to continue...
```

---

### [3] Repeat text

Prints the entered text N times (default 10) using a `for` loop, comma-separated on one line.

```
★ ★ ★  Repeat text  ★ ★ ★ 

Enter text to repeat ten times: 
hello
Number of times to repeat (default 10): 
1. hello, 2. hello, 3. hello, 4. hello, 5. hello, 6. hello, 7. hello, 8. hello, 9. hello, 10. hello

Press any key to continue...
```

---

### [4] Third word

Splits the sentence on spaces and prints the word at index 2.

```
★ ★ ★  Extract third word  ★ ★ ★ 

Enter a sentence (at least 3 words): The quick brown fox
The third word is: brown

Press any key to continue...
```

## Notes

- All numeric input is validated — invalid entries re-prompt rather than crash
- Children aged 5 or under and centenarians (100+) get free tickets
- Data is not persisted between runs
