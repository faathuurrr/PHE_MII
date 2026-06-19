using Microsoft.JSInterop;
using System.Text.Json;

namespace SupplyManagement.Web.Services
{
    public class AuthStateService
    {
        private readonly IJSRuntime _js;
        private const string TOKEN_KEY = "auth_token";
        private const string ROLE_KEY  = "auth_role";

        public AuthStateService(IJSRuntime js) => _js = js;

        public async Task SaveTokenAsync(string token, string role)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", TOKEN_KEY, token);
            await _js.InvokeVoidAsync("localStorage.setItem", ROLE_KEY, role);
        }

        public async Task<string?> GetTokenAsync()
            => await _js.InvokeAsync<string?>("localStorage.getItem", TOKEN_KEY);

        public async Task<string?> GetRoleAsync()
            => await _js.InvokeAsync<string?>("localStorage.getItem", ROLE_KEY);

        public async Task ClearAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", TOKEN_KEY);
            await _js.InvokeVoidAsync("localStorage.removeItem", ROLE_KEY);
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
