using JWTAuthentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_API_JWT_Angular_1.Identity;

namespace JWTAuthentication.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;

        }
        public bool CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            return Save();
        }

        public bool DeleteEmployee(Employee employee)
        {
            _context.Employees.Remove(employee);
            return Save();
        }

        public Employee GetEmployee(int employeeid)
        {
           return _context.Employees.Find(employeeid);
        }

        public ICollection<Employee> GetEmployees()
        {
           return _context.Employees.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges()==1 ? true : false; 
        }

        public bool UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            return Save();
        }
    }
}
