using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("❌ Please provide the path to the YAML test file.");
            return;
        }

        string inputFile = args[0];
        if (!File.Exists(inputFile))
        {
            Console.WriteLine($"❌ File not found: {inputFile}");
            return;
        }

        string yaml = await File.ReadAllTextAsync(inputFile);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();

        var testCases = deserializer.Deserialize<List<TestCase>>(yaml);

        Directory.CreateDirectory("Output");
        string resultFile = Path.Combine("Output", "result.md");
        var sb = new StringBuilder();
        sb.AppendLine("# Test Results\n");

        var client = new HttpClient();

        foreach (var test in testCases)
        {
            Console.WriteLine($"▶️ Running test: {test.Name}");

            var request = new HttpRequestMessage(new HttpMethod(test.Request.Method), test.Request.Url);

            if (test.Request.Headers != null)
            {
                foreach (var header in test.Request.Headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (test.Request.Body != null)
            {
                string jsonBody = JsonSerializer.Serialize(test.Request.Body);
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();
            var response = await client.SendAsync(request);
            sw.Stop();

            var statusCode = (int)response.StatusCode;
            bool statusMatch = statusCode == test.Expect.Status;

            var responseBody = await response.Content.ReadAsStringAsync();

            bool bodyMatch = true;
            if (test.Expect.Body != null)
            {
                try
                {
                    var expectedJson = JsonSerializer.Serialize(test.Expect.Body);
                    bodyMatch = CompareJsonBodies(responseBody, expectedJson);
                }
                catch
                {
                    bodyMatch = false;
                }
            }

            bool headersMatch = true;
            if (test.Expect.Headers != null)
            {
                foreach (var expectedHeader in test.Expect.Headers)
                {
                    if (!response.Headers.TryGetValues(expectedHeader.Key, out var values) ||
                        !values.Contains(expectedHeader.Value))
                    {
                        headersMatch = false;
                        break;
                    }
                }
            }

            bool timeMatch = test.Expect.MaxResponseTimeMs == null || sw.ElapsedMilliseconds <= test.Expect.MaxResponseTimeMs;

            bool passed = statusMatch && bodyMatch && headersMatch && timeMatch;

            string resultText = passed ? "✅ PASSED" : "❌ FAILED";

            Console.WriteLine($"{resultText} - {test.Name} [{sw.ElapsedMilliseconds}ms]");

            sb.AppendLine($"## {test.Name}");
            sb.AppendLine($"- **URL**: {test.Request.Url}");
            sb.AppendLine($"- **Method**: {test.Request.Method}");
            sb.AppendLine($"- **Expected Status**: {test.Expect.Status}");
            sb.AppendLine($"- **Actual Status**: {statusCode}");
            sb.AppendLine($"- **Response Time**: {sw.ElapsedMilliseconds}ms");
            sb.AppendLine($"- **Result**: {resultText}");
            sb.AppendLine();
        }

        await File.WriteAllTextAsync(resultFile, sb.ToString());
        Console.WriteLine($"📄 Test results saved to {resultFile}");

    }

    static bool CompareJsonBodies(string actualJson, string expectedJson)
    {
        try
        {
            using var actualDoc = JsonDocument.Parse(actualJson);
            using var expectedDoc = JsonDocument.Parse(expectedJson);

            return JsonElementDeepEquals(actualDoc.RootElement, expectedDoc.RootElement);
        }
        catch
        {
            return false;
        }
    }

    static bool JsonElementDeepEquals(JsonElement actual, JsonElement expected)
    {
        if (actual.ValueKind != expected.ValueKind)
            return false;

        switch (actual.ValueKind)
        {
            case JsonValueKind.Object:
                {
                    var actualProps = actual.EnumerateObject();
                    var expectedProps = expected.EnumerateObject();

                    var actualDict = new Dictionary<string, JsonElement>();
                    foreach (var prop in actualProps)
                        actualDict[prop.Name] = prop.Value;

                    foreach (var expectedProp in expectedProps)
                    {
                        if (!actualDict.TryGetValue(expectedProp.Name, out var actualValue))
                            return false;
                        if (!JsonElementDeepEquals(actualValue, expectedProp.Value))
                            return false;
                    }
                    return true;
                }
            case JsonValueKind.Array:
                {
                    var actualArr = actual.EnumerateArray();
                    var expectedArr = expected.EnumerateArray();

                    var actualList = new List<JsonElement>(actualArr);
                    var expectedList = new List<JsonElement>(expectedArr);

                    if (actualList.Count != expectedList.Count)
                        return false;

                    for (int i = 0; i < actualList.Count; i++)
                        if (!JsonElementDeepEquals(actualList[i], expectedList[i]))
                            return false;

                    return true;
                }
            default:
                return actual.ToString() == expected.ToString();
        }
    }
}
