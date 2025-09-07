using EmployeeManagementService.API.Controllers.Base;
using EmployeeManagementService.Application.Features.Employees.Interfaces;
using EmployeeManagementService.Application.Features.Employees.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementService.API.Controllers;

public class EmployeeController : BaseController
{
	private readonly IEmployeeService _employeeService;
	private readonly ILogger<EmployeeController> _logger;

	public EmployeeController(
		IEmployeeService employeeService,
		ILogger<EmployeeController> logger)
	{
		_employeeService = employeeService;
		_logger = logger;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateEmplpoyeeRequest postEmplpoyeeRequest)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _employeeService.CreateEmployeeAsync(postEmplpoyeeRequest);
			return Ok(result);
		}
		catch (ArgumentException ex)
		{
			_logger.LogWarning(ex, "Invalid request to create employee");
			return BadRequest(ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error creating employee");
			return StatusCode(500, "Internal server error");
		}
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{
			await _employeeService.DeleteEmplpoyeeByIdAsync(id);
			return NoContent();
		}
		catch (KeyNotFoundException ex)
		{
			_logger.LogWarning(ex, "Employee not found for deletion");
			return NotFound(ex.Message);
		}
		catch (ArgumentException ex)
		{
			_logger.LogWarning(ex, "Invalid request to delete employee");
			return BadRequest(ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error deleting employee");
			return StatusCode(500, "Internal server error");
		}
	}

	[HttpGet("{companyId}")]
	public async Task<IActionResult> GetByCompanyIdAsync(int companyId)
	{
		try
		{
			var result = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);
			return Ok(result);
		}
		catch (ArgumentException ex)
		{
			_logger.LogWarning(ex, "Invalid request to get employees by company");
			return BadRequest(ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting employees by company");
			return StatusCode(500, "Internal server error");
		}
	}

	[HttpGet("{departmentId}")]
	public async Task<IActionResult> GetByDepartmentIdAsync(int departmentId)
	{
		try
		{
			var result = await _employeeService.GetEmployeesByDepartmentIdAsync(departmentId);
			return Ok(result);
		}
		catch (ArgumentException ex)
		{
			_logger.LogWarning(ex, "Invalid request to get employees by department");
			return BadRequest(ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error getting employees by department");
			return StatusCode(500, "Internal server error");
		}
	}

	[HttpPatch("{id}")]
	public async Task<IActionResult> Update(int id, [FromBody] UpdateEmplpoyeeRequest employee)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await _employeeService.UpdateEmployeeAsync(id, employee);
			return Ok();
		}
		catch (KeyNotFoundException ex)
		{
			_logger.LogWarning(ex, "Employee not found for update");
			return NotFound(ex.Message);
		}
		catch (ArgumentException ex)
		{
			_logger.LogWarning(ex, "Invalid request to update employee");
			return BadRequest(ex.Message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating employee");
			return StatusCode(500, "Internal server error");
		}
	}
}