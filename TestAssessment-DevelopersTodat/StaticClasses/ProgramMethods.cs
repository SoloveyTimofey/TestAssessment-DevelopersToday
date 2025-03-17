using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestAssessment_DevelopersTodat.DAL.Context;
using TestAssessment_DevelopersTodat.DAL.Models;
using TestAssessment_DevelopersTodat.DAL.Repositories;
using TestAssessment_DevelopersTodat.Services;

namespace TestAssessment_DevelopersTodat.StaticClasses;

public static class ProgramMethods
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            var connectionString = "Server=.;Database=DeverlopersTodayTestAssessmentDb;User Id=sa;Password=root;MultipleActiveResultSets=True;TrustServerCertificate=True;";
            services.AddDbContext<ApplicationDbcontext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<CsvProcessor>();
            services.AddScoped<ICabDataRepository, CabDataRepository>();
            services.AddScoped<Operations>();
        });
}