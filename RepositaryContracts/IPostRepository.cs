using Entities;

namespace RepositaryContracts;

public interface IPostRepository
{
    Task<Post> AddAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(int id);
    Task<Post> GetByIdAsync(int id);
    IQueryable<Post> GetAll();
}