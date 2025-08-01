using System.Text.Json;

namespace SpecAPI.Utils;

public static class JsonCompare
{
    public static bool IsMatch(JsonElement expected, JsonElement actual)
    {
        // Simple partial matching: expected should be a subset of actual
        foreach (var expectedProp in expected.EnumerateObject())
        {
            if (!actual.TryGetProperty(expectedProp.Name, out var actualProp))
                return false;

            if (expectedProp.Value.ValueKind != actualProp.ValueKind)
                return false;

            if (expectedProp.Value.ValueKind == JsonValueKind.Object)
            {
                if (!IsMatch(expectedProp.Value, actualProp))
                    return false;
            }
            else if (!expectedProp.Value.ToString().Equals(actualProp.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }

        return true;
    }
} 