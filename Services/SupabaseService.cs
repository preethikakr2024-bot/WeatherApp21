using Microsoft.Extensions.Configuration;
using Supabase;
using Supabase.Gotrue;

namespace BlazorApp21.Services
{
    public class SupabaseService
    {
        private Supabase.Client? _client;
        private readonly LocalStorageService _localStorage;
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private const string SessionKey = "supabase_session";

        public string FavoriteCity { get; set; } = string.Empty;
        public string FavoriteWeather { get; set; } = string.Empty;

        public SupabaseService(LocalStorageService localStorage, IConfiguration config)
        {
            _localStorage = localStorage;
            _supabaseUrl = config["Supabase:Url"]!;
            _supabaseKey = config["Supabase:Key"]!;
        }

        public async Task Initialize()
        {
            if (_client == null)
            {
                var options = new SupabaseOptions
                {
                    AutoConnectRealtime = false,
                    AutoRefreshToken = true
                };
                _client = new Supabase.Client(_supabaseUrl, _supabaseKey, options);
                await _client.InitializeAsync();
            }

            if (_client.Auth.CurrentSession == null)
            {
                try
                {
                    var savedSession = await _localStorage.GetItem(SessionKey);
                    if (!string.IsNullOrEmpty(savedSession))
                    {
                        var session = System.Text.Json.JsonSerializer.Deserialize<Session>(savedSession);
                        if (session != null)
                            await _client.Auth.SetSession(session.AccessToken!, session.RefreshToken!);
                    }
                }
                catch { }
            }
        }

        public async Task<Session?> Login(string email, string password)
        {
            await _localStorage.RemoveItem(SessionKey);
            _client = null;

            var options = new SupabaseOptions
            {
                AutoConnectRealtime = false,
                AutoRefreshToken = true
            };

            _client = new Supabase.Client(_supabaseUrl, _supabaseKey, options);
            await _client.InitializeAsync();

            var session = await _client.Auth.SignInWithPassword(email, password);

            if (session != null)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(session);
                await _localStorage.SetItem(SessionKey, json);
            }

            return session;
        }

        public async Task<bool> SignUp(string email, string password)
        {
            await Initialize();
            var response = await _client!.Auth.SignUp(email, password);
            return response != null;
        }

        public async Task Logout()
        {
            if (_client != null)
            {
                try { await _client.Auth.SignOut(); } catch { }
                _client = null;
            }
            await _localStorage.RemoveItem(SessionKey);
            FavoriteCity = string.Empty;
            FavoriteWeather = string.Empty;
        }

        public User? GetUser() => _client?.Auth.CurrentUser;
        public bool IsLoggedIn() => _client?.Auth.CurrentSession != null;
    }
}
