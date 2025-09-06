using Dapper;
using EmployeeManagementService.Domain.Entities;
using EmployeeManagementService.Domain.Interfaces.Repositories.Employees;
using Npgsql;

namespace EmployeeManagementService.Infrastructure.Persistence.Repositories.Employees;

public class EmployeeWritableRepository : IEmployeeWritableRepository
{
	private readonly string _connectionString;

	public EmployeeWritableRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<int> CreateAsync(Employee employee, Passport passport, Department department)
	{
		using var connection = new NpgsqlConnection(_connectionString);
		connection.Open();

		using var transaction = connection.BeginTransaction();

		try
		{
			var passportSql = @"
                INSERT INTO passports (type, number)
                VALUES (@Type, @Number)
                RETURNING id";

			var passportId = await connection.ExecuteScalarAsync<int>(
				passportSql, passport, transaction);

			var departmentSql = @"
                INSERT INTO departments (name, phone)
                VALUES (@Name, @Phone)
                RETURNING id";

			var departmentId = await connection.ExecuteScalarAsync<int>(
				departmentSql, department, transaction);

			employee.PassportId = passportId;
			employee.DepartmentId = departmentId;

			var employeeSql = @"
                INSERT INTO employees (name, surname, phone, company_id, passport_id, department_id)
                VALUES (@Name, @Surname, @Phone, @CompanyId, @PassportId, @DepartmentId)
                RETURNING id";

			var employeeId = await connection.ExecuteScalarAsync<int>(
				employeeSql, employee, transaction);

			transaction.Commit();
			return employeeId;
		}
		catch
		{
			transaction.Rollback();
			throw;
		}
	}

	public async Task UpdateAsync(int id, Employee employee, Passport passport, Department department)
	{
		using var connection = new NpgsqlConnection(_connectionString);
		connection.Open();

		using var transaction = connection.BeginTransaction();

		try
		{
			if (passport != null)
			{
				var passportSql = @"
                    UPDATE passports 
                    SET type = @Type, number = @Number
                    WHERE id = (SELECT passport_id FROM employees WHERE id = @EmployeeId)";

				await connection.ExecuteAsync(
					passportSql,
					new { passport.Type, passport.Number, EmployeeId = id },
					transaction);
			}

			if (department != null)
			{
				var departmentSql = @"
                    UPDATE departments 
                    SET name = @Name, phone = @Phone
                    WHERE id = (SELECT department_id FROM employees WHERE id = @EmployeeId)";

				await connection.ExecuteAsync(
					departmentSql,
					new { department.Name, department.Phone, EmployeeId = id },
					transaction);
			}

			if (employee != null)
			{
				var updates = new List<string>();
				var parameters = new DynamicParameters();
				parameters.Add("Id", id);

				if (!string.IsNullOrEmpty(employee.Name))
				{
					updates.Add("name = @Name");
					parameters.Add("Name", employee.Name);
				}

				if (!string.IsNullOrEmpty(employee.Surname))
				{
					updates.Add("surname = @Surname");
					parameters.Add("Surname", employee.Surname);
				}

				if (!string.IsNullOrEmpty(employee.Phone))
				{
					updates.Add("phone = @Phone");
					parameters.Add("Phone", employee.Phone);
				}

				if (employee.CompanyId > 0)
				{
					updates.Add("company_id = @CompanyId");
					parameters.Add("CompanyId", employee.CompanyId);
				}

				if (updates.Count > 0)
				{
					var employeeSql = $"UPDATE employees SET {string.Join(", ", updates)} WHERE id = @Id";
					await connection.ExecuteAsync(employeeSql, parameters, transaction);
				}
			}

			transaction.Commit();
		}
		catch
		{
			transaction.Rollback();
			throw;
		}
	}
}
