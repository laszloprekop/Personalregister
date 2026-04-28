namespace Personalregister;

public class Employee
{
    private string _firstName;

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("First name cannot be empty", nameof(value));
            _firstName = value.Trim();
        }
    }

    public string LastName { get; set; }
    public double Salary { get; set; }
    public bool IsDeleted { get; set; }
    public string FullName => $"{FirstName} {LastName}";

    public override string ToString()
    {
        return $"{FullName} - {Salary:C}";
    }

    public Employee(string firstName, string lastName, double salary, bool isDeleted = false)
    {
        FirstName = firstName;
        LastName = lastName;
        Salary = salary;
        IsDeleted = isDeleted;
    }
}