using EmployeeManagementService.Domain.Entities;

namespace EmployeeManagementService.Domain.Interfaces.Repositories.Employees;

/// <summary>
/// Запись сотрудника
/// </summary>
public interface IEmployeeWritableRepository
{
	/// <summary>
	/// Добавлять сотрудников, в ответ должен приходить Id добавленного сотрудника
	/// </summary>
	/// <param name="employee"></param>
	/// <param name="passport"></param>
	/// <param name="department"></param>
	/// <returns></returns>
	public Task<int> CreateAsync(Employee employee, Passport passport, Department department);

	/// <summary>
	/// Изменять сотрудника по его Id. Изменения должно быть только тех полей, которые указаны в запросе
	/// </summary>
	/// <param name="id"></param>
	/// <param name="employee"></param>
	/// <param name="passport"></param>
	/// <param name="department"></param>
	/// <returns></returns>
	public Task UpdateAsync(int id, Employee employee, Passport passport, Department department);
}
