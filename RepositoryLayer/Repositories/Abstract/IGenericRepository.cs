using System.Linq.Expressions;

namespace RepositoryLayer.Repositories.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllEntity ();
        Task<T> GetEntityByIdAsync (Guid id);
        Task<T> GetIncludeAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
        Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task AddEntityAsync (T entity);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        void DeleteEntity (T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
