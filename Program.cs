using SpecAPI;
using SpecAPI.Logic;

class Program
{
    static async Task Main(string[] args)
    {
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

            await TestRunner.RunTests(testCases);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"💥 Error: {ex.Message}");
            Console.ResetColor();
        }
    }
}
