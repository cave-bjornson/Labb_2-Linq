using EntityModels.Data;
using EntityModels.Models;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Services;

public class ViewModelService
{
    private readonly SchoolDbContext _db;

    public ViewModelService(SchoolDbContext db)
    {
        _db = db;
    }

    public StudentViewModel GetStudent(Guid id, Guid? courseId = null)
    {
        var student = _db.Students.Find(id);

        IQueryable<StudentCourseTeacher> courseTeachers = _db.StudentCourseTeachers
            .Where(sct => sct.StudentId == id)
            .Include(sct => sct.Course)
            .Include(sct => sct.Teacher);

        if (courseId is not null)
        {
            courseTeachers = courseTeachers.Where(sct => sct.CourseId == courseId);
        }

        return student.MapToView(courseTeachers);
    }

    public IEnumerable<StudentViewModel> GetAllStudents(Guid? courseId = null)
    {
        IQueryable<Student> students = _db.Students;

        IQueryable<StudentCourseTeacher> courseTeachers = _db.StudentCourseTeachers
            .Include(sct => sct.Course)
            .Include(sct => sct.Teacher);

        if (courseId is not null)
        {
            students = students.Where(s => s.Courses.Any(c => c.Id == courseId));
            courseTeachers = courseTeachers.Where(sct => sct.CourseId == courseId);
        }

        return students.Select(
            s => s.MapToView(courseTeachers.Where(sct => sct.StudentId == s.Id))
        );
    }

    public CourseViewModel GetCourse(Guid id)
    {
        var course = _db.Courses.Find(id);
        _db.Entry(course).Collection(c => c.Teachers).Load();

        var students = GetAllStudents(id);

        return course.MapToView(students);
    }

    public StudentTeacherChoiceViewModel GetTeacherChoice(Guid studentId)
    {
        var courseTeachers = _db.CourseTeachers
            .Include(ct => ct.Course)
            .Include(ct => ct.Teacher)
            .Select(ct => ct.MapToView())
            .ToList()
            .OrderBy(ct => ct.CourseName)
            .ToList();

        var student = GetStudent(studentId);

        var teacherChoices = new Dictionary<string, bool>();

        foreach (var ct in courseTeachers)
        {
            teacherChoices[ct.CourseId + ct.TeacherId.ToString()] = student.CourseTeachers.Any(
                sct => sct.CourseId == ct.CourseId && sct.TeacherId == ct.TeacherId
            );
        }

        return new StudentTeacherChoiceViewModel(student, teacherChoices);
    }

    public IEnumerable<CourseTeacherChoiceViewModel> GetTeacherChoices(Guid studentId)
    {
        var studentCourses = _db.Courses
            .Include(c => c.Students)
            .Where(c => c.Students.Any(s => s.Id == studentId));

        var courseTeachers = _db.CourseTeachers
            .Where(ct => studentCourses.Any(sc => sc.Id == ct.CourseId))
            .Include(ct => ct.Course)
            .Include(ct => ct.Teacher)
            .AsEnumerable()
            .Select(ct => ct.MapToView());

        var studentsCourseTeachers = _db.StudentCourseTeachers
            .Where(sct => sct.StudentId == studentId)
            .Include(sct => sct.Course)
            .Include(sct => sct.Teacher)
            .AsEnumerable()
            .Select(sct => sct.MapToView());

        return courseTeachers
            .Select(
                ct =>
                    new CourseTeacherChoiceViewModel
                    {
                        // ChoiceId = ct.CourseId + ct.CourseId.ToString(),
                        CourseTeacher = ct,
                        Enabled = studentsCourseTeachers.Contains(ct)
                    }
            )
            .ToList();
    }

    public void UpdateTeacherChoices(
        Guid studentId,
        IEnumerable<CourseTeacherChoiceViewModel> teacherChoices
    )
    {
        var studentCourses = _db.StudentCourseTeachers.Where(sct => sct.StudentId == studentId);

        var courseTeacherChoiceViewModels = teacherChoices.ToList();
        var additions = (
            from choice in courseTeacherChoiceViewModels.Where(tc => tc.Enabled)
            where
                !studentCourses.Any(
                    sct =>
                        sct.CourseId == choice.CourseTeacher.CourseId
                        && sct.TeacherId == choice.CourseTeacher.TeacherId
                )
            select new StudentCourseTeacher()
            {
                StudentId = studentId,
                CourseId = choice.CourseTeacher.CourseId,
                TeacherId = choice.CourseTeacher.TeacherId
            }
        ).ToList();

        var subtractions = (
            from choice in courseTeacherChoiceViewModels.Where(tc => tc.Enabled == false)
            where
                studentCourses.Any(
                    sct =>
                        sct.CourseId == choice.CourseTeacher.CourseId
                        && sct.TeacherId == choice.CourseTeacher.TeacherId
                )
            select new StudentCourseTeacher()
            {
                StudentId = studentId,
                CourseId = choice.CourseTeacher.CourseId,
                TeacherId = choice.CourseTeacher.TeacherId
            }
        ).ToList();

        _db.StudentCourseTeachers.AddRange(additions);
        _db.StudentCourseTeachers.RemoveRange(subtractions);
        _db.SaveChanges();
    }
}
