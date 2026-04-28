using Personalregister;

var register = new EmployeeRegister();
register.AddEmployee(new Employee("Sven", "Svensson", 52000.00));
register.AddEmployee(new Employee("Lars", "Larsson", 48000.00));


Console.WriteLine("=== Employee Register ===");
Console.WriteLine();

foreach (var (employee, index) in register.GetActiveEmployees().Select((e, i) => (e, i + 1)))
{
    Console.WriteLine($"{index + 1}. {employee}");
}

Console.WriteLine();
Console.WriteLine("[A] Add employee");
Console.WriteLine("[Q] Quit");
Console.Write("Enter number to select employee: ");