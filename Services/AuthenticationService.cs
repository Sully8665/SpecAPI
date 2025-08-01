namespace SpecAPI.Services
{
    public class AuthenticationService
    {
        public bool ValidateApiKey(string apiKey)
        {
            // In Free version, only one hardcoded key is valid
            return apiKey == "FREE-API-KEY-1234";
        }
    }
}
