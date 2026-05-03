using System.Diagnostics;

bool running = true;

while (running)
{
    Console.Clear();
    Console.WriteLine("★ ★ ★  Flow Control Exercises  ★ ★ ★ ");
    Console.WriteLine();
    Console.WriteLine("[1] Single ticket price");
    Console.WriteLine("[2] Group ticket price");
    Console.WriteLine("[3] Repeat text");
    Console.WriteLine("[4] Third word");
    Console.WriteLine("[0] Quit");
    Console.WriteLine();
    Console.Write("Choose an option: ");


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
            Console.WriteLine($"Ticket price: {priceDisplay}");

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            break;
        case 2:
            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine("★ ★ ★  Group ticket price  ★ ★ ★ ");
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
            Console.WriteLine(top);
            Console.WriteLine($"│ {"Person",-10} │ {"Age",-10} │ {"Discount",-14} │ {"Price",8} │");

            // Table body
            Console.WriteLine(mid);
            for (int i = 0; i < groupSize; i++)
            {
                string discount = GetPriceCategory(ages[i]);
                string priceCell = prices[i] == 0 ? "-" : prices[i] + " kr";
                Console.WriteLine($"│ {(i + 1),-10} │ {ages[i],-10} │ {discount,-14} │ {priceCell,8} │");
            }

            // Table footer
            Console.WriteLine(mid21);
            Console.WriteLine($"│ {"Group size: " + groupSize,-23} │ {$"Total: {totalPrice} kr",25} │");
            Console.WriteLine(bottom);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            break;
        case 3:
            Console.WriteLine();
            Console.WriteLine("★ ★ ★  Repeat text  ★ ★ ★ ");
            Console.WriteLine();
            Console.Write("Enter text to repeat ten times: ");
            string text = Console.ReadLine() ?? string.Empty;
            int times = ReadPositiveIntWithDefault("Number of times to repeat (default 10)", 10);
            for (int i = 1; i <= times; i++)
            {
                Console.Write($"{i}. {text}");
                if (i < times) Console.Write(", ");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            break;
        case 4:
            Console.WriteLine("★ ★ ★  Extract third word  ★ ★ ★ ");
            Console.WriteLine();
            Console.Write("Enter a sentence (at least 3 words): ");
            string sentence = Console.ReadLine() ?? string.Empty;
            string[] words = sentence.Split(' ');
            if (words.Length < 3)
            {
                Console.WriteLine("Error: Sentence must have at least 3 words.");
                Console.ReadKey();
                break;
            }

            Console.WriteLine($"The third word is: {words[2]}");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
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
static int ReadPositiveWholeNumber(string prompt, int min = 1)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int value) && value >= min)
            return value;
        Console.WriteLine($"Please enter a whole number greater than {min}.");
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
static int ReadPositiveIntWithDefault(string prompt, int defaultValue)
{
    while (true)
    {
        Console.Write($"{prompt}: ");
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) return defaultValue;
        if (int.TryParse(input, out var value) && value > 0) return value;
        Console.WriteLine("Please enter a positive whole number.");
    }
}