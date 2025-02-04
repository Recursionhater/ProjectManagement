namespace ProjectManagement.Api.Bll.Entities;

public class Project
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
    
    public required string CustomerCompany { get; set; }
    
    public required string ContactorCompany { get; set; }

    public int OwnerId { get; set; }
    
    public Employee Owner { get; set; } = null!;
    
    public DateOnly StartDate { get; set; }
    
    public DateOnly EndDate { get; set; }
    
    public int Priority { get; set; }
    
    public List<Employee> Employees { get; } = [];
}