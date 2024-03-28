using EntityLayer.DTOs;
using MediatR;

namespace UserWalletService.MediatR.Commands
{
    public class TransferMoneyRequest : IRequest<TransferMoneyDTO>
    {
        public Guid _senderId { get; }
        public Guid _recipientId { get; }
        public decimal _amount { get; }

        public TransferMoneyRequest(Guid senderId, Guid recipientId, decimal amount)
        {
            _senderId = senderId;
            _recipientId = recipientId;
            _amount = amount;
        }
    }
}
