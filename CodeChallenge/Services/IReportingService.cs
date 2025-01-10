using CodeChallenge.Models;

namespace CodeChallenge.Services
{
    public interface IReportingService
    {
        ReportingStructure GetReportingStructure(string employeeId);
    }
}

