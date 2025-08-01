using YamlDotNet.Serialization;

public class Expect
{
    [YamlMember(Alias = "statusCode")]
    public int StatusCode { get; set; } = 200;

    [YamlMember(Alias = "body")]
    public object? Body { get; set; }

    [YamlMember(Alias = "headers")]
    public Dictionary<string, string>? Headers { get; set; }
    public int? MaxResponseTimeMs { get; set; }
}