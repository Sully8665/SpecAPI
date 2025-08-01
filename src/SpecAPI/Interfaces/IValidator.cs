using SpecAPI.Models;

namespace SpecAPI.Interfaces;

public interface IValidator
{
    bool Validate(TestResult result, Expect expectations);
} 