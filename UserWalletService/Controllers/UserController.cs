using EntityLayer.DTOs;
using EntityLayer.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserWalletService.MediatR.Commands;
using UserWalletService.MediatR.Queries;

namespace UserWalletService.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllUser()
        {
            // Specifying the query that i have for this endpoint 
            var query = new GetAllUsersQuery();

            var result = await _mediator.Send(query);

            return Ok(result);
        }



        [HttpGet("{id:Guid}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid Id");
            }

            var query = new GetUserQuery(id);

            var result = await _mediator.Send(query);

            if(result == null)
            {
                return NotFound("User with given id does not exist!");
            }

            return Ok(result);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDTO model)
        {

            var command = new CreateUserRequest(model);

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
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

            var command = new RemoveUserRequest(id);

            var result = await _mediator.Send(command);

            if(result == null)
            {
                return NotFound("User with given id does not exist");
            }

            return NoContent();
        }




        [HttpPost("{senderId}/Transfer/{recipientId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TransferMoneyDTO>> TransferMoney(Guid senderId, Guid recipientId, [FromBody] decimal amount)
        {
            if (amount <= 1)
            {
                return BadRequest("Minimum amount of transaction is 1");
            }

            var command = new TransferMoneyRequest(senderId, recipientId, amount);
            
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return BadRequest("Something went wrong. Please check sender and recipient Id or balance and try again!");
            }
            return Ok(result);
        }

    }
}
