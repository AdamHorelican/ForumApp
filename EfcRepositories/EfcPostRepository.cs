using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositaryContracts;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly ForumContext ctx;

    public EfcPostRepository(ForumContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> AddAsync(Post post)
    {
        EntityEntry<Post> entry = await ctx.Posts.AddAsync(post);
        await ctx.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task UpdateAsync(Post post)
    {
        if (!await ctx.Posts.AnyAsync(p => p.Id == post.Id))
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' was not found.");
        }

        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found");
        }

        ctx.Posts.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        Post? post = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' was not found.");
        }

        return post;
    }

    public IQueryable<Post> GetAll()
    {
        return ctx.Posts.AsQueryable();
    }
}