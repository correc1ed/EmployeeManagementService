using EmployeeManagementService.Application.Features.Employees.DTO;
using EmployeeManagementService.Application.Features.Employees.Interfaces;
using EmployeeManagementService.Application.Features.Employees.Requests;
using EmployeeManagementService.Application.Features.Employees.Responses;
using EmployeeManagementService.Domain.Data.Abstractions.Entities;
using EmployeeManagementService.Domain.Data.Abstractions.Repositories.Employees;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementService.Infrastructure.Services.Employees;

public class EmployeeService : IEmployeeService
{
	private readonly IEmployeeRepository _employeeRepository;
	private readonly ILogger<EmployeeService> _logger;

	public EmployeeService(
		IEmployeeRepository employeeRepository,
		ILogger<EmployeeService> logger)
	{
		_employeeRepository = employeeRepository;
		_logger = logger;
	}

	public async Task<GetEmployeesResponse> GetEmployeesByCompanyIdAsync(int companyId)
	{
		try
		{
			if (companyId <= 0)
			{
				var errorMessage = "Invalid company id";
				_logger.LogError(errorMessage);
				throw new ArgumentException(errorMessage, nameof(companyId));
			}

			var employees = await _employeeRepository.GetByCompanyIdAsync(companyId);

			if (employees == null || !employees.Any())
			{
				var errorMessage = "No employees found for the specified company";
				_logger.LogWarning(errorMessage);
				return new GetEmployeesResponse { Employees = new List<EmployeeDTO>() };
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

			return new GetEmployeesResponse()
			{
				Employees = result
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error retrieving employees for company {CompanyId}", companyId);
			throw;
		}
	}

	public async Task<GetEmployeesResponse> GetEmployeesByDepartmentIdAsync(int departmentId)
	{
		try
		{
			if (departmentId <= 0)
			{
				var errorMessage = "Invalid department id";
				_logger.LogError(errorMessage);
				throw new ArgumentException(errorMessage, nameof(departmentId));
			}

			var employees = await _employeeRepository.GetByDepartmentIdAsync(departmentId);

			if (employees == null || !employees.Any())
			{
				var errorMessage = "No employees found for the specified department";
				_logger.LogWarning(errorMessage);
				return new GetEmployeesResponse { Employees = new List<EmployeeDTO>() };
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

			return new GetEmployeesResponse()
			{
				Employees = result
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error retrieving employees for department {DepartmentId}", departmentId);
			throw;
		}
	}

	public async Task<CreateEmplpoyeeResponse> CreateEmployeeAsync(CreateEmplpoyeeRequest createEmplpoyeeRequest)
	{
		try
		{
			if (createEmplpoyeeRequest == null)
			{
				var errorMessage = "Request cannot be null";
				_logger.LogError(errorMessage);
				throw new ArgumentNullException(nameof(createEmplpoyeeRequest), errorMessage);
			}

			var passport = new Passport()
			{
				Type = createEmplpoyeeRequest.Passport.Type,
				Number = createEmplpoyeeRequest.Passport.Number
			};

			var department = new Department()
			{
				Name = createEmplpoyeeRequest.Department.Name,
				Phone = createEmplpoyeeRequest.Department.Phone
			};

			var employee = new Employee()
			{
				Name = createEmplpoyeeRequest.Name,
				Surname = createEmplpoyeeRequest.Surname,
				Phone = createEmplpoyeeRequest.Phone,
				CompanyId = createEmplpoyeeRequest.CompanyId
			};

			var result = await _employeeRepository.CreateAsync(employee, passport, department);

			return new CreateEmplpoyeeResponse()
			{
				Id = result
			};
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error creating employee");
			throw;
		}
	}

	public async Task UpdateEmployeeAsync(int id, UpdateEmplpoyeeRequest updateEmplpoyeeRequest)
	{
		try
		{
			if (id <= 0)
			{
				var errorMessage = "Invalid employee id";
				_logger.LogError(errorMessage);
				throw new ArgumentException(errorMessage, nameof(id));
			}

			if (updateEmplpoyeeRequest == null)
			{
				var errorMessage = "Request cannot be null";
				_logger.LogError(errorMessage);
				throw new ArgumentNullException(nameof(updateEmplpoyeeRequest), errorMessage);
			}

			var employee = await _employeeRepository.GetByIdAsync(id);

			if (employee == null)
			{
				var errorMessage = $"Employee with id {id} not found";
				_logger.LogError(errorMessage);
				throw new KeyNotFoundException(errorMessage);
			}

			var passport = employee.Passport;
			var department = employee.Department;

			if (updateEmplpoyeeRequest.Name != null)
			{
				employee.Name = updateEmplpoyeeRequest.Name;
			}
			if (updateEmplpoyeeRequest.Surname != null)
			{
				employee.Surname = updateEmplpoyeeRequest.Surname;
			}
			if (updateEmplpoyeeRequest.Phone != null)
			{
				employee.Phone = updateEmplpoyeeRequest.Phone;
			}
			if (updateEmplpoyeeRequest.CompanyId != null)
			{
				employee.CompanyId = updateEmplpoyeeRequest.CompanyId.Value;
			}
			if (updateEmplpoyeeRequest.Passport != null)
			{
				if (updateEmplpoyeeRequest.Passport.Type != null)
				{
					passport.Type = updateEmplpoyeeRequest.Passport.Type;
				}
				if (updateEmplpoyeeRequest.Passport.Number != null)
				{
					passport.Number = updateEmplpoyeeRequest.Passport.Number;
				}
			}
			if (updateEmplpoyeeRequest.Department != null)
			{
				if (updateEmplpoyeeRequest.Department.Name != null)
				{
					department.Name = updateEmplpoyeeRequest.Department.Name;
				}
				if (updateEmplpoyeeRequest.Department.Phone != null)
				{
					department.Phone = updateEmplpoyeeRequest.Department.Phone;
				}
			}

			await _employeeRepository.UpdateAsync(id, employee, passport, department);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating employee {EmployeeId}", id);
			throw;
		}
	}

	public async Task DeleteEmplpoyeeByIdAsync(int id)
	{
		try
		{
			if (id <= 0)
			{
				var errorMessage = "Invalid employee id";
				_logger.LogError(errorMessage);
				throw new ArgumentException(errorMessage, nameof(id));
			}

			var employee = await _employeeRepository.GetByIdAsync(id);

			if (employee == null)
			{
				var errorMessage = $"Employee with id {id} not found";
				_logger.LogError(errorMessage);
				throw new KeyNotFoundException(errorMessage);
			}

			await _employeeRepository.DeleteByIdAsync(id);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error deleting employee {EmployeeId}", id);
			throw;
		}
	}
}