bool running = true;

while (running)
{
    Console.Clear();
    Console.WriteLine("★★★ Flow Control Exercises ★★★");
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
        default:
            Console.WriteLine("Invalid option. Please try again.");
            Console.ReadKey();
            break;
    }
}