namespace ProjectManagement.Shared.Dto.Projects;

public class CreateProjectDto
{
    public required string Name { get; init; }

    public required string CustomerCompanyName { get; init; }

    public required string ContractorCompanyName { get; init; }

    public required int OwnerId { get; init; }

    public required DateOnly StartDate { get; init; }

    public required DateOnly EndDate { get; init; }

    public required int Priority { get; init; }
}