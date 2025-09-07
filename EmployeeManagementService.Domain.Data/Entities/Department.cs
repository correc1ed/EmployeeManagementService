namespace EmployeeManagementService.Domain.Data.Abstractions.Entities;

/// <summary>
/// Отделение
/// </summary>
public class Department : BaseEntity
{
	/// <summary>
	/// Название отдела
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Телефон отдела
	/// </summary>
	public string Phone { get; set; }
}
