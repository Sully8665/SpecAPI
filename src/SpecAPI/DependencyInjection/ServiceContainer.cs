using SpecAPI.Authentication;
using SpecAPI.Interfaces;
using SpecAPI.Loading;
using SpecAPI.Reporting;
using SpecAPI.Running;
using SpecAPI.Validation;

namespace SpecAPI.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceProvider CreateServiceProvider()
    {
        var services = new List<(Type ServiceType, object Implementation)>();

        // HTTP Client
        services.Add((typeof(HttpClient), new HttpClient()));

        // Authentication handlers
        var authHandlers = new List<IAuthenticationHandler>
        {
            new BasicAuthenticationHandler(),
            new BearerAuthenticationHandler(),
            new ApiKeyAuthenticationHandler()
        };
        services.Add((typeof(IEnumerable<IAuthenticationHandler>), authHandlers));

        // Authentication manager
        services.Add((typeof(AuthenticationManager), new AuthenticationManager(authHandlers)));

        // Validators
        var validators = new List<IValidator>
        {
            new StatusCodeValidator(),
            new BodyValidator(),
            new ResponseTimeValidator()
        };
        services.Add((typeof(IEnumerable<IValidator>), validators));

        // Validation manager
        services.Add((typeof(ValidationManager), new ValidationManager(validators)));

        // Test loader factory
        services.Add((typeof(TestLoaderFactory), new TestLoaderFactory()));

        // Test runner
        var httpClient = (HttpClient)services.First(s => s.ServiceType == typeof(HttpClient)).Implementation;
        var authManager = (AuthenticationManager)services.First(s => s.ServiceType == typeof(AuthenticationManager)).Implementation;
        var validationManager = (ValidationManager)services.First(s => s.ServiceType == typeof(ValidationManager)).Implementation;
        services.Add((typeof(ITestRunner), new HttpTestRunner(httpClient, authManager, validationManager)));

        // Result reporter
        services.Add((typeof(IResultReporter), new ConsoleResultReporter()));

        return new ServiceProvider(services);
    }
}

public class ServiceProvider : IServiceProvider
{
    private readonly Dictionary<Type, object> _services;

    public ServiceProvider(List<(Type ServiceType, object Implementation)> services)
    {
        _services = services.ToDictionary(s => s.ServiceType, s => s.Implementation);
    }

    public object? GetService(Type serviceType)
    {
        return _services.TryGetValue(serviceType, out var service) ? service : null;
    }
} 