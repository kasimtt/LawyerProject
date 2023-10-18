using AutoMapper;
using LawyerProject.Application.Abstractions.Storage;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.Features.Commands.Case.DeleteCase;
using LawyerProject.Application.Features.Commands.Case.UpdateCase;
using LawyerProject.Application.Features.Commands.CasePdfFiles.RemoveCasePdfFile;
using LawyerProject.Application.Features.Commands.CasePdfFiles.UploadCasePdfFile;
using LawyerProject.Application.Features.Commands.CreateCase;
using LawyerProject.Application.Features.Queries.Cases.GetAllCase;
using LawyerProject.Application.Features.Queries.Cases.GetByIdCase;
using LawyerProject.Application.Repositories.CasePdfFileRepositories;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Application.Repositories.FileRepositories;
using LawyerProject.Application.Repositories.UserImageFileRepositories;
using LawyerProject.Application.RequestParameters;
using LawyerProject.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace LawyerProject.API.Controllers
{   //bu controller test icin oluşturulmuştur lütfen dalga geçmeyin :)
    [Route("api/[controller]")]
    [ApiController]
    public class CasesTestController : ControllerBase
    {
        private readonly ICaseReadRepository _caseReadRepository; //test icin şimdilik repositoryler ile calışıyoruz
        private readonly ICaseWriteRepository _caseWriteRepository;
        private readonly IMapper _mapper;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IUserImageFileReadRepository _userImageFileReadRepository;
        private readonly IUserImageFileWriteRepository _userImageFileWriteRepository;
        private readonly ICasePdfFileWriteRepository _casePdfFileWriteRepository;
        private readonly IUserImageFileReadRepository _currentUserImageFileReadRepository;
        private readonly IStorageService _storageService; 
        private readonly IConfiguration _configuration;

        private readonly IMediator _mediator;

        public CasesTestController(ICaseReadRepository caseReadRepository,
                               ICaseWriteRepository caseWriteRepository,
                               IMapper mapper,
                               IFileReadRepository fileReadRepository,
                               IFileWriteRepository fileWriteRepository,
                               IUserImageFileReadRepository userImageFileReadRepository,
                               IUserImageFileWriteRepository userImageFileWriteRepository,
                               ICasePdfFileWriteRepository casePdfFileWriteRepository,
                               IUserImageFileReadRepository currentUserImageFileReadRepository,
                               IStorageService storageService,
                               IConfiguration configuration,
                               IMediator mediator)
        {
            _caseReadRepository = caseReadRepository;
            _caseWriteRepository = caseWriteRepository;
            _mapper = mapper;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _userImageFileReadRepository = userImageFileReadRepository;
            _userImageFileWriteRepository = userImageFileWriteRepository;
            _casePdfFileWriteRepository = casePdfFileWriteRepository;
            _currentUserImageFileReadRepository = currentUserImageFileReadRepository;
            _storageService = storageService;
            _configuration = configuration;
            _mediator = mediator;
        }


        [HttpGet("getall")]
        public async Task<IActionResult> Get([FromQuery] GetAllCaseQueryRequest getAllCaseQueryRequest)
        {
           GetAllCaseQueryResponse getAllCaseQueryResponse =  await _mediator.Send(getAllCaseQueryRequest);
           return Ok(getAllCaseQueryResponse);
        }
        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> GetById([FromRoute] GetByIdCaseQueryRequest getByIdCaseQueryRequest)
        {
            GetByIdCaseQueryResponse getByIdCaseQueryResponse =await _mediator.Send(getByIdCaseQueryRequest);
            return Ok(getByIdCaseQueryResponse.GetCaseDtos);

        }

        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromBody] CreateCaseCommandRequest CreateCaseCommandRequest) // şimdilik dto şeklinde gönderiyoruz bunları CQRS pattern'e göre düzenlicez
        {
             await _mediator.Send(CreateCaseCommandRequest);
            return Ok("başarıyla eklendi");
        }

        [HttpPut("Update")]
        public async  Task<IActionResult> Put([FromBody] UpdateCaseCommandRequest updateCaseCommandRequest)
        {

            UpdateCaseCommandResponse updateCaseCommandResponse= await _mediator.Send(updateCaseCommandRequest);
            if(updateCaseCommandResponse.success)
                return Ok();
            else 
                return BadRequest();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromHeader] IFormFileCollection files, int id)// Request.Form.Files ==>angulardan aldığımızda fromform kullanmak yerine request ederek dosyamıza ulasıcağız 
        {
            UploadCasePdfFileCommandRequest uploadCasePdfFileCommandRequest = new UploadCasePdfFileCommandRequest
            {
                FormFiles = files,  //Request.Form.Files yazılacak
                Id = id
            };
             await _mediator.Send(uploadCasePdfFileCommandRequest);
            return Ok();
        }
                                                                                               //from form veya from query olarak güncelleyebiliriz
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetProductImage(int id) {

           Case? Case =   await _caseReadRepository.Table.Include(c => c.CasePdfFiles).FirstOrDefaultAsync(c => c.ObjectId == id);

            return Ok(Case?.CasePdfFiles?.Select(c => new
            {
                Path = $"{_configuration["BaseStorageUrl"]}/{c.Path}",
                c.FileName
            }));

        }
         
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteImage(int id, int imageId)
        {
           RemoveCasePdfFileCommandResponse removeCasePdfFileCommandResponse =   await _mediator.Send(new RemoveCasePdfFileCommandRequest { Id = id, ImageId = imageId});
            if (removeCasePdfFileCommandResponse.Success)
                return Ok("silindi");
            else
                return BadRequest();
        }

        [HttpPost("[action]/{Id}")]

        public async Task<IActionResult> Delete([FromRoute] DeleteCaseCommandRequest deleteCaseCommandRequest)
        {
           DeleteCaseCommandResponse deleteCaseCommandResponse =  await _mediator.Send(deleteCaseCommandRequest);
            if (deleteCaseCommandResponse.Success)
            {
                return Ok();
            }
            else
                return BadRequest();
           
        }


    }
} 
