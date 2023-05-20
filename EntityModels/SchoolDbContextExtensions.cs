using EntityModels.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EntityModels;

public static class SchoolDbContextExtensions
{
    public static IServiceCollection AddSchoolDbContext(this IServiceCollection services, string relativePath = "..")
    {
        string databasePath = Path.Combine(relativePath, "app.db");

        services.AddDbContext<SchoolDbContext>(options =>
        {
            options.UseSqlite($"DataSource={relativePath};Cache=Shared");

            options.LogTo(Console.WriteLine,
                new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
        });

        return services;
    }
}