using SpecAPI.Models;
using SpecAPI.Services;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;

class Program
{
    static async Task Main(string[] args)
    {
        var inputFile = "Input/sample-test.json";
        var schemaFile = "Input/spec-schema.json";

        // Read test file
        var json = await File.ReadAllTextAsync(inputFile);

        // Read and parse JSON Schema
        var schemaJson = await File.ReadAllTextAsync(schemaFile);
        JSchema schema = JSchema.Parse(schemaJson);

        // Parse test file as JObject
        JObject jsonObj = JObject.Parse(json);

        // Validate test file against schema
        if (!jsonObj.IsValid(schema, out IList<string> errors))
        {
            Console.WriteLine("❌ Validation errors in test spec file:");
            foreach (var err in errors)
                Console.WriteLine($" - {err}");
            return;
        }

        // Deserialize JSON into models
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

        // Create Output directory if it doesn't exist
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
    }
}
