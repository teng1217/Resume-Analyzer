using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string ResumeText { get; set; }

    [BindProperty]
    public string JobDescription { get; set; }

    public IActionResult OnPost()
    {
        TempData["ResumeText"] = ResumeText;
        TempData["JobDescription"] = JobDescription;
        return RedirectToPage("Result");
    }
}
