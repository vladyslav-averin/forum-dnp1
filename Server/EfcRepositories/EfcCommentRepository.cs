using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        EntityEntry<Comment> entityEntry = await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(Comment comment)
    {
        if (!await ctx.Comments.AnyAsync(existing => existing.Id == comment.Id))
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
        }

        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Comment? comment = await ctx.Comments.SingleOrDefaultAsync(comment => comment.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        ctx.Comments.Remove(comment);
        await ctx.SaveChangesAsync();
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = await ctx.Comments.SingleOrDefaultAsync(comment => comment.Id == id);
        if (comment is null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found");
        }

        return comment;
    }

    public IQueryable<Comment> GetMany()
    {
        return ctx.Comments.AsQueryable();
    }
}
