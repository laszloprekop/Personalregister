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
            Console.WriteLine("★ ★ ★  Group ticket price  ★ ★ ★");
            Console.WriteLine();
            int groupSize = ReadPositiveWholeNumber("How many people? ");
            int totalPrice = 0;

            for (int i = 1; i <= groupSize; i++)
            {
                totalPrice += GetTicketPrice(ReadPositiveWholeNumber($"Age of person {i}: "));
            }

            Console.WriteLine();
            Console.WriteLine($"Group size:            {groupSize}");
            Console.WriteLine($"Total price for group: {totalPrice} kr");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            break;
        case 3:
            Console.WriteLine("(Not yet implemented.)");
            Console.ReadKey();
            break;
        case 4:
            Console.WriteLine("(Not yet implemented.)");
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