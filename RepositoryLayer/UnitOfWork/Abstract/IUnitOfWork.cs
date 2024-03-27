using RepositoryLayer.Repository.Abstract;

namespace RepositoryLayer.UnitOfWork.Abstract
{
    public interface IUnitOfWork
    {
        void Commit();
        Task<bool> CommitAsync();
        IGenericRepository<T> GetGenericRepository<T>() where T : class;
    }
}
