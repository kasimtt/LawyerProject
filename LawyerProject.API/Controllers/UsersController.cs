using LawyerProject.Application.Features.Commands.AppUsers.CreateUser;
using LawyerProject.Application.Features.Commands.AppUsers.LoginUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LawyerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("[action]")] // action'u client isterse kaldırız
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommadRequest request)
        {
          CreateUserCommandResponse response=  await _mediator.Send(request);
            if(response.Success)
            {
                return Ok();
            }
           return BadRequest();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommandRequest request)
        {
            LoginUserCommandResponse response = await _mediator.Send(request);
            return Ok(response.Token); //response veya response.Token donucek. Buraya gelindiğinde client ile konuşulup anlaşılacak
        }


    }
}
