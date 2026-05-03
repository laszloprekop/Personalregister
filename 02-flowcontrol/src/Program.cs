using System.Diagnostics;

bool running = true;

const ConsoleColor titleColor = ConsoleColor.Cyan;
const ConsoleColor messageColor = ConsoleColor.White;
const ConsoleColor errorColor = ConsoleColor.Red;
const ConsoleColor successColor = ConsoleColor.Blue;
const ConsoleColor borderColor = ConsoleColor.DarkYellow;

while (running)
{
    Console.Clear();


    WriteLineColored("★ ★ ★  Flow Control Exercises  ★ ★ ★ ", titleColor);
    Console.WriteLine();
    Console.WriteLine("[1] Single ticket price");
    Console.WriteLine("[2] Group ticket price");
    Console.WriteLine("[3] Repeat text");
    Console.WriteLine("[4] Third word");
    Console.WriteLine("[0] Quit");
    Console.WriteLine();
    WriteLineColored("Choose an option: ", messageColor);


    if (!int.TryParse(Console.ReadLine(), out int choice))
        choice = -1;

    switch (choice)
    {
        case 0:
            running = false;
            break;
        case 1:
            Console.Clear();
            Console.WriteLine("★ ★ ★  Single ticket price  ★ ★ ★ ");
            Console.WriteLine();
            int age = ReadPositiveWholeNumber("Enter age: ");
            int price = GetTicketPrice(age);
            string category = GetPriceCategory(age);
            string priceDisplay = price == 0 ? "Free" : $"{price} kr ({category})";
            WriteColored($"Ticket price: {priceDisplay}", successColor);

            Console.WriteLine();
            WriteLineColored("Press any key to continue...", messageColor);
            Console.ReadKey();
            break;
        case 2:
            Console.Clear();
            Console.WriteLine();
            WriteLineColored("★ ★ ★  Group ticket price  ★ ★ ★ ", titleColor);
            Console.WriteLine();
/*            int groupSize = ReadPositiveWholeNumber("How many people? ");
            int totalPrice = 0;

            for (int i = 1; i <= groupSize; i++)
            {
                totalPrice += GetTicketPrice(ReadPositiveWholeNumber($"Age of person {i}: "));
            }

            Console.WriteLine();
            Console.WriteLine($"Group size:            {groupSize}");
            Console.WriteLine($"Total price for group: {totalPrice} kr");
*/

            // Alternative solution using arrays and a quazi-table structure
            const string top = "╭────────────┬────────────┬────────────────┬──────────╮";
            const string mid = "├────────────┼────────────┼────────────────┼──────────┤";
            const string mid21 = "├────────────┴────────────┼────────────────┴──────────┤";
            const string bottom = "╰─────────────────────────┴───────────────────────────╯";

            int groupSize = ReadPositiveWholeNumber("How many people? ", min: 2);
            int totalPrice = 0;
            int[] ages = new int[groupSize];
            int[] prices = new int[groupSize];

            for (int i = 0; i < groupSize; i++)
            {
                Console.Write($"Age of person {i + 1}: ");
                ages[i] = ReadPositiveWholeNumber("");
                prices[i] = GetTicketPrice(ages[i]);
                totalPrice += prices[i];
            }

            // Table header
            Console.WriteLine();
            WriteLineColored(top, borderColor);
            WriteTableRow(borderColor,
                $" {"Person",-10} ",
                $" {"Age",-10} ",
                $" {"Category",-14} ",
                $" {"Price",8} "
            );

            // Table body
            WriteLineColored(mid, borderColor);
            for (int i = 0; i < groupSize; i++)
            {
                string discount = GetPriceCategory(ages[i]);
                string priceCell = prices[i] == 0 ? "-" : prices[i] + " kr";
                WriteTableRow(borderColor,
                    $" {(i + 1),10} ",
                    $" {ages[i],10} ",
                    $" {discount,14} ",
                    $" {priceCell,8} "
                );
            }

            // Table footer
            WriteLineColored(mid21, borderColor);
            WriteTableRow(borderColor,
                $" {"Group size: " + groupSize,-23} ",
                $" {"Total: " + totalPrice + " kr",25} "
            );
            WriteLineColored(bottom, borderColor);
            Console.WriteLine();
            WriteLineColored("Press any key to continue...", messageColor);
            Console.ReadKey();
            break;
        case 3:
            Console.WriteLine();
            WriteLineColored("★ ★ ★  Repeat text  ★ ★ ★ ", titleColor);
            Console.WriteLine();
            Console.WriteLine("Enter text to repeat ten times: ");
            string text = Console.ReadLine() ?? string.Empty;
            int times = ReadPositiveIntWithDefault("Number of times to repeat (default 10)", 10);
            for (int i = 1; i <= times; i++)
            {
                WriteColored($"{i}. {text}", successColor);
                if (i < times) Console.Write(", ");
            }

            Console.WriteLine();
            WriteLineColored("Press any key to continue...", messageColor);
            Console.ReadKey();
            break;
        case 4:
            WriteLineColored("★ ★ ★  Extract third word  ★ ★ ★ ", titleColor);
            Console.WriteLine();
            Console.Write("Enter a sentence (at least 3 words): ");
            string sentence = Console.ReadLine() ?? string.Empty;
            string[] words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 3)
            {
                WriteLineColored("Error: Sentence must have at least 3 words.", errorColor);
                Console.ReadKey();
                break;
            }

            Console.WriteLine($"The third word is: {words[2]}");
            Console.WriteLine();
            WriteLineColored("Press any key to continue...", messageColor);
            Console.ReadKey();
            break;
        default:
            WriteLineColored("Invalid option. Please try again.", errorColor);
            Console.ReadKey();
            break;
    }
}

// Returns ticket price based on age
static int GetTicketPrice(int age)
{
    return age switch
    {
        <= 5 or >= 100 => 0,
        < 20 => 80,
        > 64 => 90,
        _ => 120
    };
}

// Loops until the user enter a positive whole number
int ReadPositiveWholeNumber(string prompt, int min = 1)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int value) && value >= min)
            return value;
        WriteLineColored($"Please enter a whole number greater than {min}.", errorColor);
    }
}

// Returns category label with discount vs. Standard
static string GetPriceCategory(int age)
{
    return age switch
    {
        <= 5 or >= 100 => "Free",
        < 20 => "Youth (-33%)",
        > 64 => "Senior (-25%)",
        _ => "Standard"
    };
}

// Helper method to read a positive integer with a default value
int ReadPositiveIntWithDefault(string prompt, int defaultValue)
{
    while (true)
    {
        Console.Write($"{prompt}: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) return defaultValue;
        if (int.TryParse(input, out var value) && value > 0) return value;
        WriteLineColored("Please enter a positive whole number.", errorColor);
    }
}

// Two utility helpers to add coloring to the console app

static void WriteColored(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.Write(text);
    Console.ResetColor();
}

static void WriteLineColored(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}

static void WriteTableRow(ConsoleColor borderColor, params string[] cells)
{
    foreach (var cell in cells)
    {
        WriteColored($"│", borderColor);
        Console.Write(cell);
    }

    WriteColored("│", borderColor);
    Console.WriteLine();
}