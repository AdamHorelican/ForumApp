using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using BlazorApp.Services;
using DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private const string StorageKey = "currentUser";

    private readonly HttpClient httpClient;
    private readonly IJSRuntime jsRuntime;

    public SimpleAuthProvider(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        this.httpClient = httpClient;
        this.jsRuntime = jsRuntime;
    }

    public async Task Login(string userName, string password)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(
            "auth/login", new LoginRequest(userName, password));
        UserDto userDto = await ApiResponse.ReadAsync<UserDto>(response);

        string serialisedUser = JsonSerializer.Serialize(userDto);
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", StorageKey, serialisedUser);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(CreatePrincipal(userDto))));
    }

    public async Task Logout()
    {
        await jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", StorageKey);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? serialisedUser;

        try
        {
            serialisedUser = await jsRuntime.InvokeAsync<string?>("sessionStorage.getItem", StorageKey);
        }
        catch (InvalidOperationException)
        {
            // JavaScript is not available yet, so nobody can be logged in at this point.
            return new AuthenticationState(new ClaimsPrincipal());
        }

        if (string.IsNullOrEmpty(serialisedUser))
        {
            return new AuthenticationState(new ClaimsPrincipal());
        }

        UserDto userDto = JsonSerializer.Deserialize<UserDto>(serialisedUser)!;
        return new AuthenticationState(CreatePrincipal(userDto));
    }

    private static ClaimsPrincipal CreatePrincipal(UserDto userDto)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, userDto.Username),
            new Claim("Id", userDto.Id.ToString())
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
    }
}