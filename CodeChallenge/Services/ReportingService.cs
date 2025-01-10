using System;
using Castle.Core.Logging;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Services
{
    public class ReportingService : IReportingService
    {

        private readonly IReportingRepository _reportingRepository;
        private readonly ILogger<ReportingService> _logger;

        public ReportingService(IReportingRepository reportingRepository, ILogger<ReportingService> logger)
        {
            _reportingRepository = reportingRepository;
            _logger = logger;
        }

        public ReportingStructure GetReportingStructure(string employeeId)
        {
            if (!String.IsNullOrEmpty(employeeId))
            {
                return _reportingRepository.GetReportingStructure(employeeId);
            }

            return null;
        }
    }
}