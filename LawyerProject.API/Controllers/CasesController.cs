using AutoMapper;
using LawyerProject.Application.Abstractions.Storage;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.Features.Commands.Case.DeleteCase;
using LawyerProject.Application.Features.Commands.Case.UpdateCase;
using LawyerProject.Application.Features.Commands.CasePdfFiles.RemoveCasePdfFile;
using LawyerProject.Application.Features.Commands.CasePdfFiles.UploadCasePdfFile;
using LawyerProject.Application.Features.Commands.CreateCase;
using LawyerProject.Application.Features.Queries.CasePdfFiles.GetCasePdfFile;
using LawyerProject.Application.Features.Queries.Cases.GetAllCase;
using LawyerProject.Application.Features.Queries.Cases.GetByIdCase;
using LawyerProject.Application.Features.Queries.Cases.GetByUserIdCase;
using LawyerProject.Application.Repositories.CasePdfFileRepositories;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Application.Repositories.FileRepositories;
using LawyerProject.Application.Repositories.UserImageFileRepositories;
using LawyerProject.Application.RequestParameters;
using LawyerProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;

namespace LawyerProject.API.Controllers
{   //bu controller test icin oluşturulmuştur lütfen dalga geçmeyin :)
    [Route("api/[controller]")]
    [ApiController]
    public class CasesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CasesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getall")]  //ileride lazım olması durumunda pagination işlemi eklenecek.
        public async Task<IActionResult> Get([FromQuery] GetAllCaseQueryRequest getAllCaseQueryRequest)
        {
            GetAllCaseQueryResponse getAllCaseQueryResponse = await _mediator.Send(getAllCaseQueryRequest);
            return Ok(getAllCaseQueryResponse);
        }
        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdCaseQueryRequest getByIdCaseQueryRequest)
        {
            GetByIdCaseQueryResponse getByIdCaseQueryResponse = await _mediator.Send(getByIdCaseQueryRequest);
            return Ok(getByIdCaseQueryResponse.GetCaseDtos);

        }

        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromBody] CreateCaseCommandRequest CreateCaseCommandRequest) // şimdilik dto şeklinde gönderiyoruz bunları CQRS pattern'e göre düzenlicez
        {
            await _mediator.Send(CreateCaseCommandRequest);
            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Put([FromBody] UpdateCaseCommandRequest updateCaseCommandRequest)
        {

            UpdateCaseCommandResponse updateCaseCommandResponse = await _mediator.Send(updateCaseCommandRequest);
            if (updateCaseCommandResponse.success)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Upload([FromRoute] int id)// Request.Form.Files ==>angulardan aldığımızda fromform kullanmak yerine request ederek dosyamıza ulasıcağız 
        {
            UploadCasePdfFileCommandRequest uploadCasePdfFileCommandRequest = new UploadCasePdfFileCommandRequest
            {
                FormFiles = Request.Form.Files,  //Request.Form.Files yazılacak
                Id = id
            };
            await _mediator.Send(uploadCasePdfFileCommandRequest);
            return Ok();
        }
        //from form veya from query olarak güncelleyebiliriz
        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetCasePdfFile([FromRoute] GetCasePdfFileQueryRequest getCasePdfFileQueryRequest)
        {

            List<GetCasePdfFileQueryResponse> getCasePdfFileQueryResponse = await _mediator.Send(getCasePdfFileQueryRequest);
            return Ok(getCasePdfFileQueryResponse);


        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteImage(int id, int imageId)
        {
            RemoveCasePdfFileCommandResponse removeCasePdfFileCommandResponse = await _mediator.Send(new RemoveCasePdfFileCommandRequest { Id = id, ImageId = imageId });
            if (removeCasePdfFileCommandResponse.Success)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost("[action]/{Id}")]

        public async Task<IActionResult> Delete([FromRoute] DeleteCaseCommandRequest deleteCaseCommandRequest)
        {
            DeleteCaseCommandResponse deleteCaseCommandResponse = await _mediator.Send(deleteCaseCommandRequest);
            if (deleteCaseCommandResponse.Success)
            {
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpGet("[action]/{Id}")]

        public async Task<IActionResult> GetByUserId([FromRoute] GetByUserIdCaseQueryRequest request)
        {
            GetByUserIdCaseQueryResponse response = await _mediator.Send(request);
            return Ok(response.GetCasesDto);
        }

    }
}
