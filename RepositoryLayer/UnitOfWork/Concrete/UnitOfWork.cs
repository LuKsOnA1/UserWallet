using RepositoryLayer.Data;
using RepositoryLayer.Repository.Abstract;
using RepositoryLayer.UnitOfWork.Abstract;

namespace RepositoryLayer.UnitOfWork.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Commit()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CommitAsync()
        {
            throw new NotImplementedException();
        }

        public IGenericRepository<T> GetGenericRepository<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
