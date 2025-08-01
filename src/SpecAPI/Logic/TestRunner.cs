using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;

namespace SpecAPI
{
    public static class TestRunner
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public static async Task RunTests(List<TestCase> testCases)
        {
            foreach (var test in testCases)
            {
                Console.WriteLine($"\n▶️ Running: {test.Name}");

                try
                {
                    var request = new HttpRequestMessage(new HttpMethod(test.Request.Method), test.Request.Url);

                    // Add headers
                    if (test.Request.Headers != null)
                    {
                        foreach (var header in test.Request.Headers)
                        {
                            request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                        }
                    }

                    // Handle authentication
                    if (test.Request.Auth != null)
                    {
                        var auth = test.Request.Auth;

                        if (auth.Type?.Equals("basic", StringComparison.OrdinalIgnoreCase) == true
                            && auth.Username != null && auth.Password != null)
                        {
                            var byteArray = Encoding.ASCII.GetBytes($"{auth.Username}:{auth.Password}");
                            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                        }
                        else if (auth.Type?.Equals("bearer", StringComparison.OrdinalIgnoreCase) == true
                            && auth.Token != null)
                        {
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
                        }
                        else if (auth.Type?.Equals("apikey", StringComparison.OrdinalIgnoreCase) == true
                            && auth.Name != null && auth.Value != null)
                        {
                            if (auth.In?.ToLower() == "query")
                            {
                                var uriBuilder = new UriBuilder(test.Request.Url);
                                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                                query[auth.Name] = auth.Value;
                                uriBuilder.Query = query.ToString();
                                request.RequestUri = uriBuilder.Uri;
                            }
                            else // Default to header
                            {
                                request.Headers.TryAddWithoutValidation(auth.Name, auth.Value);
                            }
                        }
                    }

                    // Set body
                    if (test.Request.Body != null)
                    {
                        var json = JsonSerializer.Serialize(test.Request.Body);
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    var stopwatch = Stopwatch.StartNew();
                    var response = await httpClient.SendAsync(request);
                    stopwatch.Stop();

                    var responseTime = stopwatch.ElapsedMilliseconds;
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Validate status code
                    bool passed = true;
                    if ((int)response.StatusCode != test.Expect.StatusCode)
                    {
                        passed = false;
                    }

                    // Validate body
                    if (test.Expect.Body != null)
                    {
                        try
                        {
                            using var expectedJson = JsonDocument.Parse(JsonSerializer.Serialize(test.Expect.Body));
                            using var actualJson = JsonDocument.Parse(responseContent);

                            if (!JsonCompare.IsMatch(expectedJson.RootElement, actualJson.RootElement))
                            {
                                passed = false;
                            }
                        }
                        catch
                        {
                            passed = false;
                        }
                    }

                    if (passed)
                    {
                        Console.WriteLine($"✅ Test '{test.Name}' passed.");
                    }
                    else
                    {
                        Console.WriteLine($"❌ Test '{test.Name}' failed. Status: {(int)response.StatusCode} Expected: {test.Expect.StatusCode}, Response time: {responseTime}ms");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Test '{test.Name}' error: {ex.Message}");
                }
            }
        }
    }
}
