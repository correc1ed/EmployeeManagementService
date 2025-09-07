using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementService.Application.Features.Employees.ViewModel;

public class PassportViewModel
{
	[Required(ErrorMessage = "Тип паспорта обязателен для заполнения")]
	[StringLength(10, ErrorMessage = "Тип паспорта не должен превышать 10 символов")]
	public string Type { get; set; }

	[Required(ErrorMessage = "Номер паспорта обязателен для заполнения")]
	[StringLength(20, ErrorMessage = "Номер паспорта не должен превышать 20 символов")]
	[RegularExpression(@"^[0-9A-Za-z]+$", ErrorMessage = "Номер паспорта может содержать только цифры и буквы")]
	public string Number { get; set; }
}