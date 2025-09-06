using EmployeeManagementService.Application.Features.Common;
using EmployeeManagementService.Application.Features.Employees.DTO;
using EmployeeManagementService.Application.Features.Employees.Requests;
using EmployeeManagementService.Application.Features.Employees.Responses;
using EmployeeManagementService.Application.Interfaces.Employees;
using EmployeeManagementService.Domain.Entities;
using EmployeeManagementService.Domain.Interfaces.Repositories.Employees;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementService.Infrastructure.Services.Employees;

public class EmployeeService : IEmployeeService
{
	private readonly IEmployeeWritableRepository _employeeWritableRepository;
	private readonly IEmployeeReadableRepository _employeeReadableRepository;
	private readonly IEmployeeDeletableRepository _employeeDeletableRepository;

	private readonly ILogger<EmployeeService> _logger;

	public EmployeeService(IEmployeeWritableRepository employeeWritableRepository, IEmployeeReadableRepository employeeReadableRepository, IEmployeeDeletableRepository employeeDeletableRepository,
		ILogger<EmployeeService> logger)
	{
		_employeeReadableRepository = employeeReadableRepository;
		_employeeWritableRepository = employeeWritableRepository;
		_employeeDeletableRepository = employeeDeletableRepository;

		_logger = logger;
	}

