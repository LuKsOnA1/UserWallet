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
            var users = _repository.GetAllEntity();
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

            var user = await _repository.GetEntityByIdAsync(id);
            if (user == null)
            {
                return NotFound("User with given ID does not exist!");
            }


            return Ok(user);
        }



        [HttpPost]
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


    }
}
