using EmployeeManagementService.Application.Interfaces.Employees;
using EmployeeManagementService.Domain.Interfaces.Repositories.Employees;
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

		services.AddScoped<IEmployeeWritableRepository, EmployeeWritableRepository>();
		services.AddScoped<IEmployeeReadableRepository, EmployeeReadableRepository>();
		services.AddScoped<IEmployeeDeletableRepository, EmployeeDeletableRepository>();

		return services;
	}
}
