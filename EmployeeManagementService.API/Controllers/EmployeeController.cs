using EmployeeManagementService.API.Controllers.Base;
using EmployeeManagementService.Application.Features.Employees.Requests;
using EmployeeManagementService.Application.Interfaces.Employees;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementService.API.Controllers;

public class EmployeeController : BaseController
{
	private readonly IEmployeeService _employeeService;

	public EmployeeController(IEmployeeService employeeService)
	{
		_employeeService = employeeService;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] PostEmplpoyeeRequest postEmplpoyeeRequest)
	{
		var result = await _employeeService.PostEmployeeAsync(postEmplpoyeeRequest);

		if (result.IsSuccess)
		{
			return Ok(result.Data);
		}
		else
		{
			return BadRequest(result);
		}
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		var result = await _employeeService.DeleteEmplpoyeeByIdAsync(id);

		if (result.IsSuccess)
		{
			return NoContent();
		}
		else
		{
			return BadRequest(result);
		}
	}

	[HttpGet("{companyId}")]
	public async Task<IActionResult> GetByCompanyIdAsync(int companyId)
	{
		var result = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);

		if (result.IsSuccess)
		{
			return Ok(result.Data);
		}
		else
		{
			return BadRequest(result);
		}
	}

	[HttpGet("{departmentId}")]
	public async Task<IActionResult> GetByDepartmentIdAsync(int departmentId)
	{
		var result = await _employeeService.GetEmployeesByDepartmentIdAsync(departmentId);

		if (result.IsSuccess)
		{
			return Ok(result.Data);
		}
		else
		{
			return BadRequest(result);
		}
	}

	[HttpPatch("{id}")]
	public async Task<IActionResult> Update(int id, [FromBody] PutEmplpoyeeRequest employee)
	{
		var result = await _employeeService.UpdateEmployeeAsync(id, employee);

		if (result.IsSuccess)
		{
			return Ok(result);
		}
		else
		{
			return BadRequest(result);
		}
	}
}
