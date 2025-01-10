using System;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{

    [ApiController]
    [Route("api/reportingStructure")]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingService _reportingService;
        private readonly ILogger _logger;

        public ReportingController(IReportingService reportingService, ILogger<ReportingController> logger)
        {
            _reportingService = reportingService;
            _logger = logger;
        }


        [HttpGet("{id}", Name = "getReportingStructure")]
        public IActionResult GetReportingStructure(String id)
        {
            _logger.LogDebug($"Received reporting structure request for employee : '{id}'");

            var reportingStructure = _reportingService.GetReportingStructure(id);

            if (reportingStructure == null)
            {
                return NotFound();
            }

            return Ok(reportingStructure);
        }

    }
}