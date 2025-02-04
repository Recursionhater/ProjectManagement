namespace ProjectManagement.Shared.Dto.Projects;

public class GetAllProjectsDto
{
    public string? FilterPropertyName { get; init; }

    public string? FilterValue { get; init; }

    public string? SortPropertyName { get; init; }

    public string? SortDirection { get; init; }
}