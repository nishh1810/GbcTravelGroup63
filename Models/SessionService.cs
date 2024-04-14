using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Gbc_Travel_Group63.Models
{
    public class SessionService:ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T Get<T>(string key)
        {
            var sessionValue = _httpContextAccessor.HttpContext.Session.GetString(key);
            return sessionValue == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionValue);
        }

        public void Set<T>(string key, T value)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            _httpContextAccessor.HttpContext.Session.SetString(key, serializedValue);
        }
    }
}
