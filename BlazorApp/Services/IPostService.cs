using DTOs;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<PostDto> GetPostAsync(int id);
    Task<List<PostDto>> GetPostsAsync();
}