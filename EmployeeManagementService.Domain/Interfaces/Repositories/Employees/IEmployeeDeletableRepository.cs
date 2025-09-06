namespace EmployeeManagementService.Domain.Interfaces.Repositories.Employees;

/// <summary>
/// Удаление сотрудника
/// </summary>
public interface IEmployeeDeletableRepository
{
	/// <summary>
	/// Удаление сотрудника по Id
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task DeleteByIdAsync(int id);
}
