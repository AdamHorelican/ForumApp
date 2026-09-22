using Entities;
using RepositaryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    public Task<User> AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<User> GetAll()
    {
        throw new NotImplementedException();
    }
}