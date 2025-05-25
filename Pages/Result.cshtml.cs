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
            ErrorMessage = "? 無法取得履歷或職缺內容，請返回重新上傳。";
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
            ErrorMessage = $"? 發生錯誤：{ex.Message}";
        }
    }
}
