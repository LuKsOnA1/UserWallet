using EntityLayer.Models;
using MediatR;
using RepositoryLayer.Repositories.Abstract;
using RepositoryLayer.UnitOfWorks.Abstract;
using UserWalletService.MediatR.Commands;

namespace UserWalletService.MediatR.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserRequest, User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _repository;

        public CreateUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetGenericRepository<User>();
        }
        public async Task<User> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = request._userRequest.Name,
                Surname = request._userRequest.Surname,
                Email = request._userRequest.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request._userRequest.Password),
                Wallet = new Wallet()
            };

            await _repository.AddEntityAsync(user);
            await _unitOfWork.CommitAsync();

            return user;
        }
    }
}
