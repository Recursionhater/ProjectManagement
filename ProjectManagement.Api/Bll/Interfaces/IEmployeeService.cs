using ProjectManagement.Shared.Dto.Employees;


namespace ProjectManagement.Api.Bll.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken token);
    
    Task<EmployeeDto> GetByIdAsync(int id, CancellationToken token);
    
    Task <int> CreateAsync(CreateEmployeeDto dto, CancellationToken token);
    
    Task UpdateAsync(UpdateEmployeeDto dto, CancellationToken token);
    
    Task DeleteAsync(int id, CancellationToken token);
}