using CodeChallenge.Models;

namespace CodeChallenge.Repositories
{
    public interface IReportingRepository
    {
        ReportingStructure GetReportingStructure(string employeeId);
    }
}