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
            Console.WriteLine("★ ★ ★ 2 Single ticket price  ★ ★ ★ ");
            Console.WriteLine();
            Console.Write("Enter age: ");
            int age = int.Parse(Console.ReadLine());
            
            if (age < 20)
                Console.WriteLine("Youth Price: 80 kr");
            else if (age > 64)
                Console.WriteLine("Senior Price: 90 kr");
            else
                Console.WriteLine("Regular Price: 120 kr");
            
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            break;
        case 2:
            Console.WriteLine("(Not yet implemented.)");
            break;
        case 3:
            Console.WriteLine("(Not yet implemented.)");
            break;
        case 4:
            Console.WriteLine("(Not yet implemented.)");
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            Console.ReadKey();
            break;
    }
}