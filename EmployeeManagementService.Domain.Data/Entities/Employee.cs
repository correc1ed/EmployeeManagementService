namespace EmployeeManagementService.Domain.Data.Abstractions.Entities;

/// <summary>
/// Сотрудник
/// </summary>
public class Employee : BaseEntity
{
	/// <summary>
	/// Имя сотрудника
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Фамилия сотрудника
	/// </summary>
	public string Surname { get; set; }

	/// <summary>
	/// Номер телефона сотрудника
	/// </summary>
	public string Phone { get; set; }

	/// <summary>
	/// Id Компании
	/// </summary>
	public int CompanyId { get; set; }

	/// <summary>
	/// Id Паспорта
	/// </summary>
	public int PassportId { get; set; }

	/// <summary>
	/// Паспорт
	/// </summary>
	public Passport Passport { get; set; }

	/// <summary>
	/// Id Отделения
	/// </summary>
	public int DepartmentId { get; set; }

	/// <summary>
	/// Отделение
	/// </summary>
	public Department Department { get; set; }
}
