using EmployeeManagementService.Application.Features.Employees.Requests;
using EmployeeManagementService.Application.Features.Employees.ViewModel;
using EmployeeManagementService.Domain.Data.Entities;
using EmployeeManagementService.Domain.Data.Interfaces.Repositories.Employees;
using EmployeeManagementService.Infrastructure.Services.Employees;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EmployeeManagementService.Infrastructure.Tests;

public class EmployeeServiceTests
{
	private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
	private readonly Mock<ILogger<EmployeeService>> _loggerMock;
	private readonly EmployeeService _employeeService;

	public EmployeeServiceTests()
	{
		_employeeRepositoryMock = new Mock<IEmployeeRepository>();
		_loggerMock = new Mock<ILogger<EmployeeService>>();

		_employeeService = new EmployeeService(
			_employeeRepositoryMock.Object,
			_loggerMock.Object
		);
	}

	// Вспомогательные методы для создания тестовых данных
	private Employee CreateTestEmployee(int id = 1)
	{
		return new Employee
		{
			Id = id,
			Name = "Test",
			Surname = "User",
			Phone = "1234567890",
			CompanyId = 1,
			Passport = new Passport { Type = "RU", Number = "123456" },
			Department = new Department { Name = "IT", Phone = "1234" }
		};
	}

	private List<Employee> CreateTestEmployees(int count = 2)
	{
		var employees = new List<Employee>();
		for (int i = 1; i <= count; i++)
		{
			employees.Add(CreateTestEmployee(i));
		}
		return employees;
	}

	[Fact]
	public async Task GetEmployeesByCompanyIdAsync_ShouldReturnEmployees_WhenEmployeesExist()
	{
		// Arrange
		var companyId = 1;
		var testEmployees = CreateTestEmployees(2);
		_employeeRepositoryMock
			.Setup(repo => repo.GetByCompanyIdAsync(companyId))
			.ReturnsAsync(testEmployees);

		// Act
		var result = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);

