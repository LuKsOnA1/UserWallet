using EntityLayer.Models;
using MediatR;
using RepositoryLayer.Repositories.Abstract;
using RepositoryLayer.UnitOfWorks.Abstract;
using UserWalletService.Queries;

namespace UserWalletService.Handlers
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _repository;

        public GetUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetGenericRepository<User>();
        }
        public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return null;
            }

            var user = await _repository.GetIncludeAsync(filter: x => x.Id == request.UserId, includeProperties: "Wallet,Wallet.Transactions");
            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
