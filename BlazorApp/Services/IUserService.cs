using DTOs;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<List<UserDto>> GetUsersAsync();
}