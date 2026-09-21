using Console.UI.ManageComments;
using Entities;
using RepositaryContracts;

namespace Console.UI.ManagePosts;
using System;
public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
 
    public SinglePostView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }
 
    public async Task ShowAsync()
    {
        Console.WriteLine();
        Console.Write("Post id: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Post id must be a number.");
            return;
        }
 
        Post post;
        try
        {
            post = await postRepository.GetByIdAsync(postId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
 
        string authorName = await GetUserNameAsync(post.UserId);
 
        Console.WriteLine();
        Console.WriteLine($"=== {post.Title} ===");
        Console.WriteLine($"By: {authorName}");
        Console.WriteLine();
        Console.WriteLine(post.Body);
        Console.WriteLine();
        Console.WriteLine("Comments:");
 
        List<Comment> comments = commentRepository.GetAll()
            .Where(c => c.PostId == postId)
            .ToList();
 
        if (comments.Count == 0)
        {
            Console.WriteLine("  No comments yet.");
        }
 
        foreach (Comment comment in comments)
        {
            string commenterName = await GetUserNameAsync(comment.UserId);
            Console.WriteLine($"  - {commenterName}: {comment.Body}");
        }
 
        Console.WriteLine();
        Console.Write("Type 1 to add a comment, or press Enter to go back: ");
        if (Console.ReadLine() == "1")
        {
            await new CreateCommentView(commentRepository, userRepository, postRepository)
                .AddToPostAsync(postId);
        }
    }
 
    private async Task<string> GetUserNameAsync(int userId)
    {
        try
        {
            User user = await userRepository.GetByIdAsync(userId);
            return user.Username;
        }
        catch (Exception)
        {
            return "[unknown user]";
        }
    }
}