using SpecAPI.DependencyInjection;
using SpecAPI.Interfaces;
using SpecAPI.Loading;
using SpecAPI.Models;

namespace SpecAPI.Services;

public class TestExecutionService
{
    private readonly ITestRunner _testRunner;
    private readonly TestLoaderFactory _loaderFactory;
    private readonly IResultReporter _resultReporter;

    public TestExecutionService(
        ITestRunner testRunner,
        TestLoaderFactory loaderFactory,
        IResultReporter resultReporter)
    {
        _testRunner = testRunner;
        _loaderFactory = loaderFactory;
        _resultReporter = resultReporter;
    }

    public async Task ExecuteTestsAsync(string filePath)
    {
        // Load test cases
        var loader = _loaderFactory.GetLoader(filePath);
        var testCases = loader.Load(filePath);

        Console.WriteLine($"✅ Loaded {testCases.Count} test case(s).\n");

        // Run tests
        var results = await _testRunner.RunTestsAsync(testCases);

        // Report results
        _resultReporter.ReportResults(results);
    }
} 