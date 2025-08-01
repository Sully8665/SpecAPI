using SpecAPI.Models;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SpecAPI.Services;

public class ApiTestRunner
{
    private readonly HttpClient _client = new();

    public async Task<List<ApiTestResult>> RunAsync(ApiTestSpec spec)
    {
        var results = new List<ApiTestResult>();

        foreach (var test in spec.Tests)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var request = new HttpRequestMessage(
                    new HttpMethod(test.Method), ReplaceVars(test.Url, spec.Variables)
                );

                if (test.Headers != null)
                {
                    foreach (var header in test.Headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, ReplaceVars(header.Value, spec.Variables));
                    }
                }

                if (test.Body != null)
                {
                    string jsonBody = JsonSerializer.Serialize(test.Body);
                    request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                }

                var response = await _client.SendAsync(request);
                var body = await response.Content.ReadAsStringAsync();
                sw.Stop();

                var passed = (int)response.StatusCode == test.Expect.Status &&
                             (string.IsNullOrWhiteSpace(test.Expect.BodyContains) || body.Contains(test.Expect.BodyContains));

                results.Add(new ApiTestResult
                {
                    Name = test.Name,
                    Passed = passed,
                    StatusCode = (int)response.StatusCode,
                    ResponseTimeMs = sw.ElapsedMilliseconds,
                    BodyPreview = body.Length > 200 ? body[..200] + "..." : body
                });
            }
            catch (Exception ex)
            {
                sw.Stop();
                results.Add(new ApiTestResult
                {
                    Name = test.Name,
                    Passed = false,
                    Error = ex.Message,
                    StatusCode = 0,
                    ResponseTimeMs = sw.ElapsedMilliseconds
                });
            }
        }

        return results;
    }

    private string ReplaceVars(string input, Dictionary<string, string>? vars)
    {
        if (vars == null) return input;
        foreach (var kv in vars)
        {
            input = input.Replace($"{{{{{kv.Key}}}}}", kv.Value);
        }
        return input;
    }
}
