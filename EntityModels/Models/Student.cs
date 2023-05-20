namespace EntityModels.Models;

public class Student : Person
{
    public Guid? ClassId { get; set; }
    public Clazz? Class { get; set; }

    public ICollection<Course> Courses { get; init; } = new List<Course>();

    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
