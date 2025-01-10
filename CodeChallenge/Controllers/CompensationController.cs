using System;
using System.Linq;
using CodeChallenge.Data;
using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{

    // Controller responsible for handling compensation related API requests
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        // Service to handle business logic related to compensations
        private readonly ICompensationService _compensationService;
        private readonly ILogger _logger;


        /// <summary>
        ///  Constructor for the CompensationController.
        ///  Initializes the CompensationService and Logger.
        /// </summary>
        /// <param name="compensationService">An instance of ICompensationService for managing compensation data.</param>
        /// <param name="logger">Logger instance for logging messages.</param>
        public CompensationController(ICompensationService compensationService, ILogger<CompensationController> logger)
        {
            _compensationService = compensationService;
            _logger = logger;
        }

        /// <summary>
        /// API endpoint to create a new compensation for an employee.
        /// </summary>
        /// <param name="compensation">The compensation data to be created.</param>
        /// <returns>
        /// 201 Created if the compensation is successfully created
        /// 404 Not Found if no employee exists with the given Employee ID.
        /// </returns>
        [HttpPost]
        public IActionResult Create([FromBody] Compensation compensation)
        {

            if (compensation == null)
            {
                return BadRequest();
            }

            // Check if compensation already exists for the employee
            var existingCompensation = _compensationService.GetCompensationByEmployeeId(compensation.Employee.EmployeeId);

            if (existingCompensation != null)
            {
                // If compensation exists, route to the Edit method to update
                _logger.LogDebug($"Compensation already exists for employee '{compensation.Employee.FirstName} {compensation.Employee.LastName}', updating compensation.");

                return UpdateCompensation(compensation);
            }

            // If compensation does not exist, create a new one
            var createdCompensation = _compensationService.Create(compensation);

            if (createdCompensation != null)
            {
                _logger.LogDebug($"Received compensation create request for '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

                return CreatedAtAction(nameof(GetCompensationById), new { employeeId = createdCompensation.CompensationId }, createdCompensation);

            }

            return NotFound($"No employee exisits with Employee Id: {compensation.Employee.EmployeeId}");

        }

        /// <summary>
        /// API endpoint to get the compensation details for a given employee by their employee ID.
        /// </summary>
        /// <param name="employeeId">The Employee ID for which compensation details are requested.</param>
        /// <returns>
        /// 200 OK if the compensation is found, along with the compensation data.
        /// 404 Not Found if no compensation is found for the given employee ID.
        /// </returns>
        [HttpGet("{employeeId}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String employeeId)
        {
            var compensation = _compensationService.GetCompensationByEmployeeId(employeeId);

            if (compensation == null)
            {
                return NotFound($"Compensation not found for employeeId: '{employeeId}'");
            }

            _logger.LogDebug($"Received compensation get request for '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

            return Ok(compensation);

        }

        /// <summary>
        /// API endpoint to update the compensation details for an existing employee.
        /// </summary>
        /// <param name="compensation">The compensation data to be updated.</param>
        /// <returns>
        /// 200 OK if the compensation is successfully updated.
        /// 404 Not Found if no compensation is found for the given employee ID.
        /// </returns>
        [HttpPut("{employeeId}")]
        public IActionResult UpdateCompensation([FromBody] Compensation compensation)
        {
            var updatedCompensation = _compensationService.UpdateCompensation(compensation);

            if (updatedCompensation == null)
            {
                return NotFound($"Compensation not found for employee ID: '{compensation.Employee.EmployeeId}'");
            }

            _logger.LogDebug($"Compensation updated for employee: '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

            return Ok(updatedCompensation);
        }


        // [HttpGet]
        // public IActionResult GetCompensations()
        // {
        //     var compensations = .Compensations.ToList();
        //     if (compensations != null)
        //     {
        //         return Ok(compensations);
        //     }

        //     return NotFound($"Nothing to return");
        // }

    }
}