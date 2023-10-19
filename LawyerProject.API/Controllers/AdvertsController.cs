﻿using LawyerProject.Application.Features.Commands.Adverts.CreateAdvert;
using LawyerProject.Application.Features.Commands.Adverts.DeleteAdvert;
using LawyerProject.Application.Features.Commands.Adverts.UpdateAdvert;
using LawyerProject.Application.Features.Queries.Adverts.GetAllAdvert;
using LawyerProject.Application.Features.Queries.Adverts.GetByIdAdvert;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LawyerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertsController : ControllerBase
    {
        public IMediator mediator;
        public AdvertsController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateAdvert([FromBody] CreateAdvertCommandRequest request)
        {
            CreateAdvertCommandResponse  response =  await mediator.Send(request);
            if(response.Success)
            {
                return Ok("Başarıyla kaydedilmiştir");
            }
            return BadRequest("Kaydedilemedi");
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateAdvert([FromBody] UpdateAdvertCommandRequest request)
        {
            UpdateAdvertCommandResponse response = await mediator.Send(request);
            if(response.Success)
            {
                return Ok("başarıyla güncellenmiştir");
            }
            return BadRequest("güncellenemedi");
        }
        
        [HttpPut("[action]/{Id}")]
        public async Task<IActionResult> DeleteAdvert([FromRoute] DeleteAdvertCommandRequest request)
        {
           DeleteAdvertCommandResponse response = await mediator.Send(request);
            if(response.Success)
            {
               return Ok("başarıyla silindi");
            }
           return BadRequest("silinemedi");

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllAdvertQueryRequest request)
        {
            GetAllAdvertQueryResponse response = await mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("[action]/{Id}")] 
        public async Task<IActionResult> GetById([FromRoute] GetByIdAdvertQueryRequest request)
        {
            GetByIdAdvertQueryResponse response = await mediator.Send(request);
            return Ok(response); 
        }


    }
}
