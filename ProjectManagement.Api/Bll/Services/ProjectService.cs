using System.Linq.Expressions;
using Gridify;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Bll.Entities;
using ProjectManagement.Api.Bll.Interfaces;
using ProjectManagement.Api.Dal;
using ProjectManagement.Shared.Dto.Projects;

namespace ProjectManagement.Api.Bll.Services;

public class ProjectService(ProjectManagementDbContext context) : IProjectService
{
    public async Task<IEnumerable<ProjectDto>> GetAllAsync(GetAllProjectsDto dto, CancellationToken token)
    {
        var query = context.Projects.AsNoTracking().Select(Map());

        if (dto.FilterPropertyName is { } filterBy)
        {
            query = query.ApplyFiltering($"{filterBy}=*{dto.FilterValue}");
        }

        if (dto.SortPropertyName is { } sort)
        {
            query = query.ApplyOrdering($"{sort} {dto.SortDirection}");
        }

        return await query
            .ToListAsync(token);
    }

    public async Task<ProjectDto> GetAsync(int id, CancellationToken token)
    {
        return await context.Projects
                   .AsNoTracking()
                   .Select(Map())
                   .SingleOrDefaultAsync(x => x.Id == id, token) ??
               throw new InvalidOperationException($"Project not found by {id}");
    }

    public async Task<int> CreateAsync(CreateProjectDto dto, CancellationToken token)
    {
        var project = new Project
        {
            Name = dto.Name,
            CustomerCompany = dto.CustomerCompanyName,
            ContactorCompany = dto.ContractorCompanyName,
            OwnerId = dto.OwnerId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Priority = dto.Priority
        };
        context.Projects.Add(project);
        await context.SaveChangesAsync(token);

        return project.Id;
    }

    public async Task UpdateAsync(UpdateProjectDto dto, CancellationToken token)
    {
        var project = await context.Projects.FirstOrDefaultAsync(x => x.Id == dto.Id, token) ??
                      throw new InvalidOperationException($"Project not found by {dto.Id}");
        project.Name = dto.Name;
        project.CustomerCompany = dto.CustomerCompanyName;
        project.ContactorCompany = dto.ContractorCompanyName;
        project.OwnerId = dto.OwnerId;
        project.StartDate = dto.StartDate;
        project.EndDate = dto.EndDate;
        project.Priority = dto.Priority;
        context.Projects.Update(project);
        await context.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(int id, CancellationToken token)
    {
        await context.Projects
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(token);
    }

    public async Task AddEmployeesAsync(int projectId, IEnumerable<int> employeeIds, CancellationToken token)
    {
        var project = context.Projects
                          .Include(x => x.Employees)
                          .FirstOrDefault(x => x.Id == projectId) ??
                      throw new InvalidOperationException($"Project not found by {projectId}");

        employeeIds = employeeIds.Except(project.Employees.Select(x => x.Id));

        var employees = await context.Employees
            .Where(x => employeeIds.Contains(x.Id))
            .ToListAsync(token);

        project.Employees.AddRange(employees);

        await context.SaveChangesAsync(token);
    }

    public async Task RemoveEmployeesAsync(int projectId, IEnumerable<int> employeeIds, CancellationToken token)
    {
        var project = context.Projects
                          .Include(x => x.Employees)
                          .FirstOrDefault(x => x.Id == projectId) ??
                      throw new InvalidOperationException($"Project not found by {projectId}");

        project.Employees.RemoveAll(x => employeeIds.Contains(x.Id));

        await context.SaveChangesAsync(token);
    }


    private static Expression<Func<Project, ProjectDto>> Map()
    {
        return x => new ProjectDto
        {
            Id = x.Id,
            Name = x.Name,
            CustomerCompanyName = x.CustomerCompany,
            ContractorCompanyName = x.ContactorCompany,
            OwnerId = x.OwnerId,
            OwnerName = $"{x.Owner.FirstName} {x.Owner.LastName} {x.Owner.MiddleName}",
            StartDate = x.StartDate,
            EndDate = x.EndDate,
            Priority = x.Priority
        };
    }
}