using System.Net.Http.Json;
using DTOs;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(CreateUserDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("users", request);
        return await ApiResponse.ReadAsync<UserDto>(response);
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        HttpResponseMessage response = await client.GetAsync("users");
        return await ApiResponse.ReadAsync<List<UserDto>>(response);
    }
}