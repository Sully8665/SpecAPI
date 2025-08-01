using SpecAPI.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SpecAPI.Logic;

public static class TestRunner
{
    public static async Task<TestResult> RunTestCase(TestCase test)
    {
        using var client = new HttpClient();
        try
        {
            var request = new HttpRequestMessage(new HttpMethod(test.Request.Method), test.Request.Url);

            if (test.Request.Headers != null)
            {
                foreach (var header in test.Request.Headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (test.Request.Body != null)
                request.Content = new StringContent(test.Request.Body.ToString() ?? "", Encoding.UTF8, "application/json");

            var sw = Stopwatch.StartNew();
            var response = await client.SendAsync(request);
            sw.Stop();

            var responseBody = await response.Content.ReadAsStringAsync();

            bool statusPassed = ((int)response.StatusCode) == test.Expect.StatusCode;

            bool bodyPassed = true;
            if (test.Expect.Body != null)
            {
                try
                {
                    var expectedJson = JsonSerializer.Serialize(test.Expect.Body);
                    bodyPassed = expectedJson == responseBody;
                }
                catch
                {
                    bodyPassed = test.Expect.Body.ToString()?.Trim() == responseBody.Trim();
                }
            }

            bool timePassed = true;
            if (test.Expect.MaxResponseTimeMs.HasValue)
                timePassed = sw.ElapsedMilliseconds <= test.Expect.MaxResponseTimeMs.Value;

            bool allPassed = statusPassed && bodyPassed && timePassed;

            var msg = allPassed
                ? $"Test '{test.Name}' passed."
                : $"Test '{test.Name}' failed. Status: {(int)response.StatusCode} Expected: {test.Expect.StatusCode}, Response time: {sw.ElapsedMilliseconds}ms";

            return new TestResult { Passed = allPassed, Message = msg };
        }
        catch (Exception ex)
        {
            return new TestResult { Passed = false, Message = $"Test '{test.Name}' error: {ex.Message}" };
        }
    }
}
