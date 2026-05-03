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
            Console.WriteLine($"Ticket price: {price} kr");

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
            int groupSize = ReadPositiveWholeNumber("How many people? ");
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

            Console.WriteLine();
            Console.WriteLine("Person".PadRight(12) + "Age".PadRight(12) + "Price".PadLeft(12));
            Console.WriteLine(new string('-', 36));
            for (int i = 0; i < groupSize; i++)
            {
                Console.WriteLine($"{i + 1}".PadRight(12) + $"{ages[i]}".PadRight(12) + $"{prices[i]} kr".PadLeft(12));
            }

            Console.WriteLine(new string('-', 36));
            Console.WriteLine($"Group size: {groupSize}".PadRight(24) + $"Total: {totalPrice} kr".PadLeft(12));
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
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine($"{i}. {text}");
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
    if (age < 20) return 80;
    if (age > 64) return 90;
    return 120;
}

// Loops until the user enter a positive whole number
static int ReadPositiveWholeNumber(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
            return value;
        Console.WriteLine("Please enter a positive whole number.");
    }
}