using System.Text.Json;
using Entities;
using RepositaryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{

    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
           File.WriteAllText(filePath, "[]");
        }
    }
    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
        comment.Id = maxId + 1;
        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        string CommentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(CommentsAsJson);
        Comment? existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID '{comment.Id}' not found");
        }
        
        comments.Remove(existingComment);
        comments.Add(comment);

        CommentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, CommentsAsJson);
        
    }

    public async Task DeleteAsync(int id)
    {
        string CommentAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(CommentAsJson);
        Comment? comment = comments.FirstOrDefault(c => c.Id == id);
        
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        comments.Remove(comment);
        
        CommentAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, CommentAsJson);
    }

    public Task<Comment> GetByIdAsync(int id)
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
       Comment? comment = comments.FirstOrDefault(c => c.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetAll()
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }
}