namespace SpecAPI.Models;

public class ApiTestResult
{
    public string Name { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public int StatusCode { get; set; }
    public long ResponseTimeMs { get; set; }
    public string? Error { get; set; }
    public string? BodyPreview { get; set; }
}
