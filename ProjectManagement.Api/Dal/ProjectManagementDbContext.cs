using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Bll.Entities;

namespace ProjectManagement.Api.Dal;

public class ProjectManagementDbContext(DbContextOptions<ProjectManagementDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    
    public DbSet<Employee> Employees => Set<Employee>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectManagementDbContext).Assembly);
    }
}