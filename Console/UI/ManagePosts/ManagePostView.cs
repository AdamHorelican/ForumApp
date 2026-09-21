using Console.UI.ManageComments;
using RepositaryContracts;

namespace Console.UI.ManagePosts;
using System;
public class ManagePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
 
    public ManagePostView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Manage posts ===");
            Console.WriteLine("1. Create post");
            Console.WriteLine("2. List posts");
            Console.WriteLine("3. View single post");
            Console.WriteLine("4. Add comment to post");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
            string? choice = Console.ReadLine();
 
            switch (choice)
            {
                case "1":
                    await new CreatePostView(postRepository, userRepository).ShowAsync();
                    break;
                case "2":
                    new ListPostsView(postRepository).Show();
                    break;
                case "3":
                    await new SinglePostView(postRepository, userRepository, commentRepository).ShowAsync();
                    break;
                case "4":
                    await new CreateCommentView(commentRepository, userRepository, postRepository).ShowAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }
    
}