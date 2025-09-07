using EmployeeManagementService.Application.Features.Employees.ViewModel;

namespace EmployeeManagementService.Application.Features.Employees.Responses;

public class GetEmployeesResponse
{
	public IEnumerable<EmployeeViewModel> Employees { get; set; }
}
