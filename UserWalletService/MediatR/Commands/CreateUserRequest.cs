using EntityLayer.DTOs;
using EntityLayer.Models;
using MediatR;

namespace UserWalletService.MediatR.Commands
{
    public class CreateUserRequest : IRequest<User>
    {
        public UserDTO _userRequest { get; }

        public CreateUserRequest(UserDTO userRequest)
        {
            _userRequest = userRequest;
        }
    }
}
