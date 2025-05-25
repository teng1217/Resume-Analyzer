using System.Text;
using System.Text.Json;

public class OpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "8ljsbXU19Uiw7Gt7iCnBGivld2J5vOwsohGG0t6FRA6upJ27iPL2JQQJ99BEACHYHv6XJ3w3AAAAACOGpRQ8";
    private readonly string _endpoint = "https://tenge-mb39qyqo-eastus2.cognitiveservices.azure.com/openai/deployments/resume-gpt/chat/completions?api-version=2025-01-01-preview";

    public OpenAIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);
    }

    private async Task<string> CallOpenAIAsync(string systemPrompt, string userPrompt)
    {
        var requestBody = new
        {
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            },
            max_tokens = 500,
            temperature = 0.7
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_endpoint, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(responseJson);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
    }

    public Task<string> GetSummaryAsync(string resume) =>
        CallOpenAIAsync("請將以下履歷內容摘要成一段簡短介紹：", resume);

    public Task<string> GetSkillsAsync(string resume) =>
        CallOpenAIAsync("請從以下履歷內容中擷取出技能清單：", resume);

    public Task<string> GetSuggestionsAsync(string resume, string job) =>
        CallOpenAIAsync("根據以下職缺與履歷，請給出三點具體的修改建議：\n職缺：" + job, resume);

    public async Task<int> GetResumeScoreAsync(string resume, string job)
    {
        var result = await CallOpenAIAsync("根據職缺與履歷內容，請給這份履歷與該職缺的匹配度打分（0-100且說明原因）：\n職缺：" + job, resume);
        var match = System.Text.RegularExpressions.Regex.Match(result, @"\d+");
        return match.Success ? int.Parse(match.Value) : 0;
    }
}
