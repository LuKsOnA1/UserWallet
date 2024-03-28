using EntityLayer.Models;
using MediatR;
using RepositoryLayer.Repositories.Abstract;
using RepositoryLayer.UnitOfWorks.Abstract;
using UserWalletService.MediatR.Queries;

namespace UserWalletService.MediatR.Handlers
{
    public class LoadAllUserHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _repository;

        public LoadAllUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetGenericRepository<User>();
        }
        public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllIncluding(x => x.Wallet, x => x.Wallet.Transactions);

            return users;
        }
    }
}
