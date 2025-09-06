namespace EmployeeManagementService.Application.Features.Common;

public class ExecutionResult
{
	public bool IsSuccess { get; set; }
	public string ErrorMessage { get; set; } = string.Empty;
}

public class ExecutionResult<T> : ExecutionResult
{
	public T? Data { get; set; }
}
