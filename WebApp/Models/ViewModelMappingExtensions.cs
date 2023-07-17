using EntityModels.Models;

namespace WebApp.Models;

public static class ViewModelMappingExtensions
{
    public static StudentViewModel MapToView(
        this Student student,
        IEnumerable<StudentCourseTeacher> studentCourseTeachers
    )
    {
        var courseTeachers = studentCourseTeachers.Select(sct => sct.MapToView()).ToList();

        return new StudentViewModel(
            StudentId: student.Id,
            StudentName: student.Name,
            CourseTeachers: courseTeachers
        );
    }

    public static CourseTeacherViewModel MapToView(this StudentCourseTeacher sct)
    {
        return new CourseTeacherViewModel(
            CourseId: sct.CourseId,
            CourseName: sct.Course.Name,
            TeacherId: sct.TeacherId,
            TeacherName: sct.Teacher.Name
        );
    }

    public static CourseTeacherViewModel MapToView(this CourseTeacher ct)
    {
        return new CourseTeacherViewModel(
            CourseId: ct.CourseId,
            CourseName: ct.Course.Name,
            TeacherId: ct.TeacherId,
            TeacherName: ct.Teacher.Name
        );
    }

    public static CourseViewModel MapToView(
        this Course course,
        IEnumerable<StudentViewModel> students
    )
    {
        return new CourseViewModel(
            CourseId: course.Id,
            CourseName: course.Name,
            Teachers: course.Teachers,
            Students: students
        );
    }
}
