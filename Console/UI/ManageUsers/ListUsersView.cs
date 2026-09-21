using RepositaryContracts;

namespace Console.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository userRepo;
    
    public ListUsersView(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }
    
    
}