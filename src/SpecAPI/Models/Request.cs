public class Auth
{
    public string Type { get; set; } = ""; // basic, bearer, apikey
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Token { get; set; }
    public string? In { get; set; }  // header or query (for API key)
    public string? Name { get; set; } // e.g., X-API-Key
    public string? Value { get; set; }
}

public class Request
{
    public string Method { get; set; } = "GET";
    public string Url { get; set; } = "";
    public Dictionary<string, string>? Headers { get; set; }
    public object? Body { get; set; }
    public Auth? Auth { get; set; }
}
