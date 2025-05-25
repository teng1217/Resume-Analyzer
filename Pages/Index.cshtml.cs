using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UglyToad.PdfPig;
using System.Text;

public class IndexModel : PageModel
{
    [BindProperty]
    public string ResumeText { get; set; }

    [BindProperty]
    public string JobDescription { get; set; }

    [BindProperty]
    public IFormFile ResumeFile { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        string text = ResumeText;

        if (ResumeFile != null && ResumeFile.Length > 0)
        {
            using var stream = ResumeFile.OpenReadStream();
            using var pdf = PdfDocument.Open(stream);
            var sb = new StringBuilder();
            foreach (var page in pdf.GetPages())
            {
                sb.AppendLine(page.Text);
            }
            text = sb.ToString();
        }

        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(JobDescription))
        {
            TempData["ErrorMessage"] = "請提供履歷內容或職缺說明。";
            return Page();
        }

        TempData["ResumeText"] = text;
        TempData["JobDescription"] = JobDescription;
        return RedirectToPage("Result");
    }
}
