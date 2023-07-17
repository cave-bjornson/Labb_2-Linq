using EntityModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EntityModels.Data;

public class SchoolDbContext : DbContext
{
    public DbSet<Clazz> Classes { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<StudentCourseTeacher> StudentCourseTeachers { get; set; }

    public DbSet<CourseTeacher> CourseTeachers { get; set; }

    public DbSet<StudentCourse> StudentCourses { get; set; }

    public DbSet<Student> Students { get; set; }

    public DbSet<Teacher> Teachers { get; set; }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<Guid>().HaveConversion<GuidToStringConverter>();
    }

    /// <inheritdoc />
    protected SchoolDbContext() { }

    /// <inheritdoc />
    public SchoolDbContext(DbContextOptions options)
        : base(options) { }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
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
        }

        // optionsBuilder.LogTo(Console.WriteLine);
        // optionsBuilder.EnableSensitiveDataLogging();
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ;
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Clazz>()
            .HasMany<Student>(c => c.Students)
            .WithOne(s => s.Class)
            .HasForeignKey(s => s.ClassId);

        modelBuilder.Entity<Course>(course =>
        {
            course
                .HasMany<Student>(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity<StudentCourse>();

            course
                .HasMany<Teacher>(c => c.Teachers)
                .WithMany(t => t.Courses)
                .UsingEntity<CourseTeacher>();
        });

        modelBuilder.Entity<Teacher>(teacher =>
        {
            teacher
                .HasMany<Student>(t => t.Students)
                .WithMany(s => s.Teachers)
                .UsingEntity<StudentCourseTeacher>();
        });

        modelBuilder.Entity<CourseTeacher>().HasKey(ct => new { ct.CourseId, ct.TeacherId });

        modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder
            .Entity<StudentCourseTeacher>()
            .HasKey(
                sct =>
                    new
                    {
                        sct.StudentId,
                        sct.CourseId,
                        sct.TeacherId
                    }
            );

        var dataBaseSeeder = new DataSeeder();
        modelBuilder.Entity<Clazz>().HasData(dataBaseSeeder.Classes);
        modelBuilder.Entity<Course>().HasData(dataBaseSeeder.Courses);
        modelBuilder.Entity<Teacher>().HasData(dataBaseSeeder.Teachers);
        modelBuilder.Entity<Student>().HasData(dataBaseSeeder.Students);
        modelBuilder.Entity<StudentCourse>().HasData(dataBaseSeeder.SC);
        modelBuilder.Entity<CourseTeacher>().HasData(dataBaseSeeder.CT);
        modelBuilder.Entity<StudentCourseTeacher>().HasData(dataBaseSeeder.SCT);
    }
}
