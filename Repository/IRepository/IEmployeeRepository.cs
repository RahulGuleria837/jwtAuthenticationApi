using JWTAuthentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Repository
{
    public interface IEmployeeRepository
    {

        ICollection<Employee> GetEmployees();
        Employee GetEmployee(int employeeid);  //find
        bool CreateEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);

        bool Save();
    }
}
