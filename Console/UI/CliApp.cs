using Console.UI.ManagePosts;
using Console.UI.ManageUsers;
using RepositaryContracts;

namespace Console.UI;
using System;

public class CliApp
{
    private readonly IUserRepository userRepo;
    private readonly IPostRepository postRepo;
    private readonly ICommentRepository commentRepo;

    public CliApp(IUserRepository userRepo, IPostRepository postRepo, ICommentRepository commentRepo)
    {
        this.userRepo = userRepo;
        this.postRepo = postRepo;
        this.commentRepo = commentRepo;
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Main menu ===");
            Console.WriteLine("1. Manage users");
            Console.WriteLine("2. Manage posts");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");
            string? answer = Console.ReadLine();

            switch (answer)
            {
                case "1":
                    await new ManageUsersView(userRepo).ShowAsync();
                    break;
                case "2":
                    await new ManagePostView(postRepo, userRepo, commentRepo).ShowAsync();
                    break;
                case "0":
                    Console.WriteLine("Bye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }
}