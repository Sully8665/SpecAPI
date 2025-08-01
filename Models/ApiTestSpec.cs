namespace SpecAPI.Models;

public class ApiTestSpec
{
    public Dictionary<string, string>? Variables { get; set; }
    public List<ApiTestCase> Tests { get; set; } = new();
}

public class ApiTestCase
{
    public string Name { get; set; } = string.Empty;
    public string Method { get; set; } = "GET";
    public string Url { get; set; } = string.Empty;
    public Dictionary<string, string>? Headers { get; set; }
    public object? Body { get; set; }
    public ApiExpectations Expect { get; set; } = new();
}

public class ApiExpectations
{
    public int Status { get; set; }
    public string? BodyContains { get; set; }
}
