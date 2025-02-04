namespace ProjectManagement.Api.Bll.Entities;

public class Employee
{ 
    public int Id { get; set; } 
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required string MiddleName { get; set; }
    
    public required string Email { get; set; }
    
    public List<Project> Projects { get; } = [];
}