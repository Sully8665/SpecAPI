using SpecAPI.Models;
using System.Text.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SpecAPI.Logic;

public static class TestLoader
{
    public static List<TestCase> Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Test file not found: {filePath}");

        var ext = Path.GetExtension(filePath).ToLower();
        var content = File.ReadAllText(filePath);

        return ext switch
        {
            ".yaml" or ".yml" => LoadYaml(content),
            ".json" => LoadJson(content),
            _ => throw new NotSupportedException("Only .yaml, .yml and .json files are supported.")
        };
    }

    private static List<TestCase> LoadYaml(string content)
    {
        var deserializer = new DeserializerBuilder()
    .WithNamingConvention(CamelCaseNamingConvention.Instance)
    .IgnoreUnmatchedProperties()
    .Build();

        return deserializer.Deserialize<List<TestCase>>(content) ?? new List<TestCase>();
    }

    private static List<TestCase> LoadJson(string content)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<List<TestCase>>(content, options) ?? new List<TestCase>();
    }
}
