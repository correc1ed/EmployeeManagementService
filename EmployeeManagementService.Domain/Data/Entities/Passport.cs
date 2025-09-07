namespace EmployeeManagementService.Domain.Data.Entities;

/// <summary>
/// Паспорт
/// </summary>
public class Passport : BaseEntity
{
	/// <summary>
	/// Тип паспорта
	/// </summary>
	public string Type { get; set; }

	/// <summary>
	/// Номер паспорта
	/// </summary>
	public string Number { get; set; }
}