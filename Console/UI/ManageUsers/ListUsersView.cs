using Entities;
using RepositaryContracts;

namespace Console.UI.ManageUsers;
using System;

public class ListUsersView
{
    private readonly IUserRepository userRepo;
    
    public ListUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public void Show()
    {
        Console.WriteLine();
        Console.WriteLine("=== Users ===");

        List<User> users = userRepo.GetAll().ToList();
        
        if (users.Count > 0)
        {
            foreach (User user in users)
            {
                Console.WriteLine(user.Id + " " + user.Username);
            }
        }
        else
        {
            Console.WriteLine("No users found");
        }
    }
}