using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementService.Application.Features.Employees.DTO;

public class PassportForUpdateDTO
{
	[StringLength(10, ErrorMessage = "Тип паспорта не должен превышать 10 символов")]
	public string? Type { get; set; }

	[StringLength(20, ErrorMessage = "Номер паспорта не должен превышать 20 символов")]
	[RegularExpression(@"^[0-9A-Za-z]+$", ErrorMessage = "Номер паспорта может содержать только цифры и буквы")]
	public string? Number { get; set; }
}
