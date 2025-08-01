using System.Collections.Generic;

namespace SpecAPIFree.Services
{
    public class ApiService
    {
        public List<string> GetEndpoints()
        {
            return new List<string>
            {
                "/test",
                "/status"
            };
        }
    }
}
