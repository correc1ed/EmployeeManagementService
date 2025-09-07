using EmployeeManagementService.Application.Features.Employees.DTO;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementService.Application.Features.Employees.Requests;

public class CreateEmplpoyeeRequest
{
	[Required(ErrorMessage = "Имя сотрудника обязательно для заполнения")]
	[StringLength(100, MinimumLength = 2, ErrorMessage = "Имя должно содержать от 2 до 100 символов")]
	[RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s\-]+$", ErrorMessage = "Имя может содержать только буквы, пробелы и дефисы")]
	public string Name { get; set; }

	[Required(ErrorMessage = "Фамилия сотрудника обязательна для заполнения")]
	[StringLength(100, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать от 2 до 100 символов")]
	[RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ\s\-]+$", ErrorMessage = "Фамилия может содержать только буквы, пробелы и дефисы")]
	public string Surname { get; set; }

	[Required(ErrorMessage = "Телефон обязателен для заполнения")]
	[StringLength(20, ErrorMessage = "Телефон не должен превышать 20 символов")]
	[Phone(ErrorMessage = "Неверный формат телефона")]
	public string Phone { get; set; }

	[Required(ErrorMessage = "Компания обязательна для заполнения")]
	[Range(1, int.MaxValue, ErrorMessage = "Неверный идентификатор компании")]
	public int CompanyId { get; set; }

	[Required(ErrorMessage = "Данные паспорта обязательны для заполнения")]
	public PassportDTO Passport { get; set; }

	[Required(ErrorMessage = "Данные отдела обязательны для заполнения")]
	public DepartmentDTO Department { get; set; }
}
