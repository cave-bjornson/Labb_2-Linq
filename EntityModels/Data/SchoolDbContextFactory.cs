using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EntityModels.Data;

public class SchoolDbContextFactory : IDesignTimeDbContextFactory<SchoolDbContext>
{
    /// <inheritdoc />
    public SchoolDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SchoolDbContext>();
        string dir = Environment.CurrentDirectory;
        string path = string.Empty;

        if (dir.EndsWith("net7.0"))
        {
            path = Path.Combine("..", "..", "..", "..", "app.db");
        }
        else
        {
            path = Path.Combine("..", "app.db");
        }

        optionsBuilder.UseSqlite($"DataSource={path};Cache=Shared");

        return new SchoolDbContext(optionsBuilder.Options);
    }
}
