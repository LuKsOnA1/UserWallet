using RepositoryLayer.Repositories.Abstract;

namespace RepositoryLayer.UnitOfWorks.Abstract
{
    public interface IUnitOfWork
    {
        void Commit();
        Task<bool> CommitAsync();
        IGenericRepository<T> GetGenericRepository<T>() where T : class;
    }
}
