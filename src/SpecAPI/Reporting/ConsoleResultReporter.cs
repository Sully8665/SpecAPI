using SpecAPI.Interfaces;
using SpecAPI.Models;

namespace SpecAPI.Reporting;

public class ConsoleResultReporter : IResultReporter
{
    public void ReportResults(List<TestResult> results)
    {
        foreach (var result in results)
        {
            if (result.Passed)
            {
                Console.WriteLine($"✅ Test '{result.TestName}' passed.");
            }
            else
            {
                Console.WriteLine($"❌ Test '{result.TestName}' failed. Status: {result.StatusCode} Expected: {result.ExpectedStatusCode}, Response time: {result.ResponseTimeMs}ms");
                if (!string.IsNullOrEmpty(result.Error))
                {
                    Console.WriteLine($"   Error: {result.Error}");
                }
            }
        }
    }
} 