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
        Console.WriteLine("Main menu");
        Console.WriteLine("1. Manage users");
        Console.WriteLine("2. Manage posts");

        Console.WriteLine("Choose");
        string? asnwer =  Console.ReadLine();
        switch (asnwer)
        {
            case "1":
                await new ManageUsersView(userRepo).ShowAsync();
                break;
        }
    }
}