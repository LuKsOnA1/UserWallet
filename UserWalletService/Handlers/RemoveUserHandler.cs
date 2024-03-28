using EntityLayer.Models;
using MediatR;
using RepositoryLayer.Repositories.Abstract;
using RepositoryLayer.UnitOfWorks.Abstract;
using UserWalletService.Commands;

namespace UserWalletService.Handlers
{
    public class RemoveUserHandler : IRequestHandler<RemoveUserRequest, User>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _repository;

        public RemoveUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetGenericRepository<User>();
        }

        public async Task<User> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetEntityByIdAsync(request._id);
            if (user == null)
            {
                return null;
            }

            _repository.DeleteEntity(user);
            await _unitOfWork.CommitAsync();

            return user;
        }
    }
}
