namespace EntityModels.Models;

public class Teacher : Person
{
    public ICollection<Course> Courses { get; init; } = new List<Course>();

    public ICollection<Student> Students { get; init; } = new List<Student>();
}
