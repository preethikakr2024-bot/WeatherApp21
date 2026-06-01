using Microsoft.Extensions.Configuration;
using Supabase;
using Supabase.Gotrue;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BlazorApp21.Services
{
    [Table("favorites")]
    public class UserFavorite : BaseModel
    {
        [PrimaryKey("id", false)]
        public string? Id { get; set; }

        [Column("user_id")]
        public string? UserId { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("weather")]
        public string? Weather { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }

    public class SupabaseService
    {
        private Supabase.Client? _client;
        private readonly LocalStorageService _localStorage;
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;
        private const string SessionKey = "supabase_session";

        public string FavoriteCity { get; set; } = string.Empty;
        public string FavoriteWeather { get; set; } = string.Empty;
        public string LastDbError { get; set; } = string.Empty;

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

        // ✅ Fixed: Delete then Insert instead of Update (avoids Where filter mismatch)
        public async Task<string> SaveFavoriteToDb(string city, string weather)
        {
            await Initialize();

            var userId = GetUser()?.Id;
            if (string.IsNullOrEmpty(userId))
                return "Error: User not logged in — GetUser() returned null";

            try
            {
                // ✅ Delete old row first
                await _client!
                    .From<UserFavorite>()
                    .Where(x => x.UserId == userId)
                    .Delete();

                // ✅ Insert fresh row
                var favorite = new UserFavorite
                {
                    UserId = userId,
                    City = city,
                    Weather = weather
                };

                await _client!.From<UserFavorite>().Insert(favorite);

                // ✅ Update in-memory
                FavoriteCity = city;
                FavoriteWeather = weather;
                LastDbError = string.Empty;
                return "Success";
            }
            catch (Exception ex)
            {
                LastDbError = ex.Message;
                return $"Error: {ex.Message}";
            }
        }

        // ✅ Fixed: Also updates in-memory on load
        public async Task<UserFavorite?> LoadFavoriteFromDb()
        {
            await Initialize();
            var userId = GetUser()?.Id;
            if (string.IsNullOrEmpty(userId)) return null;

            try
            {
                var result = await _client!
                    .From<UserFavorite>()
                    .Where(x => x.UserId == userId)
                    .Get();

                var favorite = result.Models.FirstOrDefault();

                if (favorite != null)
                {
                    FavoriteCity = favorite.City ?? string.Empty;
                    FavoriteWeather = favorite.Weather ?? string.Empty;
                }

                return favorite;
            }
            catch (Exception ex)
            {
                LastDbError = ex.Message;
                return null;
            }
        }

        public async Task ClearFavoriteFromDb()
        {
            await Initialize();
            var userId = GetUser()?.Id;
            if (string.IsNullOrEmpty(userId)) return;

            try
            {
                await _client!
                    .From<UserFavorite>()
                    .Where(x => x.UserId == userId)
                    .Delete();

                FavoriteCity = string.Empty;
                FavoriteWeather = string.Empty;
            }
            catch (Exception ex)
            {
                LastDbError = ex.Message;
            }
        }

        public User? GetUser() => _client?.Auth.CurrentUser;
        public bool IsLoggedIn() => _client?.Auth.CurrentSession != null;
    }
}