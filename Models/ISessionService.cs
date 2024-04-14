namespace Gbc_Travel_Group63.Models
{
    public interface ISessionService
    {
        T Get<T>(string key);
        void Set<T>(string key, T value);
    }
}
