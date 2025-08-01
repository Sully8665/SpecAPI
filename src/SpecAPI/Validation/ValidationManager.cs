using SpecAPI.Interfaces;
using SpecAPI.Models;

namespace SpecAPI.Validation;

public class ValidationManager
{
    private readonly IEnumerable<IValidator> _validators;

    public ValidationManager(IEnumerable<IValidator> validators)
    {
        _validators = validators;
    }

    public bool Validate(TestResult result, Expect expectations)
    {
        return _validators.All(validator => validator.Validate(result, expectations));
    }
} 