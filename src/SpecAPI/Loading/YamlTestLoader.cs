using SpecAPI.Interfaces;
using SpecAPI.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SpecAPI.Loading;

public class YamlTestLoader : ITestLoader
{
    public List<TestCase> Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Test file not found: {filePath}");

        var content = File.ReadAllText(filePath);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        return deserializer.Deserialize<List<TestCase>>(content) ?? new List<TestCase>();
    }
} 