using SpecAPI.Models;

namespace SpecAPI.Interfaces;

public interface IResultReporter
{
    void ReportResults(List<TestResult> results);
} 