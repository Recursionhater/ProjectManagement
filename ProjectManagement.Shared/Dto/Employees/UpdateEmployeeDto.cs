namespace ProjectManagement.Shared.Dto.Employees;

public class UpdateEmployeeDto
{
    public required int Id { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string MiddleName { get; init; }

    public required string Email { get; init; }
}