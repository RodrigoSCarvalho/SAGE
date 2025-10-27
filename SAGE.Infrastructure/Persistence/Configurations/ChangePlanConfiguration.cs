using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAGE.Domain.ChangePlans;

namespace SAGE.Infrastructure.Persistence.Configurations
{
    public class ChangePlanConfiguration : IEntityTypeConfiguration<ChangePlan>
    {
        public void Configure(EntityTypeBuilder<ChangePlan> builder)
        {
            builder.ToTable("ChangePlans");
            builder.HasKey(cp => cp.Id);
            builder.Property(cp => cp.Title).IsRequired().HasMaxLength(255);
            builder.Property(cp => cp.Description).HasMaxLength(5000);
            builder.Property(cp => cp.CreatedAt).IsRequired();
            builder.Property(cp => cp.CreatedBy).IsRequired();
        }
    }
}