﻿using LawyerProject.Application.Features.Commands.AppUsers.GoogleLogin;
using LawyerProject.Application.Features.Commands.AppUsers.LoginUser;
using LawyerProject.Application.Features.Commands.AppUsers.RefreshTokenLogin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LawyerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommandRequest request)
        {
            LoginUserCommandResponse response = await _mediator.Send(request);
            return Ok(response); //response veya response.Token donucek. Buraya gelindiğinde client ile konuşulup anlaşılacak
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginCommandRequest request)
        {
            RefreshTokenLoginCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }



        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest request) //test icin clientten arayuz yapılmasını beklicez
        {
            GoogleLoginCommandResponse response = await _mediator.Send(request);
            return Ok(response);

        }
    }
}
