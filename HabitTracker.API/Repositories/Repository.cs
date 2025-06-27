using HabitTacker.Contexts;
using HabitTacker.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace HabitTacker.Repositories;

public class Repository<K, T> : IRepository<K, T> where T : class
{
    protected readonly HabitTrackerContext _habitTrackerContext;
    protected readonly DbSet<T> _dbSet;

    public Repository(HabitTrackerContext habitTrackerContext)
    {
        _habitTrackerContext = habitTrackerContext;
        _dbSet = _habitTrackerContext.Set<T>();
    }

    public async Task<T> Add(T item)
    {
        await _dbSet.AddAsync(item);
        await _habitTrackerContext.SaveChangesAsync();
        return item;
    }

    public async Task<T?> Get(K key)
    {
        var entity = await _dbSet.FindAsync(key);
        return entity;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> Update(K key, T item)
    {
        var existing = await Get(key);
        if (existing == null)
            return null;

        _habitTrackerContext.Entry(existing).CurrentValues.SetValues(item);
        await _habitTrackerContext.SaveChangesAsync();
        return existing;
    }

    public async Task<T?> Delete(K key)
    {
        var existing = await Get(key);
        if (existing == null)
            return null;
        _dbSet.Remove(existing);
        await _habitTrackerContext.SaveChangesAsync();
        return existing;
    }
}

