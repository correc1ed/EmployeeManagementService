using EmployeeManagementService.Application.Features.Employees.DTO;

namespace EmployeeManagementService.Application.Features.Employees.Responses;

public class GetEmployeesResponse
{
	public IEnumerable<EmployeeDTO> Employees { get; set; }
}
