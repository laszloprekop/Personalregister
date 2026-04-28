namespace Personalregister;

public class EmployeeRegister
{
    private List<Employee> _employees = new List<Employee>();

    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
    }

    public IReadOnlyList<Employee> GetActiveEmployees()
    {
        return _employees.Where(e => !e.IsDeleted).ToList();
    }

    public Employee GetByIdNumber(int idNumber)
    {
        var active = GetActiveEmployees();
        if (idNumber > active.Count || idNumber < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idNumber), $"No employee with this number: {idNumber}");
        }

        return active[idNumber - 1];
    }

    public void SoftDeleteEmployee(Employee employee)
    {
        employee.IsDeleted = true;
    }
    
    public void UpdateEmployee(Employee employee, string firstName, string lastName, double salary)
    {
        employee.FirstName = firstName;
        employee.LastName = lastName;
        employee.Salary = salary;
    }
}