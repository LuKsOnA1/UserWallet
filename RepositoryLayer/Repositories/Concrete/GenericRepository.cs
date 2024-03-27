using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Repositories.Abstract;

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

        public async Task<T> GetEntityByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void UpdateEntity(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
