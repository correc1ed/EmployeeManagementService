using EmployeeManagementService.Application.Features.Employees.DTO;
using EmployeeManagementService.Application.Features.Employees.Requests;
using EmployeeManagementService.Domain.Entities;
using EmployeeManagementService.Domain.Interfaces.Repositories.Employees;
using EmployeeManagementService.Infrastructure.Services.Employees;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EmployeeManagementService.Infrastructure.Tests.Services.Employees;

public class EmployeeServiceTests
{
	private readonly Mock<IEmployeeWritableRepository> _writableRepositoryMock;
	private readonly Mock<IEmployeeReadableRepository> _readableRepositoryMock;
	private readonly Mock<IEmployeeDeletableRepository> _deletableRepositoryMock;
	private readonly EmployeeService _employeeService;

	public EmployeeServiceTests()
	{
		_writableRepositoryMock = new Mock<IEmployeeWritableRepository>();
		_readableRepositoryMock = new Mock<IEmployeeReadableRepository>();
		_deletableRepositoryMock = new Mock<IEmployeeDeletableRepository>();

		_employeeService = new EmployeeService(
			_writableRepositoryMock.Object,
			_readableRepositoryMock.Object,
			_deletableRepositoryMock.Object,
			Mock.Of<ILogger<EmployeeService>>()
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
	public async Task GetEmployeeByCompanyIdAsync_ShouldReturnEmployees_WhenEmployeesExist()
	{
		// Arrange
		var companyId = 1;
		var testEmployees = CreateTestEmployees(2);
		_readableRepositoryMock
			.Setup(repo => repo.GetByCompanyIdAsync(companyId))
			.ReturnsAsync(testEmployees);

		// Act
		var result = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Data.Employees.Should().HaveCount(2);

		result.Data.Employees.First().Name.Should().Be("Test");

		_readableRepositoryMock.Verify(repo => repo.GetByCompanyIdAsync(companyId), Times.Once);
	}

	[Fact]
	public async Task GetEmployeeByCompanyIdAsync_ShouldReturnEmptyList_WhenNoEmployeesExist()
	{
		// Arrange
		var companyId = 1;
		_readableRepositoryMock
			.Setup(repo => repo.GetByCompanyIdAsync(companyId))
			.ReturnsAsync(new List<Employee>());

		// Act
		var result = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.ErrorMessage.Should().Be("No employees found for the specified company");
	}

	[Fact]
	public async Task GetEmployeeByCompanyIdAsync_ShouldReturnError_WhenExceptionOccurs()
	{
		// Arrange
		var companyId = 1;
		_readableRepositoryMock
			.Setup(repo => repo.GetByCompanyIdAsync(companyId))
			.ThrowsAsync(new Exception("Database error"));

		// Act
		var result = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.ErrorMessage.Should().Be("Database error");
	}

	[Fact]
	public async Task GetEmployeeByDepartmentIdAsync_ShouldReturnEmployees_WhenEmployeesExist()
	{
		// Arrange
		var companyId = 1;
		var testEmployees = CreateTestEmployees(2);
		_readableRepositoryMock
			.Setup(repo => repo.GetByDepartmentIdAsync(companyId))
			.ReturnsAsync(testEmployees);

		// Act
		var result = await _employeeService.GetEmployeesByDepartmentIdAsync(companyId);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Data.Employees.Should().HaveCount(2);

		result.Data.Employees.First().Name.Should().Be("Test");

		_readableRepositoryMock.Verify(repo => repo.GetByDepartmentIdAsync(companyId), Times.Once);
	}

	[Fact]
	public async Task GetEmployeeByDepartmentIdAsync_ShouldReturnEmptyList_WhenNoEmployeesExist()
	{
		// Arrange
		var companyId = 1;
		_readableRepositoryMock
			.Setup(repo => repo.GetByDepartmentIdAsync(companyId))
			.ReturnsAsync(new List<Employee>());

		// Act
		var result = await _employeeService.GetEmployeesByDepartmentIdAsync(companyId);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.ErrorMessage.Should().Be("No employees found for the specified department");
	}

	[Fact]
	public async Task GetEmployeeByDepartmentIdAsync_ShouldReturnError_WhenExceptionOccurs()
	{
		// Arrange
		var companyId = 1;
		_readableRepositoryMock
			.Setup(repo => repo.GetByDepartmentIdAsync(companyId))
			.ThrowsAsync(new Exception("Database error"));

		// Act
		var result = await _employeeService.GetEmployeesByDepartmentIdAsync(companyId);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.ErrorMessage.Should().Be("Database error");
	}

	[Fact]
	public async Task PostEmployeeAsync_ShouldReturnEmployeeId_WhenCreationSucceeds()
	{
		// Arrange
		var request = new PostEmplpoyeeRequest
		{
			Name = "Test",
			Surname = "User",
			Phone = "1234567890",
			CompanyId = 1,
			Passport = new PassportDTO { Type = "RU", Number = "123456" },
			Department = new DepartmentDTO { Name = "IT", Phone = "1234" }
		};

		var expectedEmployeeId = 1;
		_writableRepositoryMock
			.Setup(repo => repo.CreateAsync(It.IsAny<Employee>(), It.IsAny<Passport>(), It.IsAny<Department>()))
			.ReturnsAsync(expectedEmployeeId);

		// Act
		var result = await _employeeService.PostEmployeeAsync(request);

		// Assert
		result.IsSuccess.Should().BeTrue();
		result.Data.Id.Should().Be(expectedEmployeeId);
		_writableRepositoryMock.Verify(repo => repo.CreateAsync(
			It.IsAny<Employee>(),
			It.IsAny<Passport>(),
			It.IsAny<Department>()),
			Times.Once);
	}

	[Fact]
	public async Task PostEmployeeAsync_ShouldReturnError_WhenCreationFails()
	{
		// Arrange
		var request = new PostEmplpoyeeRequest
		{
			Name = "Test",
			Surname = "User",
			Phone = "1234567890",
			CompanyId = 1,
			Passport = new PassportDTO { Type = "RU", Number = "123456" },
			Department = new DepartmentDTO { Name = "IT", Phone = "1234" }
		};

		_writableRepositoryMock
			.Setup(repo => repo.CreateAsync(It.IsAny<Employee>(), It.IsAny<Passport>(), It.IsAny<Department>()))
			.ThrowsAsync(new Exception("Creation failed"));

		// Act
		var result = await _employeeService.PostEmployeeAsync(request);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.ErrorMessage.Should().Be("Creation failed");
	}
	[Fact]
	public async Task UpdateEmployeeAsync_ShouldReturnSuccess_WhenUpdateSucceeds()
	{
		// Arrange
		var employeeId = 1;
		var request = new PutEmplpoyeeRequest
		{
			Name = "Updated",
			Surname = "User",
			Phone = "0987654321",
			CompanyId = 2,
			Passport = new PassportForUpdateDTO { Type = "US", Number = "654321" },
			Department = new DepartmentForUpdateDTO { Name = "HR", Phone = "4321" }
		};

		var existingEmployee = CreateTestEmployee(employeeId);
		_readableRepositoryMock
			.Setup(repo => repo.GetByIdAsync(employeeId))
			.ReturnsAsync(existingEmployee);

		_writableRepositoryMock
			.Setup(repo => repo.UpdateAsync(employeeId, It.IsAny<Employee>(), It.IsAny<Passport>(), It.IsAny<Department>()))
			.Returns(Task.CompletedTask);

		// Act
		var result = await _employeeService.UpdateEmployeeAsync(employeeId, request);

		// Assert
		result.IsSuccess.Should().BeTrue();
		_writableRepositoryMock.Verify(repo => repo.UpdateAsync(
			employeeId,
			It.IsAny<Employee>(),
			It.IsAny<Passport>(),
			It.IsAny<Department>()),
			Times.Once);
	}

	[Fact]
	public async Task UpdateEmployeeAsync_ShouldReturnError_WhenEmployeeNotFound()
	{
		// Arrange
		var employeeId = 1;
		var request = new PutEmplpoyeeRequest
		{
			Name = "Updated"
		};

		_readableRepositoryMock
			.Setup(repo => repo.GetByIdAsync(employeeId))
			.ReturnsAsync((Employee)null);

		// Act
		var result = await _employeeService.UpdateEmployeeAsync(employeeId, request);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.ErrorMessage.Should().Be("Employee is not found");
	}

	[Fact]
	public async Task UpdateEmployeeAsync_ShouldReturnError_WhenUpdateFails()
	{
		// Arrange
		var employeeId = 1;
		var request = new PutEmplpoyeeRequest
		{
			Name = "Updated"
		};

		var existingEmployee = CreateTestEmployee(employeeId);
		_readableRepositoryMock
			.Setup(repo => repo.GetByIdAsync(employeeId))
			.ReturnsAsync(existingEmployee);

		_writableRepositoryMock
			.Setup(repo => repo.UpdateAsync(employeeId, It.IsAny<Employee>(), It.IsAny<Passport>(), It.IsAny<Department>()))
			.ThrowsAsync(new Exception("Update failed"));

		// Act
		var result = await _employeeService.UpdateEmployeeAsync(employeeId, request);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.ErrorMessage.Should().Be("Update failed");
	}

	[Fact]
	public async Task DeleteEmployeeByIdAsync_ShouldReturnSuccess_WhenDeletionSucceeds()
	{
		// Arrange
		var employeeId = 1;
		_deletableRepositoryMock
			.Setup(repo => repo.DeleteByIdAsync(employeeId))
			.Returns(Task.CompletedTask);

		// Act
		var result = await _employeeService.DeleteEmplpoyeeByIdAsync(employeeId);

		// Assert
		result.IsSuccess.Should().BeTrue();
		_deletableRepositoryMock.Verify(repo => repo.DeleteByIdAsync(employeeId), Times.Once);
	}

	[Fact]
	public async Task DeleteEmployeeByIdAsync_ShouldReturnError_WhenDeletionFails()
	{
		// Arrange
		var employeeId = 1;
		_deletableRepositoryMock
			.Setup(repo => repo.DeleteByIdAsync(employeeId))
			.ThrowsAsync(new Exception("Deletion failed"));

		// Act
		var result = await _employeeService.DeleteEmplpoyeeByIdAsync(employeeId);

		// Assert
		result.IsSuccess.Should().BeFalse();
		result.ErrorMessage.Should().Be("Deletion failed");
	}
}