using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Server.Core
{
    public class Repo<T> : IRepo<T> where T : class
    {
        private readonly ERPDb _crmContext;
        private readonly DbSet<T> _dbSet;
        public Repo(ERPDb coursecontext)
        {
            _crmContext = coursecontext;
            _dbSet = _crmContext.Set<T>();
        }
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task<T?> Get(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<bool> Remove(Guid id)
        {
            var GernericeEntitiy = await _dbSet.FindAsync(id);
            if (GernericeEntitiy != null)
            {
                _dbSet.Remove(GernericeEntitiy);
                return true;
            }
            return false;
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        public void Update(T entity)
        {
            _crmContext.Entry(entity).State = EntityState.Modified;
        }
    }

}
