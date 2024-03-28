using EntityLayer.Models;
using MediatR;

namespace UserWalletService.MediatR.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<User>>
    {

    }
}
