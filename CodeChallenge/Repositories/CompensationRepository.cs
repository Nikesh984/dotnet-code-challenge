using System;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Castle.Core.Logging;
using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(EmployeeContext employeeContext, ILogger<ICompensationRepository> logger, IEmployeeRepository employeeRepository)
        {
            _employeeContext = employeeContext;
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public Compensation Create(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            var employee = _employeeRepository.GetById(compensation.Employee.EmployeeId);
            if (employee != null)
            {
                compensation.Employee = employee;
                _employeeContext.Compensations.Add(compensation);
                _employeeContext.SaveChangesAsync().Wait();
                return compensation;
            }
            return null;
        }

        public Compensation GetCompensationByEmployeeId(string id)
        {
            var compensation = _employeeContext.Compensations
                                .Include(e => e.Employee)
                                .ThenInclude(e => e.DirectReports)
                                .FirstOrDefault(e => e.Employee.EmployeeId == id);

            if (compensation != null)
            {
                LoadDirectReportsRecursively(compensation.Employee); ;
            }
            return compensation;
        }


        // Eager loading
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

        public Compensation UpdateCompensation(Compensation compensation)
        {
            var existingCompensation = _employeeContext.Compensations
                                               .Include(e => e.Employee)  // Ensures Employee is loaded
                                               .FirstOrDefault(e => e.Employee.EmployeeId == compensation.Employee.EmployeeId);

            if (existingCompensation == null)
            {
                _logger.LogWarning($"Compensation not found for employee with ID: {compensation.Employee.EmployeeId}");

                return null;
            }

            // Updates salary
            existingCompensation.Salary = compensation.Salary;

            _employeeContext.SaveChangesAsync().Wait();

            return existingCompensation;

        }
    }
}