namespace ProjectManagement.Shared.Dto.Employees;

public class EmployeeDto
{
    public required int Id { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string MiddleName { get; init; }

    public required string Email { get; init; }

    public override string ToString() => $"{FirstName} {LastName} {MiddleName} ({Email})";
}