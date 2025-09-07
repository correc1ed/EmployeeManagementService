using EmployeeManagementService.Domain.Data.Entities;

namespace EmployeeManagementService.Domain.Data.Interfaces.Repositories.Employees;

public interface IEmployeeRepository
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

	/// <summary>
	/// Удаление сотрудника по Id
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public Task DeleteByIdAsync(int id);
}
