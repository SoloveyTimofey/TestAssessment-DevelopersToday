using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestAssessment_DevelopersTodat.DAL.Models;

namespace TestAssessment_DevelopersTodat.DAL.Configurations;

internal class CabDataConfiguration : IEntityTypeConfiguration<CabData>
{
    public void Configure(EntityTypeBuilder<CabData> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.StoreAndFwdFlag)
            .HasConversion<string>()
            .HasMaxLength(3);

        builder.HasIndex(e => e.PULocationId);
        builder.HasIndex(e => e.StoreAndFwdFlag);
        builder.HasIndex(e => e.TripDistance);
    }
}