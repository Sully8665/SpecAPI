using SpecAPI.Interfaces;

namespace SpecAPI.Loading;

public class TestLoaderFactory
{
    private readonly Dictionary<string, ITestLoader> _loaders;

    public TestLoaderFactory()
    {
        _loaders = new Dictionary<string, ITestLoader>(StringComparer.OrdinalIgnoreCase)
        {
            { ".yaml", new YamlTestLoader() },
            { ".yml", new YamlTestLoader() },
            { ".json", new JsonTestLoader() }
        };
    }

    public ITestLoader GetLoader(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        if (_loaders.TryGetValue(extension, out var loader))
        {
            return loader;
        }

        throw new NotSupportedException($"File format not supported: {extension}. Supported formats: {string.Join(", ", _loaders.Keys)}");
    }
} 