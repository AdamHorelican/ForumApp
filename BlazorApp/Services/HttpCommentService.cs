using System.Net.Http.Json;
using DTOs;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        HttpResponseMessage response = await client.PostAsJsonAsync("comments", request);
        return await ApiResponse.ReadAsync<CommentDto>(response);
    }

    public async Task<List<CommentDto>> GetCommentsForPostAsync(int postId)
    {
        HttpResponseMessage response = await client.GetAsync($"comments?postId={postId}");
        return await ApiResponse.ReadAsync<List<CommentDto>>(response);
    }
}