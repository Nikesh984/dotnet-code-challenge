using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            //return _employeeContext.Employees.FirstOrDefault(e => e.EmployeeId == id);

            var employee = _employeeContext.Employees
                            .Include(e => e.DirectReports)
                            .FirstOrDefault(e => e.EmployeeId == id);

            if (employee != null)
            {
                LoadDirectReportsRecursively(employee);
            }

            return employee;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }


        //Helper method to load the direct reports of the direct reports for a given employee
        private void LoadDirectReportsRecursively(Employee employee)
        {
            foreach (var directReport in employee.DirectReports)
            {
                // Explicitly load the DirectReports of each DirectReport
                _employeeContext.Entry(directReport)
                    .Collection(e => e.DirectReports)
                    .Load();

                // Recursive call for deeper levels
                LoadDirectReportsRecursively(directReport);
            }
        }
    }
}
