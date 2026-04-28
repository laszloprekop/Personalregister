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
        return active[idNumber -1];
    }
}