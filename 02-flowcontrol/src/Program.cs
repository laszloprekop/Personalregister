bool running = true;

while (running)
{
    Console.WriteLine("★★★ Flow Control practices ★★★");
    Console.WriteLine();
    Console.WriteLine("[1] Single ticket price");
    Console.WriteLine("[2] Group ticket price");
    Console.WriteLine("[3] Repeat text");
    Console.WriteLine("[4] Third word");
    Console.WriteLine("[0] Quit");
    Console.WriteLine();
    Console.Write("Choose an option: ");


    int choice = int.Parse(Console.ReadLine());

    switch (choice)
    {
        case 0:
            running = false;
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}