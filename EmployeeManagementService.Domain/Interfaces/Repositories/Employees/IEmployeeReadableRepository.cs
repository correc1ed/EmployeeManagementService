using EmployeeManagementService.Domain.Entities;

namespace EmployeeManagementService.Domain.Interfaces.Repositories.Employees;

/// <summary>
/// Чтение сотрудников
/// </summary>
public interface IEmployeeReadableRepository
{
	/// <summary>
	/// Вывод сотрудника по id
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task<Employee> GetByIdAsync(int id);

	/// <summary>
	/// Выводить список сотрудников для указанной компании
	/// </summary>
	/// <param name="companyId"></param>
	/// <returns></returns>
	public Task<IEnumerable<Employee>> GetByCompanyIdAsync(int companyId);

	/// <summary>
	/// Выводить список сотрудников для указанного отдела компании.
	/// </summary>
	/// <param name="departmentId"></param>
	/// <returns></returns>
	public Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentId);
}
