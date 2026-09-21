using System.Globalization;
using Entities;
using RepositaryContracts;

namespace Console.UI.ManagePosts;
using System;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(IPostRepository postRepository,  IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine();
        Console.WriteLine("=== Create post ===");

        Console.WriteLine("Your user id: ");

        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid user input");
            return;
        }
        
        try
        {
            await userRepository.GetByIdAsync(userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }

        Console.WriteLine("Your title: ");
        string? title = Console.ReadLine().Trim();

        Console.WriteLine("Body of the post: ");
        string? body = Console.ReadLine().Trim();

        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(body))
        {
            Console.WriteLine("Invalid title or body, try again.");
            return;
        }

        Post post = new Post()
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        Post created = await postRepository.AddAsync(post);

        Console.WriteLine($"Post {created.Title} with id: {created.Id} has been created.");
    }
    
}