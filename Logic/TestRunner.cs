using SpecAPI.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SpecAPI.Logic;

public static class TestRunner
{
    public static bool PartialMatch(JsonElement actual, JsonElement expected)
    {
        if (expected.ValueKind != actual.ValueKind)
            return false;

        switch (expected.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (var prop in expected.EnumerateObject())
                {
                    if (!actual.TryGetProperty(prop.Name, out var actualProp))
                        return false;
                    if (!PartialMatch(actualProp, prop.Value))
                        return false;
                }
                return true;

            case JsonValueKind.Array:
                foreach (var expectedItem in expected.EnumerateArray())
                {
                    bool found = false;
                    foreach (var actualItem in actual.EnumerateArray())
                    {
                        if (PartialMatch(actualItem, expectedItem))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        return false;
                }
                return true;

            default:
                return actual.ToString() == expected.ToString();
        }
    }

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
                    using var expectedJsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(test.Expect.Body));
                    using var actualJsonDoc = JsonDocument.Parse(responseBody);

                    bodyPassed = PartialMatch(actualJsonDoc.RootElement, expectedJsonDoc.RootElement);
                }
                catch
                {
                    bodyPassed = test.Expect.Body.ToString()?.Trim() == responseBody.Trim();
                }
            }

            bool headersPassed = true;
            if (test.Expect.Headers != null)
            {
                foreach (var header in test.Expect.Headers)
                {
                    if (!response.Headers.TryGetValues(header.Key, out var values) &&
                        (response.Content?.Headers.TryGetValues(header.Key, out values) != true))
                    {
                        headersPassed = false;
                        break;
                    }

                    if (!values.Contains(header.Value))
                    {
                        headersPassed = false;
                        break;
                    }
                }
            }

            bool timePassed = true;
            if (test.Expect.MaxResponseTimeMs.HasValue)
                timePassed = sw.ElapsedMilliseconds <= test.Expect.MaxResponseTimeMs.Value;

            bool allPassed = statusPassed && bodyPassed && headersPassed && timePassed;

            var msg = allPassed
                ? $"Test '{test.Name}' passed."
                : $"Test '{test.Name}' failed. Status: {(int)response.StatusCode} Expected: {test.Expect.StatusCode}, Response time: {sw.ElapsedMilliseconds}ms";

            if (!allPassed)
            {
                if (!statusPassed) msg += " (Status code mismatch)";
                if (!bodyPassed) msg += " (Body mismatch)";
                if (!headersPassed) msg += " (Header mismatch)";
                if (!timePassed) msg += " (Response time exceeded)";
            }

            return new TestResult { Passed = allPassed, Message = msg };
        }
        catch (Exception ex)
        {
            return new TestResult { Passed = false, Message = $"Test '{test.Name}' error: {ex.Message}" };
        }
    }
}
