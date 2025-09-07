using EmployeeManagementService.Application.Features.Employees.Interfaces;
using EmployeeManagementService.Domain.Data.Abstractions.Repositories.Employees;
using EmployeeManagementService.Infrastructure.Persistence.Repositories.Employees;
using EmployeeManagementService.Infrastructure.Services.Employees;

namespace EmployeeManagementService.API.Extensions;

public static class ServiceExtensions
{
	public static void AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton(provider =>
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			return connectionString;
		});
	}

	public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
	{
		services.AddScoped<IEmployeeService, EmployeeService>();

		services.AddScoped<IEmployeeRepository, EmployeeRepository>();

		return services;
	}
}
