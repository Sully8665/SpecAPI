using SpecAPI.DependencyInjection;
using SpecAPI.Interfaces;
using SpecAPI.Loading;
using SpecAPI.Services;

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
            // Create service provider
            var serviceProvider = ServiceContainer.CreateServiceProvider();

            // Get required services
            var testRunner = (ITestRunner)serviceProvider.GetService(typeof(ITestRunner))!;
            var loaderFactory = (TestLoaderFactory)serviceProvider.GetService(typeof(TestLoaderFactory))!;
            var resultReporter = (IResultReporter)serviceProvider.GetService(typeof(IResultReporter))!;

            // Create and execute test service
            var testService = new TestExecutionService(testRunner, loaderFactory, resultReporter);
            await testService.ExecuteTestsAsync(yamlPath);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"💥 Error: {ex.Message}");
            Console.ResetColor();
        }
    }
}
