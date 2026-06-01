using Microsoft.JSInterop;
namespace BlazorApp21.Services
{
    public class LocalStorageService(IJSRuntime js)
    {
        // ✅ LocalStorage — permanent
        public async Task SetItem(string key, string value)
            => await js.InvokeVoidAsync("localStorage.setItem", key, value);

        public async Task<string?> GetItem(string key)
            => await js.InvokeAsync<string?>("localStorage.getItem", key);

        public async Task RemoveItem(string key)
            => await js.InvokeVoidAsync("localStorage.removeItem", key);

        public async Task Clear()
            => await js.InvokeVoidAsync("localStorage.clear");

        // ✅ SessionStorage — temporary
        public async Task SetSessionItem(string key, string value)
            => await js.InvokeVoidAsync("sessionStorage.setItem", key, value);

        public async Task<string?> GetSessionItem(string key)
            => await js.InvokeAsync<string?>("sessionStorage.getItem", key);

        public async Task RemoveSessionItem(string key)
            => await js.InvokeVoidAsync("sessionStorage.removeItem", key);

        public async Task ClearSession()
            => await js.InvokeVoidAsync("sessionStorage.clear");
    }
}