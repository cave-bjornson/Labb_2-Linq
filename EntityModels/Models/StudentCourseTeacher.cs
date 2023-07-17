namespace EntityModels.Models;

public class StudentCourseTeacher
{
    public Guid? StudentId { get; set; }

    public Student Student { get; set; }

    public Guid? CourseId { get; set; }

    public Course Course { get; set; }

    public Guid? TeacherId { get; set; }

    public Teacher Teacher { get; set; }

    // protected bool Equals(StudentCourseTeacher other)
    // {
    //     return Nullable.Equals(StudentId, other.StudentId)
    //         && Nullable.Equals(CourseId, other.CourseId)
    //         && Nullable.Equals(TeacherId, other.TeacherId);
    // }
    //
    // /// <inheritdoc />
    // public override bool Equals(object? obj)
    // {
    //     if (ReferenceEquals(null, obj))
    //         return false;
    //     if (ReferenceEquals(this, obj))
    //         return true;
    //     if (obj.GetType() != this.GetType())
    //         return false;
    //     return Equals((StudentCourseTeacher)obj);
    // }
    //
    // /// <inheritdoc />
    // public override int GetHashCode()
    // {
    //     return HashCode.Combine(StudentId, CourseId, TeacherId);
    // }
}
