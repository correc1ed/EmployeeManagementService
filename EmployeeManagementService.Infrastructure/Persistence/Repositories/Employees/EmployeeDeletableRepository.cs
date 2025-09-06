using Dapper;
using EmployeeManagementService.Domain.Interfaces.Repositories.Employees;
using Npgsql;

namespace EmployeeManagementService.Infrastructure.Persistence.Repositories.Employees;

public class EmployeeDeletableRepository : IEmployeeDeletableRepository
{
	private readonly string _connectionString;

	public EmployeeDeletableRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task DeleteByIdAsync(int id)
	{
		using var connection = new NpgsqlConnection(_connectionString);

		var sql = "DELETE FROM employees WHERE id = @Id";
		await connection.ExecuteAsync(sql, new { Id = id });
	}
}
