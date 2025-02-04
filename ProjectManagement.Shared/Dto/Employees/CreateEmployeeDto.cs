namespace ProjectManagement.Shared.Dto.Employees;

public class CreateEmployeeDto
{
    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string MiddleName { get; init; }

    public required string Email { get; init; }
}