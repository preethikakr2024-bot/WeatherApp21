namespace BlazorApp21.Services
{
    // ✅ Shared theme state across components
    public static class ThemeState
    {
        public static bool IsDark { get; set; } = false;
        public static event Action? OnChange;
        public static void NotifyChanged() => OnChange?.Invoke();
    }
}
