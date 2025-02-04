using ProjectManagement.Shared.Dto.Projects;

namespace ProjectManagement.Api.Bll.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync(GetAllProjectsDto dto, CancellationToken token);

    Task<ProjectDto> GetAsync(int id, CancellationToken token);

    Task<int> CreateAsync(CreateProjectDto dto, CancellationToken token);

    Task UpdateAsync(UpdateProjectDto dto, CancellationToken token);

    Task DeleteAsync(int id, CancellationToken token);
    
    Task AddEmployeesAsync(int projectId, IEnumerable<int> employeeIds, CancellationToken token);
    
    Task RemoveEmployeesAsync(int projectId,IEnumerable<int> employeeIds, CancellationToken token);
}