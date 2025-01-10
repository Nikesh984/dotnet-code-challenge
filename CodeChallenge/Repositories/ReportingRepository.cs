using System;
using Castle.Core.Logging;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Repositories
{
    public class ReportingRepository : IReportingRepository
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<IReportingRepository> _logger;

        public ReportingRepository(IEmployeeRepository employeeRepository, ILogger<IReportingRepository> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public ReportingStructure GetReportingStructure(string employeeId)
        {
            var employee = _employeeRepository.GetById(employeeId);
            if (employee == null)
            {
                return null;
            }

            var reportingStructure = new ReportingStructure
            {
                Employee = employee,
                Reportees = CountReportees(employee)
            };

            return reportingStructure;
        }

        private int CountReportees(Employee employee)
        {
            int count = 0;
            foreach (var directReports in employee.DirectReports)
            {
                count += 1 + CountReportees(directReports);
            }

            return count;
        }
    }
}