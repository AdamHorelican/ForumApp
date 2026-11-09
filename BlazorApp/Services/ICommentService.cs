using DTOs;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
    Task<List<CommentDto>> GetCommentsForPostAsync(int postId);
}