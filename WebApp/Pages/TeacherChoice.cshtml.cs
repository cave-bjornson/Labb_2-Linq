using System.ComponentModel;
using System.Globalization;
using EntityModels.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages;

public class TeacherChoicePageModel : PageModel
{
    private SchoolDbContext _db;
    private ViewModelService _service;
    private ILogger<TeacherChoicePageModel> _logger;

    public TeacherChoicePageModel(
        ViewModelService service,
        ILogger<TeacherChoicePageModel> logger,
        SchoolDbContext db
    )
    {
        _service = service;
        _logger = logger;
        _db = db;
    }

    public string StudentName;

    [BindProperty]
    public List<CourseTeacherChoiceViewModel> TeacherChoices { get; set; }

    public void OnGet(Guid id)
    {
        StudentName = _db.Students.Find(id).Name;
        TeacherChoices = _service
            .GetTeacherChoices(id)
            .OrderBy(tc => tc.CourseTeacher.CourseName)
            .ToList();

        _logger.LogInformation("Choices {Choices}", TeacherChoices.Dump());
    }

    public IActionResult OnPost(Guid id)
    {
        _logger.LogInformation("Choices {Choices}", TeacherChoices.Dump());

        _service.UpdateTeacherChoices(id, TeacherChoices);
        return RedirectToPage(new { id });
    }
}
