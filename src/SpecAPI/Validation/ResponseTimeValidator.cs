using SpecAPI.Interfaces;
using SpecAPI.Models;

namespace SpecAPI.Validation;

public class ResponseTimeValidator : IValidator
{
    public bool Validate(TestResult result, Expect expectations)
    {
        if (expectations.MaxResponseTimeMs == null) return true;
        return result.ResponseTimeMs <= expectations.MaxResponseTimeMs;
    }
} 