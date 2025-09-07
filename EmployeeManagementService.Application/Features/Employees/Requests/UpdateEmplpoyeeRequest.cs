using EmployeeManagementService.Application.Features.Employees.DTO;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementService.Application.Features.Employees.Requests;

public class UpdateEmplpoyeeRequest
{
	[StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 100 символов")]
	[RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s\-]+$", ErrorMessage = "Имя может содержать только буквы, пробелы и дефисы")]
	public string? Name { get; set; }

	[StringLength(100, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать от 2 до 100 символов")]
	[RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s\-]+$", ErrorMessage = "Фамилия может содержать только буквы, пробелы и дефисы")]
	public string? Surname { get; set; }

	[StringLength(20, ErrorMessage = "Телефон не должен превышать 20 символов")]
	[Phone(ErrorMessage = "Неверный формат телефона")]
	public string? Phone { get; set; }

	[Range(1, int.MaxValue, ErrorMessage = "Неверный идентификатор компании")]
	public int? CompanyId { get; set; }

	public PassportForUpdateDTO? Passport { get; set; }

	public DepartmentForUpdateDTO? Department { get; set; }
}
