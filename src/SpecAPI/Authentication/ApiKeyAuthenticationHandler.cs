using System.Web;
using SpecAPI.Interfaces;
using SpecAPI.Models;

namespace SpecAPI.Authentication;

public class ApiKeyAuthenticationHandler : IAuthenticationHandler
{
    public bool CanHandle(Auth auth)
    {
        return auth.Type?.Equals("apikey", StringComparison.OrdinalIgnoreCase) == true
               && !string.IsNullOrEmpty(auth.Name)
               && !string.IsNullOrEmpty(auth.Value);
    }

    public void ApplyAuthentication(HttpRequestMessage request, Auth auth)
    {
        if (auth.In?.ToLower() == "query")
        {
            var uriBuilder = new UriBuilder(request.RequestUri!);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[auth.Name] = auth.Value;
            uriBuilder.Query = query.ToString();
            request.RequestUri = uriBuilder.Uri;
        }
        else // Default to header
        {
            request.Headers.TryAddWithoutValidation(auth.Name, auth.Value);
        }
    }
} 