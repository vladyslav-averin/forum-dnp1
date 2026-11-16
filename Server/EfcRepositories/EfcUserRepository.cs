using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext ctx;

    public EfcUserRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        EntityEntry<User> entityEntry = await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(User user)
    {
        if (!await ctx.Users.AnyAsync(existing => existing.Id == user.Id))
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' not found");
        }

        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        User? user = await ctx.Users.SingleOrDefaultAsync(user => user.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }

        ctx.Users.Remove(user);
        await ctx.SaveChangesAsync();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        User? user = await ctx.Users.SingleOrDefaultAsync(user => user.Id == id);
        if (user is null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found");
        }

        return user;
    }

    public IQueryable<User> GetMany()
    {
        return ctx.Users.AsQueryable();
    }
}
