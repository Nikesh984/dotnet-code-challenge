using System;
using System.Linq;
using CodeChallenge.Data;
using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ICompensationService _compensationService;
        private readonly ILogger _logger;

        public CompensationController(ICompensationService compensationService, ILogger<CompensationController> logger)
        {
            _compensationService = compensationService;
            _logger = logger;
        }


        [HttpPost]
        public IActionResult Create([FromBody] Compensation compensation)
        {

            var createdCompensation = _compensationService.Create(compensation);

            if (createdCompensation != null)
            {
                _logger.LogDebug($"Received compensation create request for '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

                return CreatedAtAction(nameof(GetCompensationById), new { employeeId = createdCompensation.CompensationId }, createdCompensation);

            }

            return NotFound($"No employee exisits with Employee Id: {compensation.Employee.EmployeeId}");

        }


        [HttpGet("{employeeId}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String employeeId)
        {
            var compensation = _compensationService.GetCompensationByEmployeeId(employeeId);

            if (compensation == null)
            {
                return NotFound($"Compensation not found for employeeId: '{employeeId}'");
            }

            return Ok(compensation);

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