using Dapper;
using EmployeeManagementService.Application.Features.Employees.DTO;
using EmployeeManagementService.Domain.Entities;
using EmployeeManagementService.Domain.Interfaces.Repositories.Employees;
using Npgsql;

namespace EmployeeManagementService.Infrastructure.Persistence.Repositories.Employees;

public class EmployeeReadableRepository : IEmployeeReadableRepository
{
	private readonly string _connectionString;

	public EmployeeReadableRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	public async Task<IEnumerable<Employee>> GetByCompanyIdAsync(int companyId)
	{
		using var connection = new NpgsqlConnection(_connectionString);

		var sql = @"
            SELECT e.id, e.name, e.surname, e.phone, e.company_id as CompanyId,
                   p.id, p.type, p.number,
                   d.id, d.name, d.phone
            FROM employees e
            INNER JOIN passports p ON e.passport_id = p.id
            INNER JOIN departments d ON e.department_id = d.id
            WHERE e.company_id = @CompanyId";

		var employees = await connection.QueryAsync<Employee, Passport, Department, Employee>(
			sql,
			(employee, passport, department) =>
			{
				employee.Passport = passport;
				employee.Department = department;

				return employee;
			},
			new { CompanyId = companyId },
			splitOn: "id,id"
		);

		return employees;
	}

	public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentId)
	{
		using var connection = new NpgsqlConnection(_connectionString);

		var sql = @"
            SELECT e.id, e.name, e.surname, e.phone, e.company_id as CompanyId,
                   p.id, p.type, p.number,
                   d.id, d.name, d.phone
            FROM employees e
            INNER JOIN passports p ON e.passport_id = p.id
            INNER JOIN departments d ON e.department_id = d.id
            WHERE e.department_id = @DepartmentId";

		var employees = await connection.QueryAsync<Employee, Passport, Department, Employee>(
			sql,
			(employee, passport, department) =>
			{
				employee.Passport = passport;
				employee.Department = department;

				return employee;
			},
			new { DepartmentId = departmentId },
			splitOn: "id,id"
		);

		return employees;
	}

	public async Task<Employee> GetByIdAsync(int id)
	{
		using var connection = new NpgsqlConnection(_connectionString);

		var sql = @"
        SELECT 
            e.id, e.name, e.surname, e.phone, e.company_id as CompanyId,
            p.id, p.type, p.number,
            d.id, d.name, d.phone
        FROM employees e
        INNER JOIN passports p ON e.passport_id = p.id
        INNER JOIN departments d ON e.department_id = d.id
        WHERE e.id = @Id";

		var employee = await connection.QueryAsync<Employee, Passport, Department, Employee>(
			sql,
			(employee, passport, department) =>
			{
				employee.Passport = passport;
				employee.Department = department;
				return employee;
			},
			new { Id = id },
			splitOn: "id,id"
		);

		return employee.FirstOrDefault();
	}
}
