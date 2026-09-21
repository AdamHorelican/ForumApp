using Entities;
using RepositaryContracts;

namespace Console.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository userRepo;

    public CreateUserView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    public async Task ShowAsync()
    {
        System.Console.WriteLine("Create User:");
        System.Console.WriteLine("Write a username: ");
        string username = System.Console.ReadLine();
        if (string.IsNullOrEmpty(username))
        {
            System.Console.WriteLine("Username is required.");
            return;
        }
        bool taken = userRepo.GetAll()
            .Any(u => u.Username == username);
        if (taken)
        {
            System.Console.WriteLine("Username already exists.");
            return;
        }

        System.Console.WriteLine("Create a paswword: ");
        string password = System.Console.ReadLine();
        if (string.IsNullOrEmpty(password))
        {
            System.Console.WriteLine("Password is required.");
            return;
        }

        User user = new User
        {
            Username = username,
            Password = password,
        };
        User created = await userRepo.AddAsync(user);
    }
}