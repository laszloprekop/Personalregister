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
        AddEmployeeFlow(register);

    else
    {
        var index = int.Parse(input);
        var employee = register.GetByIdNumber(index);
        Console.WriteLine($"\nSelected: {employee}");
        Console.WriteLine();
        Console.WriteLine("[E] Edit");
        Console.WriteLine("[D] Delete");
        Console.WriteLine("[B] Back");
        Console.Write("Choice: ");
        var action = Console.ReadLine()?.Trim().ToUpper();

        if (action == "E")
            EditFlow(register, employee);
        else if (action == "D")
            DeleteFlow(register, employee);
    }
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

static void AddEmployeeFlow(EmployeeRegister register)
{
    var firstName = ReadNonEmptyString("First name: ");
    var lastName = ReadNonEmptyString("Last name: ");
    var salary = ReadDouble("Salary: ", 0);
    register.AddEmployee(new Employee(firstName, lastName, salary));
}


static void DeleteFlow(EmployeeRegister register, Employee employee)
{
    Console.Write($"Are you sure you want to delete {employee.FullName}? (y/n): ");
    var confirm = Console.ReadLine()?.Trim().ToLower();
    if (confirm == "y")
        register.SoftDeleteEmployee(employee);
}

static void EditFlow(EmployeeRegister register, Employee employee)
{
    var firstName = ReadNonEmptyString($"First name [{employee.FirstName}]: ");
    var lastName = ReadNonEmptyString($"Last name [{employee.LastName}]: ");
    var salary = ReadDouble($"Salary [{employee.Salary}]: ", 0);
    register.UpdateEmployee(employee, firstName, lastName, salary);
}