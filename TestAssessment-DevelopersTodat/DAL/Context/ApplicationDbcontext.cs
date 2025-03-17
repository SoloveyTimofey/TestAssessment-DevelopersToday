using Microsoft.EntityFrameworkCore;
using TestAssessment_DevelopersTodat.DAL.Models;

namespace TestAssessment_DevelopersTodat.DAL.Context;

public class ApplicationDbcontext : DbContext
{
    public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options) { }

    public DbSet<CabData> CabData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbcontext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}