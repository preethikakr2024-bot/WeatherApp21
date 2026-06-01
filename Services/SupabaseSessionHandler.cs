using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
namespace BlazorApp21.Services
{
    public class SupabaseSessionHandler : IGotrueSessionPersistence<Session>
    {
        private static string? _inMemorySession;
        public void SaveSession(Session session)
        {
            _inMemorySession = System.Text.Json.JsonSerializer.Serialize(session);
        }
        public void DestroySession()
        {
            _inMemorySession = null;
        }
        public Session? LoadSession()
        {
            if (string.IsNullOrEmpty(_inMemorySession)) return null;
            return System.Text.Json.JsonSerializer.Deserialize<Session>(_inMemorySession);
        }
    }
}