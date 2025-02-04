using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Bll.Entities;

namespace ProjectManagement.Api.Dal;

public static class ProjectManagementDbContextSeed
{
    public static async Task SeedAsync(ProjectManagementDbContext dbContext)
    {
        if (await dbContext.Projects.AnyAsync())
        {
            return;
        }

        var random = Random.Shared;
        var employees = Enumerable.Range(1, 100)
            .Select(i => new Employee
            {
                FirstName = $"Employee #{i}",
                LastName = $"Last Name #{i}",
                MiddleName = $"Middle Name #{i}",
                Email = $"employee{i}@email.com",
            })
            .ToList();

        var projects = Enumerable.Range(1, 10)
            .Select(i => new Project
            {
                Name = $"Project #{i}",
                CustomerCompany = $"Customer Company #{i}",
                ContactorCompany = $"Contractor Company #{i}",
                Owner = employees[random.Next(0, employees.Count)],
                StartDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-random.Next(1, 10))),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddYears(random.Next(1, 10))),
                Priority = random.Next(1, 100)
            })
            .ToList();
        
        // Add random count of random employees to projects
        foreach (var project in projects)
        {
            var count = random.Next(employees.Count);
            var randomEmployees = employees.OrderBy(_ => random.Next()).Take(count);
            project.Employees.AddRange(randomEmployees);
        }

        dbContext.Projects.AddRange(projects);
        dbContext.Employees.AddRange(employees);

        await dbContext.SaveChangesAsync();
    }
}