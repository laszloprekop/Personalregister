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

    private string _lastName;

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Last name cannot be empty.");
            _lastName = value.Trim();
        }
    }


    public double Salary
    {
        get => _salary;
        set
        {
            if (value < 0)
                throw new ArgumentException("Salary cannot be negative.");
            _salary = value;
        }
    }

    private double _salary;

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