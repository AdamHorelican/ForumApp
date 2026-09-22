using Entities;
using RepositaryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    public Task<Comment> AddAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Comment> GetAll()
    {
        throw new NotImplementedException();
    }
}