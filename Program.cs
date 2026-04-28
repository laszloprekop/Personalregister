using Personalregister;

var register = new EmployeeRegister();
register.AddEmployee(new Employee("Sven", "Svensson", 52000.00));
register.AddEmployee(new Employee("Lars", "Larsson", 48000.00));


while (true)
{
    PrintListView(register);
    var input = Console.ReadLine().Trim().ToUpper();

    if (input == "Q")
    {
        break;
    }
    else if (input == "A")
    {
        Console.Write("First name: ");
        var firstName = ReadNonEmptyString("First name: ");

        Console.Write("Last name: ");
        var lastName = ReadNonEmptyString("Last name: ");

        Console.Write("Salary: ");
        var salary = ReadDouble("Salary: ", 0);

        register.AddEmployee(new Employee(firstName, lastName, salary));
    }
    else
        Console.WriteLine("(not implemented yet)");
}

static void PrintListView(EmployeeRegister register)
{
    Console.WriteLine("=== Employee Register ===");
    Console.WriteLine();

    var active = register.GetActiveEmployees();
    if (active.Count == 0)
    {
        Console.WriteLine("(No employees registered)");
    }
    else
    {
        foreach (var (employee, i) in active.Select((e, i) => (e, i + 1)))
        {
            Console.WriteLine($"{i}. {employee}");
        }
    }

    Console.WriteLine();
    Console.WriteLine("[A] Add employee");
    Console.WriteLine("[Q] Quit");
    Console.Write("Enter number to select employee: ");
}

static string ReadNonEmptyString(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var value = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(value))
            return value;
        Console.WriteLine("Input cannot be empty. Try again.");
    }
}

static double ReadDouble(string prompt, double min)
{
    while (true)
    {
        Console.Write(prompt);
        if (double.TryParse(Console.ReadLine(), out var value) && value >= min)
            return value;
        Console.WriteLine($"Please enter a valid number greater than or equal to {min}.");
    }
}