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
}