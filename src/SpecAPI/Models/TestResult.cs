namespace SpecAPI.Models;

public class TestResult
{
    public string TestName { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public int StatusCode { get; set; }
    public int ExpectedStatusCode { get; set; }
    public long ResponseTimeMs { get; set; }
    public string? ResponseBody { get; set; }
    public string? Error { get; set; }
    public string Message { get; set; } = string.Empty;
}
