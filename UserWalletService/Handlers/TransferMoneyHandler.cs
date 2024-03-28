using EntityLayer.DTOs;
using EntityLayer.Models;
using MediatR;
using RepositoryLayer.Repositories.Abstract;
using RepositoryLayer.UnitOfWorks.Abstract;
using UserWalletService.Commands;

namespace UserWalletService.Handlers
{
   
    public class TransferMoneyHandler : IRequestHandler<TransferMoneyRequest, TransferMoneyDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _repository;

        public TransferMoneyHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetGenericRepository<User>();
        }
        public async Task<TransferMoneyDTO> Handle(TransferMoneyRequest request, CancellationToken cancellationToken)
        {
            var senderUserId = await _repository.GetIncludeAsync(filter: x => x.Id == request._senderId, includeProperties: "Wallet,Wallet.Transactions");
            var recipientUserId = await _repository.GetIncludeAsync(filter: x => x.Id == request._recipientId, includeProperties: "Wallet,Wallet.Transactions");

            if(senderUserId == null || recipientUserId == null)
            {
                return null;
            }

            var senderWalletId = senderUserId.Wallet.Id;
            var recipientWalletId = recipientUserId.Wallet.Id;

            if (senderUserId == null || recipientUserId == null)
            {
                return null;
            }

            if (senderUserId.Wallet.Balance < request._amount)
            {
                return null;
            }

            senderUserId.Wallet.Balance -= request._amount;
            recipientUserId.Wallet.Balance += request._amount;

            var transaction = new Transaction
            {
                SenderUserId = senderUserId.Id,
                RecipientUserId = recipientUserId.Id,
                Amount = request._amount,
                DateOfTransfer = DateTime.Now,
                WalletId = senderUserId.Wallet.Id

            };

            var finalTransactionDTO = new TransferMoneyDTO
            {
                SenderUserId = transaction.SenderUserId,
                RecipientUserId = transaction.RecipientUserId,
                Amount = transaction.Amount,
                DateOfTransfer = transaction.DateOfTransfer,
                WalletId = transaction.WalletId
            };

            await _unitOfWork.GetGenericRepository<Transaction>().AddEntityAsync(transaction);
            await _unitOfWork.CommitAsync();

            return finalTransactionDTO;
        }
    }
}
