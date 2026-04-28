namespace Personalregister;

public class EmployeeRegister
{
    public List<Employee> Employees = new List<Employee>();
    
    public void AddEmployee(Employee employee)
    {
        Employees.Add(employee);
    }
}