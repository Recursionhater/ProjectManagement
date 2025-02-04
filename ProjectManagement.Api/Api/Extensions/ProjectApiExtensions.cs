using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Bll.Interfaces;
using ProjectManagement.Shared.Dto.Projects;

namespace ProjectManagement.Api.Api.Extensions;

public static class ProjectApiExtensions
{
    public static WebApplication AddProjectApiEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/projects");
        group.MapGet("", ListAsync);
        group.MapGet("{id:int}", GetAsync);
        group.MapPost("", CreateAsync);
        group.MapPut("", UpdateAsync);
        group.MapDelete("{id:int}", DeleteAsync);
        group.MapPatch("{id:int}/employees/add", AddEmployeesAsync);
        group.MapPatch("{id:int}/employees/remove", RemoveEmployeesAsync);
        return app;
    }

    private static async Task RemoveEmployeesAsync(int id, [FromBody] IEnumerable<int> employeeIds,
        IProjectService service,
        CancellationToken token)
    {
        await service.RemoveEmployeesAsync(id, employeeIds, token);
    }

    private static async Task AddEmployeesAsync(int id, [FromBody] IEnumerable<int> employeeIds,
        IProjectService service,
        CancellationToken token)
    {
        await service.AddEmployeesAsync(id, employeeIds, token);
    }

    private static async Task DeleteAsync(int id, IProjectService service, CancellationToken token)
    {
        await service.DeleteAsync(id, token);
    }

    private static async Task UpdateAsync(UpdateProjectDto dto, IProjectService service, CancellationToken token)
    {
        await service.UpdateAsync(dto, token);
    }

    private static async Task<IResult> CreateAsync(CreateProjectDto dto, IProjectService service,
        CancellationToken token)
    {
        var id = await service.CreateAsync(dto, token);

        return Results.Created($"api/projects/{id}", null);
    }

    private static async Task<ProjectDto> GetAsync(int id, IProjectService service, CancellationToken token)
    {
        return await service.GetAsync(id, token);
    }
    
    private static async Task<IEnumerable<ProjectDto>> ListAsync(
        string? filterPropertyName,
        string? filterValue,
        string? sortPropertyName,
        string? sortDirection,
        IProjectService service,
        CancellationToken token)
    {
        var dto = new GetAllProjectsDto
        {
            FilterPropertyName = filterPropertyName,
            FilterValue = filterValue,
            SortPropertyName = sortPropertyName,
            SortDirection = sortDirection
        };

        return await service.GetAllAsync(dto, token);
    }
}