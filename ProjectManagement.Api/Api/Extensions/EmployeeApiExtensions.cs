using ProjectManagement.Api.Bll.Interfaces;
using ProjectManagement.Shared.Dto.Employees;

namespace ProjectManagement.Api.Api.Extensions;

public static class EmployeeApiExtensions
{
    public static WebApplication AddEmployeeEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/employees");
        group.MapGet("", GetAllAsync);
        group.MapGet("{id:int}", GetAsync);
        group.MapPost("", CreateAsync);
        group.MapPut("", UpdateAsync);
        group.MapDelete("{id:int}", DeleteAsync);
        return app;
    }

    private static async Task<IEnumerable<EmployeeDto>> GetAllAsync(IEmployeeService service, CancellationToken token)
    {
        return await service.GetAllAsync(token);
    }
    
    private static async Task<EmployeeDto> GetAsync(int id, IEmployeeService service, CancellationToken token)
    {
        return await service.GetByIdAsync(id, token);
    }
    
    private static async Task<IResult> CreateAsync(CreateEmployeeDto dto, IEmployeeService service,
        CancellationToken token)
    {
        var id = await service.CreateAsync(dto, token);

        return Results.Created($"api/employees/{id}", null);
    }
    
    private static async Task UpdateAsync(UpdateEmployeeDto dto, IEmployeeService service, CancellationToken token)
    {
        await service.UpdateAsync(dto, token);
    }
    
    private static async Task DeleteAsync(int id, IEmployeeService service, CancellationToken token)
    {
        await service.DeleteAsync(id, token);
    }
}