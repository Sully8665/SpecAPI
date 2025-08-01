using System.Text.Json;
using SpecAPI.Interfaces;
using SpecAPI.Models;
using SpecAPI.Utils;

namespace SpecAPI.Validation;

public class BodyValidator : IValidator
{
    public bool Validate(TestResult result, Expect expectations)
    {
        if (expectations.Body == null) return true;

        try
        {
            using var expectedJson = JsonDocument.Parse(JsonSerializer.Serialize(expectations.Body));
            using var actualJson = JsonDocument.Parse(result.ResponseBody ?? "{}");

            return JsonCompare.IsMatch(expectedJson.RootElement, actualJson.RootElement);
        }
        catch
        {
            return false;
        }
    }
} 