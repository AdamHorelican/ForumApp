using Entities;
using RepositaryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    public Task<Post> AddAsync(Post post)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Post post)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Post> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Post> GetAll()
    {
        throw new NotImplementedException();
    }
}