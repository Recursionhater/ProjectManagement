using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagement.Api.Bll.Entities;

namespace ProjectManagement.Api.Dal.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("project");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(128);
        
        builder.Property(x => x.CustomerCompany)
            .HasMaxLength(128);
        
        builder.Property(x => x.ContactorCompany)
            .HasMaxLength(128);
        
        builder.HasOne(x => x.Owner)
            .WithMany()
            .HasForeignKey(x => x.OwnerId);
        
        builder.Property(x => x.StartDate);
        
        builder.Property(x => x.EndDate);
        
        builder.HasMany(x => x.Employees)
            .WithMany(x => x.Projects);
    }
}