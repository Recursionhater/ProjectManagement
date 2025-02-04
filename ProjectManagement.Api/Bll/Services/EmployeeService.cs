using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Bll.Entities;
using ProjectManagement.Api.Bll.Interfaces;
using ProjectManagement.Api.Dal;
using ProjectManagement.Shared.Dto.Employees;

namespace ProjectManagement.Api.Bll.Services;

public class EmployeeService(ProjectManagementDbContext context) : IEmployeeService
{
    
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync(CancellationToken token)
    {
       return await context.Employees.AsNoTracking().Select(Map()).ToListAsync(token);
       
    }

    public async Task<EmployeeDto> GetByIdAsync(int id, CancellationToken token)
    {
        return await context.Employees
                   .AsNoTracking()
                   .Select(Map())
                   .SingleOrDefaultAsync(x => x.Id == id, token) ??
               throw new InvalidOperationException($"Employee not found by {id}");
    }

    public async Task<int> CreateAsync(CreateEmployeeDto dto, CancellationToken token)
    {
        var employee = new Employee()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            MiddleName = dto.MiddleName,
            Email = dto.Email,
        };
        
        context.Employees.Add(employee);
        await context.SaveChangesAsync(token);
        
        return employee.Id;
    }

    public async Task UpdateAsync(UpdateEmployeeDto dto, CancellationToken token)
    {
        var employee = await context.Employees.FindAsync([dto.Id], token) ??
                       throw new InvalidOperationException($"Employee not found by {dto.Id}");
        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        context.Employees.Update(employee);
        await context.SaveChangesAsync(token);
    }

    public async Task DeleteAsync(int id, CancellationToken token)
    {
        await context.Employees
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(token);
    }

    private static Expression<Func<Employee, EmployeeDto>> Map()
    {
        return x => new EmployeeDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            MiddleName = x.MiddleName,
            Email = x.Email
        };
    }
    
    
}