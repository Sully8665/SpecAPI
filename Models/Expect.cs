public class Expect
{
    public int Status { get; set; }
    public Dictionary<string, string>? Headers { get; set; }
    public object? Body { get; set; }
    public int? MaxResponseTimeMs { get; set; }
}