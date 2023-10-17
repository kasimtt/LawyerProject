using AutoMapper;
using LawyerProject.Application.Abstractions.Storage;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.Features.Commands.CreateCase;
using LawyerProject.Application.Features.Queries.GetAllCase;
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

        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromBody] CreateCaseCommandRequest CreateCaseCommandRequest) // şimdilik dto şeklinde gönderiyoruz bunları CQRS pattern'e göre düzenlicez
        {
             await _mediator.Send(CreateCaseCommandRequest);
            return Ok("başarıyla eklendi");
        }

        [HttpPut("Update")]
        public IActionResult Put([FromBody] UpdateCaseDto updateCaseDto)
        {
            Case _case = _mapper.Map<Case>(updateCaseDto);
            var result = _caseWriteRepository.Update(_case);
            _caseWriteRepository.Save();
            if (result)
            {
                return Ok("başarıyla güncellendi panpa");
            }
            return BadRequest("Güncelleme sırasında bir hata ile karşılaşıldı");

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromHeader] IFormFileCollection files, int id)// Request.Form.Files ==>angulardan aldığımızda fromform kullanmak yerine request ederek dosyamıza ulasıcağız 
        {

            var _case = await _caseReadRepository.GetByIdAsync(id);

           List<(string fileName, string pathOrContainer)> datas =  await _storageService.UploadAsync("cases-image", files);
           await _casePdfFileWriteRepository.AddRangeAsync(datas.Select(r => new CasePdfFile
            {
                FileName = r.fileName,
                CreatedDate = DateTime.Now,
                Path = r.pathOrContainer,
                Storage =_storageService.StorageName,
                Cases = new List<Case> { _case}

            }).ToList()) ;

            await _casePdfFileWriteRepository.SaveAsync() ; 
            return Ok();
        }

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
            Case? _case = await _caseReadRepository.Table.Include(c=>c.CasePdfFiles).FirstOrDefaultAsync(c=>c.ObjectId == id); // case(dava) bulunuyor
            CasePdfFile? casePdfFile = _case?.CasePdfFiles?.FirstOrDefault(p=>p.ObjectId == imageId); // davanın dosyaları bulunuyor
            _case.CasePdfFiles.Remove(casePdfFile);  //dosya silinip kaydediliyor
            await _caseWriteRepository.SaveAsync(); // veritabanına kayıt yapılıyor.// dosyalarda mahkeme ve yasal bilgiler bulunduğu için komple silme yetkisi kullanıcılara verilecek.

            return Ok("silindi");
        }
    }
} 
