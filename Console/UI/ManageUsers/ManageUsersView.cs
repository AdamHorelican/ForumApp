using RepositaryContracts;

namespace Console.UI.ManageUsers;
using System;

public class ManageUsersView
{
    private readonly IUserRepository  userRepo;

    public ManageUsersView(IUserRepository userRepository)
    {
        userRepo = userRepository;
    }

    public async Task ShowAsync()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Manage users ===");
            Console.WriteLine("1. Create user");
            Console.WriteLine("2. List users");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
            string? choice = Console.ReadLine();
 
            switch (choice)
            {
                case "1":
                    await new CreateUserView(userRepo).ShowAsync();
                    break;
                case "2":
                    new ListUsersView().Show();
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