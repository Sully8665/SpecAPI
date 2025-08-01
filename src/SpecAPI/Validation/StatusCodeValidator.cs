using SpecAPI.Interfaces;
using SpecAPI.Models;

namespace SpecAPI.Validation;

public class StatusCodeValidator : IValidator
{
    public bool Validate(TestResult result, Expect expectations)
    {
        return result.StatusCode == expectations.StatusCode;
    }
} 