		// Assert
		result.Should().NotBeNull();
		result.Employees.Should().HaveCount(2);
		result.Employees.First().Name.Should().Be("Test");
		_employeeRepositoryMock.Verify(repo => repo.GetByCompanyIdAsync(companyId), Times.Once);
	}

	[Fact]
	public async Task GetEmployeesByCompanyIdAsync_ShouldReturnEmptyList_WhenNoEmployeesExist()
	{
		// Arrange
		var companyId = 1;
		_employeeRepositoryMock
			.Setup(repo => repo.GetByCompanyIdAsync(companyId))
			.ReturnsAsync(new List<Employee>());

		// Act
		var result = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);

		// Assert
		result.Should().NotBeNull();
		result.Employees.Should().BeEmpty();
	}

	[Fact]
	public async Task GetEmployeesByCompanyIdAsync_ShouldThrowArgumentException_WhenInvalidCompanyId()
	{
		// Arrange
		var invalidCompanyId = 0;

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() =>
			_employeeService.GetEmployeesByCompanyIdAsync(invalidCompanyId));
	}

	[Fact]
	public async Task GetEmployeesByCompanyIdAsync_ShouldThrowException_WhenRepositoryThrowsException()
	{
		// Arrange
		var companyId = 1;
		_employeeRepositoryMock
			.Setup(repo => repo.GetByCompanyIdAsync(companyId))
			.ThrowsAsync(new Exception("Database error"));

		// Act & Assert
		await Assert.ThrowsAsync<Exception>(() =>
			_employeeService.GetEmployeesByCompanyIdAsync(companyId));
	}

	[Fact]
	public async Task GetEmployeesByDepartmentIdAsync_ShouldReturnEmployees_WhenEmployeesExist()
	{
		// Arrange
		var departmentId = 1;
		var testEmployees = CreateTestEmployees(2);
		_employeeRepositoryMock
			.Setup(repo => repo.GetByDepartmentIdAsync(departmentId))
			.ReturnsAsync(testEmployees);

		// Act
		var result = await _employeeService.GetEmployeesByDepartmentIdAsync(departmentId);

		// Assert
		result.Should().NotBeNull();
		result.Employees.Should().HaveCount(2);
		result.Employees.First().Name.Should().Be("Test");
		_employeeRepositoryMock.Verify(repo => repo.GetByDepartmentIdAsync(departmentId), Times.Once);
	}

	[Fact]
	public async Task GetEmployeesByDepartmentIdAsync_ShouldReturnEmptyList_WhenNoEmployeesExist()
	{
		// Arrange
		var departmentId = 1;
		_employeeRepositoryMock
			.Setup(repo => repo.GetByDepartmentIdAsync(departmentId))
			.ReturnsAsync(new List<Employee>());

		// Act
		var result = await _employeeService.GetEmployeesByDepartmentIdAsync(departmentId);

		// Assert
		result.Should().NotBeNull();
		result.Employees.Should().BeEmpty();
	}

	[Fact]
	public async Task GetEmployeesByDepartmentIdAsync_ShouldThrowArgumentException_WhenInvalidDepartmentId()
	{
		// Arrange
		var invalidDepartmentId = 0;

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() =>
			_employeeService.GetEmployeesByDepartmentIdAsync(invalidDepartmentId));
	}

	[Fact]
	public async Task CreateEmployeeAsync_ShouldReturnEmployeeId_WhenCreationSucceeds()
	{
		// Arrange
		var request = new CreateEmplpoyeeRequest
		{
			Name = "Test",
			Surname = "User",
			Phone = "1234567890",
			CompanyId = 1,
			Passport = new PassportViewModel { Type = "RU", Number = "123456" },
			Department = new DepartmentViewModel { Name = "IT", Phone = "1234" }
		};

		var expectedEmployeeId = 1;
		_employeeRepositoryMock
			.Setup(repo => repo.CreateAsync(It.IsAny<Employee>(), It.IsAny<Passport>(), It.IsAny<Department>()))
			.ReturnsAsync(expectedEmployeeId);

		// Act
		var result = await _employeeService.CreateEmployeeAsync(request);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(expectedEmployeeId);
		_employeeRepositoryMock.Verify(repo => repo.CreateAsync(
			It.IsAny<Employee>(),
			It.IsAny<Passport>(),
			It.IsAny<Department>()),
			Times.Once);
	}

	[Fact]
	public async Task CreateEmployeeAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
	{
		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() =>
			_employeeService.CreateEmployeeAsync(null));
	}

	[Fact]
	public async Task CreateEmployeeAsync_ShouldThrowException_WhenCreationFails()
	{
		// Arrange
		var request = new CreateEmplpoyeeRequest
		{
			Name = "Test",
			Surname = "User",
			Phone = "1234567890",
			CompanyId = 1,
			Passport = new PassportViewModel { Type = "RU", Number = "123456" },
			Department = new DepartmentViewModel { Name = "IT", Phone = "1234" }
		};

		_employeeRepositoryMock
			.Setup(repo => repo.CreateAsync(It.IsAny<Employee>(), It.IsAny<Passport>(), It.IsAny<Department>()))
			.ThrowsAsync(new Exception("Creation failed"));

		// Act & Assert
		await Assert.ThrowsAsync<Exception>(() =>
			_employeeService.CreateEmployeeAsync(request));
	}

	[Fact]
	public async Task UpdateEmployeeAsync_ShouldSucceed_WhenUpdateSucceeds()
	{
		// Arrange
		var employeeId = 1;
		var request = new UpdateEmplpoyeeRequest
		{
			Name = "Updated",
			Surname = "User",
			Phone = "0987654321",
			CompanyId = 2,
			Passport = new PassportForUpdateViewModel { Type = "US", Number = "654321" },
			Department = new DepartmentForUpdateViewModel { Name = "HR", Phone = "4321" }
		};

		var existingEmployee = CreateTestEmployee(employeeId);
		_employeeRepositoryMock
			.Setup(repo => repo.GetByIdAsync(employeeId))
			.ReturnsAsync(existingEmployee);

		_employeeRepositoryMock
			.Setup(repo => repo.UpdateAsync(employeeId, It.IsAny<Employee>(), It.IsAny<Passport>(), It.IsAny<Department>()))
			.Returns(Task.CompletedTask);

		// Act
		Func<Task> act = async () => await _employeeService.UpdateEmployeeAsync(employeeId, request);

		// Assert
		await act.Should().NotThrowAsync();
		_employeeRepositoryMock.Verify(repo => repo.UpdateAsync(
			employeeId,
			It.IsAny<Employee>(),
			It.IsAny<Passport>(),
			It.IsAny<Department>()),
			Times.Once);
	}

	[Fact]
	public async Task UpdateEmployeeAsync_ShouldThrowKeyNotFoundException_WhenEmployeeNotFound()
	{
		// Arrange
		var employeeId = 1;
		var request = new UpdateEmplpoyeeRequest
		{
			Name = "Updated"
		};

		_employeeRepositoryMock
			.Setup(repo => repo.GetByIdAsync(employeeId))
			.ReturnsAsync((Employee)null);

		// Act & Assert
		await Assert.ThrowsAsync<KeyNotFoundException>(() =>
			_employeeService.UpdateEmployeeAsync(employeeId, request));
	}

	[Fact]
	public async Task UpdateEmployeeAsync_ShouldThrowArgumentException_WhenInvalidEmployeeId()
	{
		// Arrange
		var invalidEmployeeId = 0;
		var request = new UpdateEmplpoyeeRequest
		{
			Name = "Updated"
		};

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() =>
			_employeeService.UpdateEmployeeAsync(invalidEmployeeId, request));
	}

	[Fact]
	public async Task UpdateEmployeeAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
	{
		// Arrange
		var employeeId = 1;

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentNullException>(() =>
			_employeeService.UpdateEmployeeAsync(employeeId, null));
	}

	[Fact]
	public async Task DeleteEmployeeByIdAsync_ShouldSucceed_WhenDeletionSucceeds()
	{
		// Arrange
		var employeeId = 1;
		var existingEmployee = CreateTestEmployee(employeeId);

		_employeeRepositoryMock
			.Setup(repo => repo.GetByIdAsync(employeeId))
			.ReturnsAsync(existingEmployee);

		_employeeRepositoryMock
			.Setup(repo => repo.DeleteByIdAsync(employeeId))
			.Returns(Task.CompletedTask);

		// Act
		Func<Task> act = async () => await _employeeService.DeleteEmplpoyeeByIdAsync(employeeId);

		// Assert
		await act.Should().NotThrowAsync();
		_employeeRepositoryMock.Verify(repo => repo.DeleteByIdAsync(employeeId), Times.Once);
	}

	[Fact]
	public async Task DeleteEmployeeByIdAsync_ShouldThrowKeyNotFoundException_WhenEmployeeNotFound()
	{
		// Arrange
		var employeeId = 1;

		_employeeRepositoryMock
			.Setup(repo => repo.GetByIdAsync(employeeId))
			.ReturnsAsync((Employee)null);

		// Act & Assert
		await Assert.ThrowsAsync<KeyNotFoundException>(() =>
			_employeeService.DeleteEmplpoyeeByIdAsync(employeeId));
	}

	[Fact]
	public async Task DeleteEmployeeByIdAsync_ShouldThrowArgumentException_WhenInvalidEmployeeId()
	{
		// Arrange
		var invalidEmployeeId = 0;

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() =>
			_employeeService.DeleteEmplpoyeeByIdAsync(invalidEmployeeId));
	}
}