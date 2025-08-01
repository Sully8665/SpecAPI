using SpecAPI.Models;

namespace SpecAPI.Interfaces;

public interface IAuthenticationHandler
{
    bool CanHandle(Auth auth);
    void ApplyAuthentication(HttpRequestMessage request, Auth auth);
} 