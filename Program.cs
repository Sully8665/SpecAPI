using SpecAPI.Logic;
using static System.Net.Mime.MediaTypeNames;

if (args.Length != 1)
{
    Console.WriteLine("Usage: dotnet run <path-to-yaml-file>");
    return;
}

var yamlPath = args[0];

if (!File.Exists(yamlPath))
{
    Console.WriteLine($"File not found: {yamlPath}");
    return;
}

try
{
    var testCases = TestLoader.Load(yamlPath);

    Console.WriteLine($"✅ Loaded {testCases.Count} test case(s).\n");

    foreach (var testCase in testCases)
    {
        Console.WriteLine($"▶️ Running: {testCase.Name}");

        var result = await TestRunner.RunTestCase(testCase);

        if (result.Passed)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ {result.Message}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {result.Message}");
        }

        Console.ResetColor();
        Console.WriteLine();
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"💥 Error: {ex.Message}");
    Console.ResetColor();
}
