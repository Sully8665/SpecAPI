using System.Text.Json;
using SpecAPI.Interfaces;
using SpecAPI.Models;

namespace SpecAPI.Loading;

public class JsonTestLoader : ITestLoader
{
    public List<TestCase> Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Test file not found: {filePath}");

        var content = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<List<TestCase>>(content, options) ?? new List<TestCase>();
    }
} 