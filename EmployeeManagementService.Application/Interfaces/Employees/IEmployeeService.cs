using EmployeeManagementService.Application.Features.Common;
using EmployeeManagementService.Application.Features.Employees.Requests;
using EmployeeManagementService.Application.Features.Employees.Responses;
using EmployeeManagementService.Domain.Entities;

namespace EmployeeManagementService.Application.Interfaces.Employees;

/// <summary>
/// Сервис для взаимодействия с сотрудниками
/// </summary>
public interface IEmployeeService
{
	/// <summary>
	/// Выводить список сотрудников для указанной компании.
	/// </summary>
	/// <param name="companyId"></param>
	/// <returns></returns>
	public Task<ExecutionResult<GetEmployeesResponse>> GetEmployeesByCompanyIdAsync(int companyId);

	/// <summary>
	/// Выводить список сотрудников для указанного отдела компании. Все доступные поля.
	/// </summary>
	/// <param name="departmentId"></param>
	/// <returns></returns>
	public Task<ExecutionResult<GetEmployeesResponse>> GetEmployeesByDepartmentIdAsync(int departmentId);

	/// <summary>
	/// Добавлять сотрудников, в ответ должен приходить Id добавленного сотрудника.
	/// </summary>
	/// <param name="employee"></param>
	/// <returns></returns>
	public Task<ExecutionResult<PostEmplpoyeeResponse>> PostEmployeeAsync(PostEmplpoyeeRequest postEmplpoyeeRequest);

	/// <summary>
	/// Удалять сотрудников по Id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task<ExecutionResult> DeleteEmplpoyeeByIdAsync(int id);

	/// <summary>
	/// Изменять сотрудника по его Id. Изменения должно быть только тех полей, которые указаны в запросе.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="employee"></param>
	/// <returns></returns>
	public Task<ExecutionResult> UpdateEmployeeAsync(int id, PutEmplpoyeeRequest putEmplpoyeeRequest);
}
