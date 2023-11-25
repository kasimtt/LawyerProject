using LawyerProject.Application.Features.Commands.AppUsers.CreateUser;
using LawyerProject.Application.Features.Commands.AppUsers.GoogleLogin;
using LawyerProject.Application.Features.Commands.AppUsers.LoginUser;
using LawyerProject.Application.Features.Commands.AppUsers.PasswordUpdate;
using LawyerProject.Application.Features.Queries.AppUser.GetUserByUserName;
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
            CreateUserCommandResponse response = await _mediator.Send(request);

            return Ok(response);

        }

        [HttpGet("[action]/{UserNameOrEmail}")]
        public async Task<IActionResult> GetUserByUserName([FromRoute] GetUserByUserNameQueryRequest request)
        {
            GetUserByUserNameQueryResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword2([FromBody] PasswordUpdateCommandRequest request)
        {
            PasswordUpdateCommandResponse response = await _mediator.Send(request);
            return Ok();
        }
    }
}
