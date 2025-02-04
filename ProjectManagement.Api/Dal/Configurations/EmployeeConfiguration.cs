using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagement.Api.Bll.Entities;

namespace ProjectManagement.Api.Dal.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employee");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(128);
        
        builder.Property(x => x.LastName)
            .HasMaxLength(128);
        
        builder.Property(x => x.MiddleName)
            .HasMaxLength(128);
        
        builder.Property(x => x.Email)
            .HasMaxLength(128);
    }
}