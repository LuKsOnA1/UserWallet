using EntityLayer.DTOs;
using EntityLayer.Models;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Repositories.Abstract;
using RepositoryLayer.UnitOfWorks.Abstract;

namespace UserWalletService.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<User> _repository;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.GetGenericRepository<User>();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllUser()
        {
            //var users = _repository.GetAllEntity();
            var users = await _repository.GetAllIncluding(x => x.Wallet, x => x.Wallet.Transactions);
            return Ok(users);
        }



        [HttpGet("{id:Guid}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid ID");
            }

            var user = await _repository.GetIncludeAsync(filter:  x => x.Id == id, includeProperties: "Wallet,Wallet.Transactions");
            if (user == null)
            {
                return NotFound("User with given ID does not exist!");
            }


            return Ok(user);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserDTO model)
        {
            var user = new User
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Wallet = new Wallet()
            };

            await _repository.AddEntityAsync(user);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }



        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Please Enter Valid ID");
            }

            var user = await _repository.GetEntityByIdAsync(id);
            if (user == null)
            {
                return NotFound("User Does Not Exist!");
            }

            _repository.DeleteEntity(user);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }




        [HttpPost("{senderId}/Transfer/{recipientId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TransferMoneyDTO>> TransferMoney(Guid senderId, Guid recipientId, [FromBody] decimal amount)
        {


            var senderUserId = await _repository.GetIncludeAsync(filter: x => x.Id == senderId, includeProperties: "Wallet,Wallet.Transactions");
            var senderWalletId = senderUserId.Wallet.Id;

            var recipientUserId = await _repository.GetIncludeAsync(filter: x => x.Id == recipientId, includeProperties: "Wallet,Wallet.Transactions");
            var recipientWalletId = recipientUserId.Wallet.Id;

            if (senderUserId == null || recipientUserId == null)
            {
                return NotFound("Sender or Recipient does not Exist!");
            }

            if (senderUserId.Wallet.Balance < amount)
            {
                return BadRequest("Insufficient funds!");
            }

            senderUserId.Wallet.Balance -= amount;
            recipientUserId.Wallet.Balance += amount;

            var transaction = new Transaction
            {
                SenderUserId = senderUserId.Id,
                RecipientUserId = recipientUserId.Id,
                Amount = amount,
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

            return Ok(finalTransactionDTO);
        }

    }
}
