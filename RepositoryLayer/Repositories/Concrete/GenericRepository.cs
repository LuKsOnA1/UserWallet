using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RepositoryLayer.Repository.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }


        public async Task AddEntityAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void DeleteEntity(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> GetAllEntity()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        public async Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbSet.AsNoTracking();

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }


        public async Task<T> GetEntityByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetIncludeAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.FirstOrDefaultAsync();
        }



        public void UpdateEntity(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
