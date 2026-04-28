namespace Personalregister;

public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double Salary { get; set; }
    public bool IsDeleted { get; set; }

    public Employee(string firstName, string lastName, double salary, bool isDeleted = false)
    {
        FirstName = firstName;
        LastName = lastName;
        Salary = salary;
        IsDeleted = isDeleted;
    }
}