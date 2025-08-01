public class Request
{
    public string Method { get; set; } = "GET";
    public string Url { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    public object? Body { get; set; }
}