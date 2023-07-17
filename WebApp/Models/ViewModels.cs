using System.Collections;
using EntityModels.Models;

namespace WebApp.Models;

public record StudentViewModel(
    Guid? StudentId,
    string StudentName,
    ICollection<CourseTeacherViewModel> CourseTeachers
);

public record CourseTeacherViewModel(
    Guid? CourseId,
    string CourseName,
    Guid? TeacherId,
    string TeacherName
);

public record CourseViewModel(
    Guid? CourseId,
    string CourseName,
    IEnumerable<Teacher> Teachers,
    IEnumerable<StudentViewModel> Students
);

public record StudentTeacherChoiceViewModel(
    StudentViewModel Student,
    Dictionary<string, bool> TeacherChoice
);

public class CourseTeacherChoiceViewModel
{
    // public required string ChoiceId { get; set; }
    public required CourseTeacherViewModel CourseTeacher { get; set; }
    public bool Enabled { get; set; }
}
