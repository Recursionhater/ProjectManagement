using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ProjectManagement.Api.Api.Extensions;
using ProjectManagement.Api.Bll.Interfaces;
using ProjectManagement.Api.Bll.Services;
using ProjectManagement.Api.Dal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<ProjectManagementDbContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddHttpLogging();

var app = builder.Build();
app.UseHttpLogging();

app.MapGet("/", () => "Hello World!");
app.AddProjectApiEndpoints();
app.AddEmployeeEndPoints();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProjectManagementDbContext>();
    var dbCreator = context.GetService<IRelationalDatabaseCreator>();
    var strategy = context.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
        await dbCreator.EnsureCreatedAsync(); 
        await ProjectManagementDbContextSeed.SeedAsync(context);
    });
}

app.Run();