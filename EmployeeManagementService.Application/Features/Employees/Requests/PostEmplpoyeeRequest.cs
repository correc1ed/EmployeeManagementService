using EmployeeManagementService.Application.Features.Employees.DTO;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementService.Application.Features.Employees.Requests;

public class PostEmplpoyeeRequest
{
	public string Name { get; set; }
	public string Surname { get; set; }
	public string Phone { get; set; }
	public int CompanyId { get; set; }
	public PassportDTO Passport { get; set; }
	public DepartmentDTO Department { get; set; }
}
