namespace RepositoryLayer.Repository.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllEntity ();
        Task<T> GetEntityByIdAsync (int id);
        Task AddEntityAsync (T entity);
        void UpdateEntity (T entity);
        void DeleteEntity (T entity);
    }
}
