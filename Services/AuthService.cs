namespace BlazorApp21.Services
{
    public class AuthService
    {
        private readonly SupabaseService _supabaseService;

        public event Action? OnChange;

        public AuthService(SupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        public bool IsLoggedIn => _supabaseService.IsLoggedIn();

        public string? CurrentUser => _supabaseService.GetUser()?.Email;

        public async Task<bool> Login(string email, string password)
        {
            try
            {
                await _supabaseService.Initialize();
                var session = await _supabaseService.Login(email, password);
                var success = session != null;
                if (success) NotifyStateChanged();
                return success;
            }
            catch
            {
                return false;
            }
        }

        public async Task Logout()
        {
            await _supabaseService.Logout();
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}