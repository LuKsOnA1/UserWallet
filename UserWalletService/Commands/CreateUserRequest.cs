using EntityLayer.DTOs;
using EntityLayer.Models;
using MediatR;

namespace UserWalletService.Commands
{
    public class CreateUserRequest : IRequest<User>
    {
        public CreateUserDTO _userRequest { get; }

        public CreateUserRequest(CreateUserDTO userRequest)
        {
            _userRequest = userRequest;
        }
    }
}
