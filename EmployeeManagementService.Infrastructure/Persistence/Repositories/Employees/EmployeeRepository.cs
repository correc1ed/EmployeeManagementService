using Dapper;
using EmployeeManagementService.Domain.Data.Abstractions.Entities;
using EmployeeManagementService.Domain.Data.Abstractions.Repositories.Employees;
using Npgsql;

namespace EmployeeManagementService.Infrastructure.Persistence.Repositories.Employees;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Employee>> GetByCompanyIdAsync(int companyId)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var sql = $@"
            SELECT e.id, e.name, e.surname, e.phone, e.company_id as CompanyId,
                   p.id, p.type, p.number,
                   d.id, d.name, d.phone
            FROM employees e
            INNER JOIN passports p ON e.passport_id = p.id
            INNER JOIN departments d ON e.department_id = d.id
            WHERE e.company_id = {companyId}";

        var employees = await connection.QueryAsync<Employee, Passport, Department, Employee>(
            sql,
            (employee, passport, department) =>
            {
                employee.Passport = passport;
                employee.Department = department;
                return employee;
            },
            splitOn: "id,id"
        );

        return employees;
    }

    public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(int departmentId)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var sql = $@"
            SELECT e.id, e.name, e.surname, e.phone, e.company_id as CompanyId,
                   p.id, p.type, p.number,
                   d.id, d.name, d.phone
            FROM employees e
            INNER JOIN passports p ON e.passport_id = p.id
            INNER JOIN departments d ON e.department_id = d.id
            WHERE e.department_id = {departmentId}";

        var employees = await connection.QueryAsync<Employee, Passport, Department, Employee>(
            sql,
            (employee, passport, department) =>
            {
                employee.Passport = passport;
                employee.Department = department;
                return employee;
            },
            splitOn: "id,id"
        );

        return employees;
    }

    public async Task<Employee> GetByIdAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);

        var sql = $@"
            SELECT e.id, e.name, e.surname, e.phone, e.company_id as CompanyId,
                   p.id, p.type, p.number,
                   d.id, d.name, d.phone
            FROM employees e
            INNER JOIN passports p ON e.passport_id = p.id
            INNER JOIN departments d ON e.department_id = d.id
            WHERE e.id = {id}";

        var employee = await connection.QueryAsync<Employee, Passport, Department, Employee>(
            sql,
            (employee, passport, department) =>
            {
                employee.Passport = passport;
                employee.Department = department;
                return employee;
            },
            splitOn: "id,id"
        );

        return employee.FirstOrDefault();
    }

    public async Task<int> CreateAsync(Employee employee, Passport passport, Department department)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var passportSql = $@"
                INSERT INTO passports (type, number)
                VALUES ('{passport.Type}', '{passport.Number}')
                RETURNING id";

            var passportId = await connection.ExecuteScalarAsync<int>(passportSql, transaction: transaction);

            var departmentSql = $@"
                INSERT INTO departments (name, phone)
                VALUES ('{department.Name}', '{department.Phone}')
                RETURNING id";

            var departmentId = await connection.ExecuteScalarAsync<int>(departmentSql, transaction: transaction);

            var employeeSql = $@"
                INSERT INTO employees (name, surname, phone, company_id, passport_id, department_id)
                VALUES ('{employee.Name}', '{employee.Surname}', '{employee.Phone}', 
                        {employee.CompanyId}, {passportId}, {departmentId})
                RETURNING id";

            var employeeId = await connection.ExecuteScalarAsync<int>(employeeSql, transaction: transaction);

            await transaction.CommitAsync();
            return employeeId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(int id, Employee employee, Passport passport, Department department)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            if (passport != null)
            {
                var passportSql = $@"
                    UPDATE passports 
                    SET type = '{passport.Type}', number = '{passport.Number}'
                    WHERE id = (SELECT passport_id FROM employees WHERE id = {id})";

                await connection.ExecuteAsync(passportSql, transaction: transaction);
            }

            if (department != null)
            {
                var departmentSql = $@"
                    UPDATE departments 
                    SET name = '{department.Name}', phone = '{department.Phone}'
                    WHERE id = (SELECT department_id FROM employees WHERE id = {id})";

                await connection.ExecuteAsync(departmentSql, transaction: transaction);
            }

            if (employee != null)
            {
                var updates = new List<string>();
                if (!string.IsNullOrEmpty(employee.Name)) updates.Add($"name = '{employee.Name}'");
                if (!string.IsNullOrEmpty(employee.Surname)) updates.Add($"surname = '{employee.Surname}'");
                if (!string.IsNullOrEmpty(employee.Phone)) updates.Add($"phone = '{employee.Phone}'");
                if (employee.CompanyId > 0) updates.Add($"company_id = {employee.CompanyId}");

                if (updates.Count > 0)
                {
                    var employeeSql = $"UPDATE employees SET {string.Join(", ", updates)} WHERE id = {id}";
                    await connection.ExecuteAsync(employeeSql, transaction: transaction);
                }
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var getRelatedIdsSql = $"SELECT passport_id, department_id FROM employees WHERE id = {id}";
            var relatedIds = await connection.QueryFirstOrDefaultAsync<(int PassportId, int DepartmentId)>(
                getRelatedIdsSql, transaction: transaction);

            var deleteEmployeeSql = $"DELETE FROM employees WHERE id = {id}";
            await connection.ExecuteAsync(deleteEmployeeSql, transaction: transaction);

            var deletePassportSql = $"DELETE FROM passports WHERE id = {relatedIds.PassportId}";
            await connection.ExecuteAsync(deletePassportSql, transaction: transaction);

            var deleteDepartmentSql = $"DELETE FROM departments WHERE id = {relatedIds.DepartmentId}";
            await connection.ExecuteAsync(deleteDepartmentSql, transaction: transaction);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
