using EntityModels.Data;
using EntityModels.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages;

public class StudentsModel : PageModel
{
    private SchoolDbContext db;
    private ViewModelService _service;

    public StudentsModel(SchoolDbContext db, ViewModelService service)
    {
        this.db = db;
        _service = service;
    }

    public StudentViewModel[]? Students { get; set; }

    public void OnGet()
    {
        ViewData["Title"] = "Students";

        Students = _service.GetAllStudents().ToArray();
    }
}
