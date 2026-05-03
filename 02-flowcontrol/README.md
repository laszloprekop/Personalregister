# Flow Control

A console application demonstrating C# flow control fundamentals: loops, switch-case, nested if-else, and string manipulation.

## Features

- Menu-driven interface with an infinite loop and clean exit
- Cinema ticket pricing by age (youth / standard / pensioner)
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

Prices are determined by age: youth (under 20) · standard (20–64) · pensioner (over 64).

```
★ ★ ★  Single ticket price  ★ ★ ★ 

Enter age: 17
Ticket price: 80 kr
```

```
Enter age: 34
Ticket price: 120 kr
```

```
Enter age: 71
Ticket price: 90 kr
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

╭────────────┬────────────┬──────────────────╮
│ Person     │ Age        │            Price │
├────────────┼────────────┼──────────────────┤
│ 1          │ 9          │            80 kr │
│ 2          │ 24         │           120 kr │
│ 3          │ 64         │           120 kr │
│ 4          │ 82         │            90 kr │
├────────────┴────────────┼──────────────────┤
│ Group size: 4           │    Total: 410 kr │
╰─────────────────────────┴──────────────────╯

Press any key to continue...
```

---

### [3] Repeat text

Prints the entered text ten times using a `for` loop.

```
★ ★ ★  Repeat text  ★ ★ ★ 

Enter text to repeat ten times: hello
1. hello
2. hello
3. hello
4. hello
5. hello
6. hello
7. hello
8. hello
9. hello
10. hello

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
- Data is not persisted between runs
