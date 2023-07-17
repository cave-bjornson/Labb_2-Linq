using EntityModels.Data;
using EntityModels.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class TeachersModel : PageModel
{
    private SchoolDbContext db;
    public Teacher[]? Teachers { get; set; }

    public TeachersModel(SchoolDbContext db)
    {
        this.db = db;
    }

    public void OnGet()
    {
        Teachers = db.Teachers
            .Include(t => t.Courses.OrderBy(c => c.Name))
            .OrderBy(t => t.Name)
            .ToArray();
    }
}
