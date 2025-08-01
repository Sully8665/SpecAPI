namespace SpecAPI.Helpers
{
    public static class ResponseHelper
    {
        public static string CreateResponseMessage(string message)
        {
            return $"{{\"message\":\"{message}\"}}";
        }
    }
}
