using EmployeeManagementService.Application.Features.Employees.DTO;

namespace EmployeeManagementService.Application.Features.Employees.Requests;

public class PutEmplpoyeeRequest
{
	public string? Name { get; set; }
	public string? Surname { get; set; }
	public string? Phone { get; set; }
	public int? CompanyId { get; set; }
	public PassportForUpdateDTO? Passport { get; set; }
	public DepartmentForUpdateDTO? Department { get; set; }
}