	public async Task<ExecutionResult<GetEmployeesResponse>> GetEmployeesByCompanyIdAsync(int companyId)
	{
		try
		{
			if (companyId <= 0)
			{
				_logger.LogError("Invalid company id");

				return new ExecutionResult<GetEmployeesResponse>()
				{

					IsSuccess = false,
					ErrorMessage = "Invalid company id"
				};
			}

			var employees = await _employeeReadableRepository.GetByCompanyIdAsync(companyId);

			if (employees == null || !employees.Any())
			{
				_logger.LogError("No employees found for the specified company");

				return new ExecutionResult<GetEmployeesResponse>()
				{
					IsSuccess = true,
					ErrorMessage = "No employees found for the specified company"
				};
			}

			var result = employees.Select(_ => new EmployeeDTO()
			{
				Id = _.Id,
				Name = _.Name,
				CompanyId = _.CompanyId,
				Phone = _.Phone,
				Surname = _.Surname,
				Passport = _.Passport,
				Department = _.Department
			}).ToList();

			return new ExecutionResult<GetEmployeesResponse>()
			{
				IsSuccess = true,
				Data = new GetEmployeesResponse()
				{
					Employees = result
				}
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error retrieving employees for company {CompanyId}", companyId);

			return new ExecutionResult<GetEmployeesResponse>()
			{
				IsSuccess = false,
				ErrorMessage = ex.Message
			};
		}
	}

	public async Task<ExecutionResult<GetEmployeesResponse>> GetEmployeesByDepartmentIdAsync(int departmentId)
	{
		try
		{
			if (departmentId <= 0)
			{
				_logger.LogError("Invalid department id");

				return new ExecutionResult<GetEmployeesResponse>()
				{
					IsSuccess = false,
					ErrorMessage = "Invalid department id"
				};
			}

			var employees = await _employeeReadableRepository.GetByDepartmentIdAsync(departmentId);

			if (employees == null || !employees.Any())
			{
				return new ExecutionResult<GetEmployeesResponse>()
				{
					IsSuccess = true,
					ErrorMessage = "No employees found for the specified department"
				};
			}

			var result = employees.Select(_ => new EmployeeDTO()
			{
				Id = _.Id,
				Name = _.Name,
				CompanyId = _.CompanyId,
				Phone = _.Phone,
				Surname = _.Surname,
				Passport = _.Passport,
				Department = _.Department
			}).ToList();

			return new ExecutionResult<GetEmployeesResponse>()
			{
				IsSuccess = true,
				Data = new GetEmployeesResponse()
				{
					Employees = result
				}
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error retrieving employees for department {DepartmentId}", departmentId);

			return new ExecutionResult<GetEmployeesResponse>()
			{
				IsSuccess = false,
				ErrorMessage = ex.Message
			};
		}
	}

	public async Task<ExecutionResult<PostEmplpoyeeResponse>> PostEmployeeAsync(PostEmplpoyeeRequest postEmplpoyeeRequest)
	{
		try
		{
			var passport = new Passport()
			{
				Type = postEmplpoyeeRequest.Passport.Type,
				Number = postEmplpoyeeRequest.Passport.Number
			};

			var department = new Department()
			{
				Name = postEmplpoyeeRequest.Department.Name,
				Phone = postEmplpoyeeRequest.Department.Phone
			};

			var employee = new Employee()
			{
				Name = postEmplpoyeeRequest.Name,
				Surname = postEmplpoyeeRequest.Surname,
				Phone = postEmplpoyeeRequest.Phone,
				CompanyId = postEmplpoyeeRequest.CompanyId,
				PassportId = passport.Id,
				Passport = passport,
				DepartmentId = department.Id,
				Department = department
			};

			var result = await _employeeWritableRepository.CreateAsync(employee, passport, department);

			return new ExecutionResult<PostEmplpoyeeResponse>()
			{
				IsSuccess = true,
				Data = new PostEmplpoyeeResponse()
				{
					Id = result
				}
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message);

			return new ExecutionResult<PostEmplpoyeeResponse>()
			{
				IsSuccess = false,
				ErrorMessage = ex.Message
			};
		}
	}

	public async Task<ExecutionResult> UpdateEmployeeAsync(int id, PutEmplpoyeeRequest putEmplpoyeeRequest)
	{
		try
		{
			var employee = await _employeeReadableRepository.GetByIdAsync(id);

			if (employee == null)
			{
				_logger.LogError("Employee is not found");

				return new ExecutionResult()
				{
					IsSuccess = false,
					ErrorMessage = "Employee is not found"
				};
			}

			var passport = employee.Passport;

			var department = employee.Department;

			if (putEmplpoyeeRequest.Name != null)
			{
				employee.Name = putEmplpoyeeRequest.Name;
			}
			if (putEmplpoyeeRequest.Surname != null)
			{
				employee.Surname = putEmplpoyeeRequest.Surname;
			}
			if (putEmplpoyeeRequest.Phone != null)
			{
				employee.Phone = putEmplpoyeeRequest.Phone;
			}
			if (putEmplpoyeeRequest.CompanyId != null)
			{
				employee.CompanyId = putEmplpoyeeRequest.CompanyId.Value;
			}
			if (putEmplpoyeeRequest.Passport != null)
			{
				if (putEmplpoyeeRequest.Passport.Type != null)
				{
					passport.Type = putEmplpoyeeRequest.Passport.Type;
				}
				if (putEmplpoyeeRequest.Passport.Number != null)
				{
					passport.Number = putEmplpoyeeRequest.Passport.Number;
				}
			}
			if (putEmplpoyeeRequest.Department != null)
			{
				if (putEmplpoyeeRequest.Department.Name != null)
				{
					department.Name = putEmplpoyeeRequest.Department.Name;
				}
				if (putEmplpoyeeRequest.Department.Phone != null)
				{
					department.Phone = putEmplpoyeeRequest.Department.Phone;
				}
			}

			await _employeeWritableRepository.UpdateAsync(id, employee, passport, department);

			return new ExecutionResult()
			{
				IsSuccess = true
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message);

			return new ExecutionResult()
			{
				IsSuccess = false,
				ErrorMessage = ex.Message
			};
		}
	}

	public async Task<ExecutionResult> DeleteEmplpoyeeByIdAsync(int id)
	{
		try
		{
			if (id == null)
			{
				_logger.LogError("Id is null");

				return new ExecutionResult()
				{
					IsSuccess = false,
					ErrorMessage = "Id is null"
				};
			}

			await _employeeDeletableRepository.DeleteByIdAsync(id);

			return new ExecutionResult()
			{
				IsSuccess = true
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message);

			return new ExecutionResult()
			{
				IsSuccess = false,
				ErrorMessage = ex.Message
			};
		}
	}
}
