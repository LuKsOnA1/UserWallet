namespace RepositoryLayer.Repositories.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllEntity ();
        Task<T> GetEntityByIdAsync (Guid id);
        Task AddEntityAsync (T entity);
        void UpdateEntity (T entity);
        void DeleteEntity (T entity);
    }
}
