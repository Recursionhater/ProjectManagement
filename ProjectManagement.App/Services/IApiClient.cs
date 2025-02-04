using ProjectManagement.Shared.Dto.Employees;
using ProjectManagement.Shared.Dto.Projects;
using Refit;

namespace ProjectManagement.App.Services;

public interface IApiClient
{
    [Get("/api/projects")]
    public Task<IReadOnlyCollection<ProjectDto>> GetAllProjectsAsync(GetAllProjectsDto dto, CancellationToken token);

    [Get("/api/projects/{id}")]
    public Task<ProjectDto> GetProjectByIdAsync(int id, CancellationToken token);

    [Post("/api/projects")]
    public Task CreateProjectAsync(CreateProjectDto dto, CancellationToken token);

    [Put("/api/projects")]
    public Task UpdateProjectAsync(UpdateProjectDto dto, CancellationToken token);

    [Patch("/api/projects/{projectId}/employees/add")]
    public Task AddEmployeesAsync(int projectId, IEnumerable<int> employeeIds, CancellationToken token);

    [Patch("/api/projects/{projectId}/employees/remove")]
    public Task RemoveEmployeesAsync(int projectId, IEnumerable<int> employeeIds, CancellationToken token);

    [Delete("/api/projects/{id}")]
    public Task DeleteProjectAsync(int id, CancellationToken token);

    [Get("/api/employees")]
    public Task<IReadOnlyCollection<EmployeeDto>> GetAllEmployeesAsync(CancellationToken token);

    [Get("/api/employees/{id}")]
    public Task<EmployeeDto> GetEmployeeByIdAsync(int id, CancellationToken token);

    [Post("/api/employees")]
    public Task<int> CreateEmployeeAsync(CreateEmployeeDto dto, CancellationToken token);

    [Put("/api/employees")]
    public Task<int> UpdateEmployeeAsync(UpdateEmployeeDto dto, CancellationToken token);

    [Delete("/api/employees/{id}")]
    public Task DeleteEmployeeAsync(int id, CancellationToken token);
}