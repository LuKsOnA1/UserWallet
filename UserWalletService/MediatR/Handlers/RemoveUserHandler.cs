using EntityLayer.Models;
using MediatR;
using RepositoryLayer.Data;
using RepositoryLayer.Repositories.Abstract;
using RepositoryLayer.UnitOfWorks.Abstract;
using UserWalletService.MediatR.Commands;

namespace UserWalletService.MediatR.Handlers
{
    public class RemoveUserHandler : IRequestHandler<RemoveUserRequest, User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _repository;
        private readonly ApplicationDbContext _db;

        public RemoveUserHandler(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetGenericRepository<User>();
            _db = db;
        }

        public async Task<User> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetIncludeAsync(filter: x => x.Id == request._id, includeProperties: "Wallet,Wallet.Transactions");


            if (user == null)
            {
                return null;
            }

            // Deleting all transactions associated with user
            _unitOfWork.GetGenericRepository<Transaction>().RemoveRange(user.Wallet.Transactions);

            // Deleting all transactions where user is recipient
            var recipientUserTransactions = _unitOfWork.GetGenericRepository<Transaction>().Where(x => x.RecipientUserId == user.Id).ToList();
            _unitOfWork.GetGenericRepository<Transaction>().RemoveRange(recipientUserTransactions);

            // Deleting wallet
            _unitOfWork.GetGenericRepository<Wallet>().DeleteEntity(user.Wallet);

            // Deleting user
            _repository.DeleteEntity(user);
            await _unitOfWork.CommitAsync();

            return user;
        }
    }
}
