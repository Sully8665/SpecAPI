using SpecAPI.Interfaces;
using SpecAPI.Models;

namespace SpecAPI.Authentication;

public class AuthenticationManager
{
    private readonly IEnumerable<IAuthenticationHandler> _handlers;

    public AuthenticationManager(IEnumerable<IAuthenticationHandler> handlers)
    {
        _handlers = handlers;
    }

    public void ApplyAuthentication(HttpRequestMessage request, Auth? auth)
    {
        if (auth == null) return;

        var handler = _handlers.FirstOrDefault(h => h.CanHandle(auth));
        if (handler != null)
        {
            handler.ApplyAuthentication(request, auth);
        }
    }
} 