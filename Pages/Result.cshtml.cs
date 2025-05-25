using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ResultModel : PageModel
{
    private readonly OpenAIService _openAI;

    public ResultModel(OpenAIService openAI)
    {
        _openAI = openAI;
    }

    public string Summary { get; set; }
    public string Skills { get; set; }
    public string Suggestions { get; set; }
    public int ResumeScore { get; set; }
    public string ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        var resume = TempData["ResumeText"]?.ToString();
        var job = TempData["JobDescription"]?.ToString();

        if (string.IsNullOrEmpty(resume) || string.IsNullOrEmpty(job))
        {
            ErrorMessage = "? �L�k���o�i����¾�ʤ��e�A�Ъ�^���s�W�ǡC";
            return;
        }

        try
        {
            Summary = await _openAI.GetSummaryAsync(resume);
            Skills = await _openAI.GetSkillsAsync(resume);
            Suggestions = await _openAI.GetSuggestionsAsync(resume, job);
            ResumeScore = await _openAI.GetResumeScoreAsync(resume, job);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"? �o�Ϳ��~�G{ex.Message}";
        }
    }
}
