using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositaryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly ForumContext ctx;

    public EfcUserRepository(ForumContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        if (await ctx.Users.AnyAsync(u => u.Username == user.Username))
        {
            throw new InvalidOperationException(
                $"User with username '{user.Username}' already exists");
        }

        EntityEntry<User> entry = await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return entry.Entity;
    }

    public async Task UpdateAsync(User user)
    {
        if (!await ctx.Users.AnyAsync(u => u.Id == user.Id))
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' not found");
        }

        if (await ctx.Users.AnyAsync(u => u.Username == user.Username && u.Id != user.Id))
        {
            throw new InvalidOperationException(
                $"User with username '{user.Username}' already exists");
        }

        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User? existing = await ctx.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (existing is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }

        ctx.Users.Remove(existing);
        await ctx.SaveChangesAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        User? user = await ctx.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }

        return user;
    }

    public IQueryable<User> GetAll()
    {
        return ctx.Users.AsQueryable();
    }
}