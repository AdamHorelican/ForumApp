using Entities;
using RepositaryContracts;

namespace Console.UI.ManageComments;
using System;
public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;
 
    public CreateCommentView(ICommentRepository commentRepository, IUserRepository userRepository, IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }
    
    public async Task ShowAsync()
    {
        Console.WriteLine();
        Console.Write("Post id to comment on: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Post id must be a number.");
            return;
        }
 
        await AddToPostAsync(postId);
    }
    
    public async Task AddToPostAsync(int postId)
    {

        try
        {
            await postRepository.GetByIdAsync(postId);
        }
        catch (Exception)
        {
            Console.WriteLine($"No post with id {postId}.");
            return;
        }
 
        Console.Write("Your user id: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("User id must be a number.");
            return;
        }
        
        try
        {
            await userRepository.GetByIdAsync(userId);
        }
        catch (Exception)
        {
            Console.WriteLine($"No user with id {userId}.");
            return;
        }
 
        Console.Write("Comment: ");
        string body = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment cannot be empty.");
            return;
        }
 
        Comment comment = new Comment
        {
            Body = body,
            UserId = userId,
            PostId = postId
        };
        Comment created = await commentRepository.AddAsync(comment);
        Console.WriteLine($"Comment added with id {created.Id}.");
    }
}