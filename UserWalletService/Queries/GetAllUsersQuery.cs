using EntityLayer.Models;
using MediatR;

namespace UserWalletService.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<User>>
    {

    }
}
