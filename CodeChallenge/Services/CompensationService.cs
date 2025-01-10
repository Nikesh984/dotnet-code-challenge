using System;
using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {

        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ICompensationRepository compensationRepository, ILogger<CompensationService> logger)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public Compensation Create(Compensation compensation)
        {

            if (compensation != null)
            {
                var createdCompensation = _compensationRepository.Create(compensation);
                //_compensationRepository.SaveAsync().Wait();
                return createdCompensation;
            }

            return null;
        }

        public Compensation GetCompensationByEmployeeId(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetCompensationByEmployeeId(id);
            }

            return null;
        }
    }
}