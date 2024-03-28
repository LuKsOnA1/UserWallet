using EntityLayer.Models;
using MediatR;

namespace UserWalletService.Commands
{
    public class RemoveUserRequest : IRequest<User>
    {
        public Guid _id { get; }

        public RemoveUserRequest(Guid id)
        {
            _id = id;
        }
    }
}
