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
        var firstName = Console.ReadLine();

        Console.Write("Last name: ");
        var lastName = Console.ReadLine();

        Console.Write("Salary: ");
        var salary = double.Parse(Console.ReadLine());

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