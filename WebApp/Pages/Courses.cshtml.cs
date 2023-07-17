using EntityModels.Data;
using EntityModels.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class CoursesModel : PageModel
{
    private SchoolDbContext db;

    public CoursesModel(SchoolDbContext db)
    {
        this.db = db;
    }

    public IEnumerable<Course>? Courses { get; set; }

    public void OnGet()
    {
        Courses = db.Courses.OrderBy(c => c.Name).Include(c => c.Teachers).Include(c => c.Students);
    }
}
