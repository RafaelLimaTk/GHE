using GHE.Domain.Interfaces.Base;
using GHE.InfraData.Data;
using Microsoft.EntityFrameworkCore;

namespace GHE.InfraData.Repository.Base;

public class BaseRepository<T> : IRepository<T> where T : class
{
    private readonly GheContext _context;
    public DbSet<T> Entities { get; }
    public BaseRepository(GheContext context)
    {
        _context = context;
        Entities = _context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await Entities.AsNoTracking().ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await Entities.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        Entities.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await Entities.FindAsync(id);
        if (entity == null)
        {
            throw new Exception("Usuário não existe");
        }

        Entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        Entities.Update(entity);
        await _context.SaveChangesAsync();
    }
}
