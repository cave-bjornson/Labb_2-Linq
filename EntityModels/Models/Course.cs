namespace EntityModels.Models;

public class Course
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }

    public ICollection<Student> Students { get; init; } = new List<Student>();

    public ICollection<Teacher> Teachers { get; init; } = new List<Teacher>();
}