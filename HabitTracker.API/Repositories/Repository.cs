using HabitTacker.Contexts;
using HabitTacker.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HabitTacker.Repositories
{
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
            if(item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }
            await _dbSet.AddAsync(item);
            await _habitTrackerContext.SaveChangesAsync();
            return item;
        }

        public async Task<T> Get(K key)
        {
            var entity = await _dbSet.FindAsync(key);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(T).Name} with key '{key}' not found.");
            }
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> Update(K key, T item)
        {
            var existing = await Get(key);
            _habitTrackerContext.Entry(existing).CurrentValues.SetValues(item);
            await _habitTrackerContext.SaveChangesAsync();
            return existing;
        }

        public async Task<T> Delete(K key)
        {
            var existing = await Get(key);
            _dbSet.Remove(existing);
            await _habitTrackerContext.SaveChangesAsync();
            return existing;
        }
    }
}
