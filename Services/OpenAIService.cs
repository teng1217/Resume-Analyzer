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
        CallOpenAIAsync("�бN�H�U�i�����e�K�n���@�q²�u���СG", resume);

    public Task<string> GetSkillsAsync(string resume) =>
        CallOpenAIAsync("�бq�H�U�i�����e���^���X�ޯ�M��G", resume);

    public Task<string> GetSuggestionsAsync(string resume, string job) =>
        CallOpenAIAsync("�ھڥH�U¾�ʻP�i���A�е��X�T�I���骺�ק��ĳ�G\n¾�ʡG" + job, resume);

    public async Task<int> GetResumeScoreAsync(string resume, string job)
    {
        var result = await CallOpenAIAsync("�ھ�¾�ʻP�i�����e�A�е��o���i���P��¾�ʪ��ǰt�ץ����]0-100�B������]�^�G\n¾�ʡG" + job, resume);
        var match = System.Text.RegularExpressions.Regex.Match(result, @"\d+");
        return match.Success ? int.Parse(match.Value) : 0;
    }
}
