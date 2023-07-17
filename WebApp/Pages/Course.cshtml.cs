using EntityModels.Data;
using EntityModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages;

public class CourseModel : PageModel
{
    private SchoolDbContext db;
    private ViewModelService _service;

    public CourseModel(SchoolDbContext db, ViewModelService service)
    {
        this.db = db;
        _service = service;
    }

    [BindProperty]
    public string CourseName { get; set; } = string.Empty;

    public CourseViewModel? CurrentCourse { get; set; }

    // [BindProperty(SupportsGet = true)]
    // public Guid Id { get; set; }

    public void OnGet(Guid id)
    {
        CurrentCourse = _service.GetCourse(id);

        CourseName = CurrentCourse.CourseName;
    }

    public IActionResult OnPost(Guid id)
    {
        if (ModelState.IsValid)
        {
            var course = db.Courses.Find(id);

            if (CourseName != course.Name)
            {
                course.Name = CourseName;
                db.SaveChanges();
            }
        }

        return RedirectToPage(new { id });
    }
}
