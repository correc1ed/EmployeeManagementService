using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementService.Application.Features.Employees.DTO;

public class DepartmentForUpdateDTO
{
	[StringLength(100, MinimumLength = 2, ErrorMessage = "Название отдела должно содержать от 2 до 100 символов")]
	public string? Name { get; set; }

	[StringLength(20, ErrorMessage = "Телефон отдела не должен превышать 20 символов")]
	[Phone(ErrorMessage = "Неверный формат телефона отдела")]
	public string? Phone { get; set; }
}
