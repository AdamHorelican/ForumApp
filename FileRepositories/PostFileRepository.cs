using System.Text.Json;
using Entities;
using RepositaryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{

    private readonly string FilePath = "posts.json";


    public PostFileRepository()
    {
        if (!File.Exists(FilePath))
        {
            File.WriteAllText(FilePath, "[]");
        }
    }
    public async Task<Post> AddAsync(Post post)
    {
        string PostAsJson = await File.ReadAllTextAsync(FilePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(PostAsJson)!;
        int maxId = posts.Count > 0 ? posts.Max(c => c.Id) : 0;
        post.Id = maxId + 1;
        posts.Add(post);
        
        PostAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(FilePath, PostAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string PostAsJson = await File.ReadAllTextAsync(FilePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(PostAsJson)!;
        
        Post? existingPost = posts.FirstOrDefault(c => c.Id == post.Id);
        if (existingPost == null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' was not found.");
        }

        posts.Remove(existingPost);
        posts.Add(post);
        
        PostAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(FilePath, PostAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string PostAsJson = await File.ReadAllTextAsync(FilePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(PostAsJson)!;
        
        Post? post = posts.FirstOrDefault(c => c.Id == id);
        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        posts.Remove(post);
        
        PostAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(FilePath, PostAsJson);
    }

    public Task<Post> GetByIdAsync(int id)
    {
        string PostAsJson =  File.ReadAllTextAsync(FilePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(PostAsJson)!;
        Post? post = posts.FirstOrDefault(c => c.Id == id);

        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' was not found.");
        }
        
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetAll()
    {
        string postAsJson = File.ReadAllTextAsync(FilePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postAsJson)!;
        return posts.AsQueryable();
    }
}