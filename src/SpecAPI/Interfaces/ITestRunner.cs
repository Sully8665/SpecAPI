using SpecAPI.Models;

namespace SpecAPI.Interfaces;

public interface ITestRunner
{
    Task<List<TestResult>> RunTestsAsync(List<TestCase> testCases);
} 