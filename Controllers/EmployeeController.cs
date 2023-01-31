using JWTAuthentication.Model;
using JWTAuthentication.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     // [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

        }
        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employeeToList = _employeeRepository.GetEmployees().ToList();
            return Ok(employeeToList);
        }

        [HttpGet("{employeeId:int}")]
        public IActionResult GetEmployee(int employeeId)
        {
            if (employeeId == 0) return BadRequest();
            var employee = _employeeRepository.GetEmployee(employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpPost]
        public IActionResult CreateEmployee(Employee employee)
        {
            if (employee == null)
                return BadRequest();
            if (!_employeeRepository.CreateEmployee(employee))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpDelete ("employeeId:int")]
        public IActionResult DeleteEmployee(int employeeId)
        {
            if (employeeId == 0) 
             return BadRequest();
            var employee = _employeeRepository.GetEmployee(employeeId);
            if (employee == null) return NotFound();
            if(! _employeeRepository.DeleteEmployee(employee))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(); ;

        }
        [HttpPut]
        public IActionResult UpdateEmployee(Employee employee)
        {
            if (employee == null)
                return BadRequest();
            if(! _employeeRepository.UpdateEmployee(employee))
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
        
    }
}
