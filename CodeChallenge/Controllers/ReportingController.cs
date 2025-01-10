using System;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{
    /// <summary>
    /// Controller responsible for handling requests related to the reporting structure of employees.
    /// </summary>

    [ApiController]
    [Route("api/reportingStructure")]
    public class ReportingController : ControllerBase
    {
        // Service for managing the logic related to reporting structures
        private readonly IReportingService _reportingService;

        // Logger instance to log relevant information regarding reporting structure requests
        private readonly ILogger _logger;


        /// <summary>
        /// Constructor for the ReportingController.
        /// Initializes the ReportingService and Logger.
        /// </summary>
        /// <param name="reportingService">An instance of IReportingService to fetch reporting structure data on the fly.</param>
        /// <param name="logger">Logger instance to log debug information.</param>

        public ReportingController(IReportingService reportingService, ILogger<ReportingController> logger)
        {
            _reportingService = reportingService;
            _logger = logger;
        }


        /// <summary>
        /// API endpoint to get the reporting structure of an employee based on their employee ID.
        /// </summary>
        /// <param name="id">The ID of the employee for whom the reporting structure is requested.</param>
        /// <returns>
        /// 200 OK with the reporting structure if found.
        /// 404 Not Found if no reporting structure is found for the given employee ID.
        /// </returns>
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