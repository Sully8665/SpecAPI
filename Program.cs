using SpecAPI.Models;
using SpecAPI.Services;
using System.Text.Json;
using System.IO;

var json = await File.ReadAllTextAsync("Input/sample-test.json");
var spec = JsonSerializer.Deserialize<ApiTestSpec>(json, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});

if (spec == null)
{
    Console.WriteLine("❌ Failed to parse test file.");
    return;
}

var runner = new ApiTestRunner();
var results = await runner.RunAsync(spec);

var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Output");
if (!Directory.Exists(outputDir))
{
    Directory.CreateDirectory(outputDir);
}

foreach (var r in results)
{
    Console.WriteLine($"{(r.Passed ? "✅" : "❌")} {r.Name} - Status: {r.StatusCode} in {r.ResponseTimeMs}ms");
}

await File.WriteAllTextAsync(Path.Combine(outputDir, "result.md"), ResultExporter.ToMarkdown(results));
await File.WriteAllTextAsync(Path.Combine(outputDir, "result.html"), ResultExporter.ToHtml(results));
