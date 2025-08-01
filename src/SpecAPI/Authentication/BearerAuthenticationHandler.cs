using System.Net.Http.Headers;
using SpecAPI.Interfaces;
using SpecAPI.Models;

namespace SpecAPI.Authentication;

public class BearerAuthenticationHandler : IAuthenticationHandler
{
    public bool CanHandle(Auth auth)
    {
        return auth.Type?.Equals("bearer", StringComparison.OrdinalIgnoreCase) == true
               && !string.IsNullOrEmpty(auth.Token);
    }

    public void ApplyAuthentication(HttpRequestMessage request, Auth auth)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
    }
} 