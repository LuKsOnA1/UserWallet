using EntityLayer.Models;
using MediatR;

namespace UserWalletService.MediatR.Queries
{
    public class GetUserQuery : IRequest<User>
    {
        public Guid UserId { get; }

        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}
