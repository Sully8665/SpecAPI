using System.Diagnostics;
using System.Text;
using System.Text.Json;
using SpecAPI.Authentication;
using SpecAPI.Interfaces;
using SpecAPI.Models;
using SpecAPI.Validation;

namespace SpecAPI.Running;

public class HttpTestRunner : ITestRunner
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationManager _authManager;
    private readonly ValidationManager _validationManager;

    public HttpTestRunner(
        HttpClient httpClient,
        AuthenticationManager authManager,
        ValidationManager validationManager)
    {
        _httpClient = httpClient;
        _authManager = authManager;
        _validationManager = validationManager;
    }

    public async Task<List<TestResult>> RunTestsAsync(List<TestCase> testCases)
    {
        var results = new List<TestResult>();

        foreach (var testCase in testCases)
        {
            Console.WriteLine($"\n▶️ Running: {testCase.Name}");
            var result = await RunSingleTestAsync(testCase);
            results.Add(result);
        }

        return results;
    }

    private async Task<TestResult> RunSingleTestAsync(TestCase testCase)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var request = CreateHttpRequest(testCase.Request);
            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            stopwatch.Stop();

            var result = new TestResult
            {
                TestName = testCase.Name,
                StatusCode = (int)response.StatusCode,
                ResponseBody = responseContent,
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                ExpectedStatusCode = testCase.Expect.StatusCode,
                Passed = _validationManager.Validate(new TestResult
                {
                    StatusCode = (int)response.StatusCode,
                    ResponseBody = responseContent,
                    ResponseTimeMs = stopwatch.ElapsedMilliseconds
                }, testCase.Expect)
            };

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new TestResult
            {
                TestName = testCase.Name,
                StatusCode = 0,
                ResponseTimeMs = stopwatch.ElapsedMilliseconds,
                ExpectedStatusCode = testCase.Expect.StatusCode,
                Passed = false,
                Error = ex.Message
            };
        }
    }

    private HttpRequestMessage CreateHttpRequest(Request request)
    {
        var httpRequest = new HttpRequestMessage(new HttpMethod(request.Method), request.Url);

        // Add headers
        if (request.Headers != null)
        {
            foreach (var header in request.Headers)
            {
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        // Apply authentication
        _authManager.ApplyAuthentication(httpRequest, request.Auth);

        // Set body
        if (request.Body != null)
        {
            var json = JsonSerializer.Serialize(request.Body);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        return httpRequest;
    }
} 