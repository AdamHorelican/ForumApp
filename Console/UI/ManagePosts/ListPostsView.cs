using Entities;
using RepositaryContracts;

namespace Console.UI.ManagePosts;
using System;
public class ListPostsView
{
    private readonly IPostRepository postRepository;
 
    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }
 
    public void Show()
    {
        Console.WriteLine();
        Console.WriteLine("=== Posts ===");
 
        List<Post> posts = postRepository.GetAll().ToList();
        if (posts.Count == 0)
        {
            Console.WriteLine("No posts yet.");
            return;
        }
 
        foreach (Post post in posts)
        {
            Console.WriteLine($"[{post.Title}, {post.Id}]");
        }
    }
}