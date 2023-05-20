namespace EntityModels.Models;

public class Clazz
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }
    
    public ICollection<Student> Students { get; set; }
}