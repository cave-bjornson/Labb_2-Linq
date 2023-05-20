using System.Collections;
using Bogus;
using EntityModels.Models;
using Person = EntityModels.Models.Person;

namespace EntityModels.Data;

public class DataSeeder
{
    public readonly IReadOnlyCollection<Clazz> Classes;
    public readonly IReadOnlyCollection<Student> Students;
    public readonly IReadOnlyCollection<Teacher> Teachers;
    public readonly IReadOnlyCollection<Course> Courses;
    public readonly ICollection<CourseTeacher> CT = new HashSet<CourseTeacher>();
    public readonly ICollection<StudentCourseTeacher> SCT = new HashSet<StudentCourseTeacher>();

    public DataSeeder()
    {
        Classes = GenerateClasses();
        Courses = GenerateCourses();
        Teachers = GenerateTeachers(10, Courses, CT);
        Students = GenerateStudents(100, Classes, Courses, Teachers, CT, SCT);
    }

    private static IReadOnlyCollection<Student> GenerateStudents(
        int amount,
        IEnumerable<Clazz> classes,
        IEnumerable<Course> courses,
        IEnumerable<Teacher> teachers,
        ICollection<CourseTeacher> ct,
        ICollection<StudentCourseTeacher> studentCourseTeachers
    )
    {
        var studentFaker = new Faker<Student>(locale: "sv").Rules(
            (f, s) =>
            {
                var courseTeacherSet = new HashSet<CourseTeacher>();

                s.Id = f.Random.Guid();
                s.Name = f.Name.FullName();
                s.ClassId = f.PickRandom(classes).Id;

                for (var i = 1; i <= f.Random.Number(12); i++)
                {
                    courseTeacherSet.Add(f.PickRandom(ct));
                }

                foreach (CourseTeacher courseTeacher in courseTeacherSet)
                {
                    studentCourseTeachers.Add(
                        new StudentCourseTeacher()
                        {
                            StudentId = s.Id,
                            CourseId = courseTeacher.CourseId,
                            TeacherId = courseTeacher.TeacherId
                        }
                    );
                }
            }
        );

        var students = Enumerable.Range(1, amount).Select(i => SeedRow(studentFaker, i)).ToList();

        var p1 = courses.First(c => c.Name == "Programmering 1");
        var t1 = teachers.First(t => t.Name == "Reidar Reidarsson");

        if (
            studentCourseTeachers.FirstOrDefault(
                sct => sct.CourseId == p1.Id && sct.TeacherId == t1.Id
            )
            is null
        )
        {
            var sct = studentCourseTeachers.First();
            sct.CourseId = p1.Id;
            sct.TeacherId = t1.Id;
        }

        return students;
    }

    private static IReadOnlyCollection<Teacher> GenerateTeachers(
        int amount,
        IEnumerable<Course> courses,
        ICollection<CourseTeacher> courseTeachers
    )
    {
        var teacherFaker = new Faker<Teacher>(locale: "sv").Rules(
            (f, t) =>
            {
                var courseSet = new HashSet<Course>();

                t.Id = f.Random.Guid();
                t.Name = f.Name.FullName();
                for (var i = 1; i <= f.Random.Number(6); i++)
                {
                    courseSet.Add(f.PickRandom(courses));
                }
                foreach (Course course in courseSet)
                {
                    courseTeachers.Add(
                        new CourseTeacher { CourseId = course.Id, TeacherId = t.Id }
                    );
                }
            }
        );

        var teachers = Enumerable.Range(1, amount).Select(i => SeedRow(teacherFaker, i)).ToList();

        var t1 = teachers.First();
        var t2 = teachers.Last();
        var p1 = courses.First(c => c.Name == "Programmering 1");
        t1.Name = "Reidar Reidarsson";
        t2.Name = "Tobias Qloksson";

        if (
            courseTeachers.FirstOrDefault(ct => ct.CourseId == p1.Id && ct.TeacherId == t1.Id)
            is null
        )
        {
            courseTeachers.Add(new CourseTeacher { CourseId = p1.Id, TeacherId = t1.Id });
        }
        if (
            courseTeachers.FirstOrDefault(ct => ct.CourseId == p1.Id && ct.TeacherId == t2.Id)
            is null
        )
        {
            courseTeachers.Add(new CourseTeacher { CourseId = p1.Id, TeacherId = t2.Id });
        }

        return teachers;
    }

    private static IReadOnlyCollection<Course> GenerateCourses()
    {
        var courseNames = new[]
        {
            "Programmering",
            "Matematik",
            "Svenska",
            "Historia",
            "Kemi",
            "Fysik"
        };

        var courseNumbers = new[] { 1, 2, 3 };

        var courseWithNumbers = courseNames
            .SelectMany(name => courseNumbers.Select(number => name + " " + number))
            .ToArray();

        var idx = 0;

        var courseFaker = new Faker<Course>().Rules(
            (f, c) =>
            {
                c.Id = f.Random.Guid();
                c.Name = courseWithNumbers[idx++];
            }
        );

        var courses = Enumerable
            .Range(1, courseWithNumbers.Length)
            .Select(i => SeedRow(courseFaker, i))
            .ToList();

        return courses;
    }

    private static IReadOnlyCollection<Clazz> GenerateClasses()
    {
        var numbers = new[] { "1", "2", "3" };
        var letters = new[] { "A", "B", "C" };
        var classNames = numbers
            .SelectMany(number => letters.Select(letter => number + " " + letter))
            .ToArray();

        var idx = 0;

        var classFaker = new Faker<Clazz>().Rules(
            (f, c) =>
            {
                c.Id = f.Random.Guid();
                c.Name = classNames[idx++];
            }
        );

        var classes = Enumerable
            .Range(1, classNames.Length)
            .Select(i => SeedRow(classFaker, i))
            .ToList();

        return classes;
    }

    private static T SeedRow<T>(Faker<T> faker, int rowId)
        where T : class
    {
        var recordRow = faker.UseSeed(rowId).Generate();
        return recordRow;
    }
}
