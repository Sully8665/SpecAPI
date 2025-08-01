using System.Net.Http.Headers;
using System.Text;
using SpecAPI.Interfaces;
using SpecAPI.Models;

namespace SpecAPI.Authentication;

public class BasicAuthenticationHandler : IAuthenticationHandler
{
    public bool CanHandle(Auth auth)
    {
        return auth.Type?.Equals("basic", StringComparison.OrdinalIgnoreCase) == true
               && !string.IsNullOrEmpty(auth.Username)
               && !string.IsNullOrEmpty(auth.Password);
    }

    public void ApplyAuthentication(HttpRequestMessage request, Auth auth)
    {
        var byteArray = Encoding.ASCII.GetBytes($"{auth.Username}:{auth.Password}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
    }
} 