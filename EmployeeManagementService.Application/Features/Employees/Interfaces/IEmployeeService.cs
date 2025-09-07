using EmployeeManagementService.Application.Features.Employees.Requests;
using EmployeeManagementService.Application.Features.Employees.Responses;

namespace EmployeeManagementService.Application.Features.Employees.Interfaces;

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
	public Task<GetEmployeesResponse> GetEmployeesByCompanyIdAsync(int companyId);

	/// <summary>
	/// Выводить список сотрудников для указанного отдела компании. Все доступные поля.
	/// </summary>
	/// <param name="departmentId"></param>
	/// <returns></returns>
	public Task<GetEmployeesResponse> GetEmployeesByDepartmentIdAsync(int departmentId);

	/// <summary>
	/// Добавлять сотрудников, в ответ должен приходить Id добавленного сотрудника.
	/// </summary>
	/// <param name="employee"></param>
	/// <returns></returns>
	public Task<CreateEmplpoyeeResponse> CreateEmployeeAsync(CreateEmplpoyeeRequest postEmplpoyeeRequest);

	/// <summary>
	/// Удалять сотрудников по Id.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task DeleteEmplpoyeeByIdAsync(int id);

	/// <summary>
	/// Изменять сотрудника по его Id. Изменения должно быть только тех полей, которые указаны в запросе.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="employee"></param>
	/// <returns></returns>
	public Task UpdateEmployeeAsync(int id, UpdateEmplpoyeeRequest updateEmplpoyeeRequest);
}
