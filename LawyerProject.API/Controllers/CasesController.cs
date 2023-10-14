﻿using AutoMapper;
using LawyerProject.Application.Abstractions.Storage;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.Repositories.CasePdfFileRepositories;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Application.Repositories.FileRepositories;
using LawyerProject.Application.Repositories.UserImageFileRepositories;
using LawyerProject.Application.RequestParameters;
using LawyerProject.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace LawyerProject.API.Controllers
{   //bu controller test icin oluşturulmuştur lütfen dalga geçmeyin :)
    [Route("api/[controller]")]
    [ApiController]
    public class CasesController : ControllerBase
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

        public CasesController(ICaseReadRepository caseReadRepository,
                               ICaseWriteRepository caseWriteRepository,
                               IMapper mapper,
                               IFileReadRepository fileReadRepository,
                               IFileWriteRepository fileWriteRepository,
                               IUserImageFileReadRepository userImageFileReadRepository,
                               IUserImageFileWriteRepository userImageFileWriteRepository,
                               ICasePdfFileWriteRepository casePdfFileWriteRepository,
                               IUserImageFileReadRepository currentUserImageFileReadRepository,
                               IStorageService storageService)
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
        }


        [HttpGet("getall")]
        public IActionResult Get([FromQuery] Pagination pagination)
        {
            // throw new Exception("hata var kardaş hele bak buraya");
            var totalCount = _caseReadRepository.GetAll().Count();
            var result = _caseReadRepository.GetAll().Skip(pagination.Size * pagination.Page).Take(pagination.Size);
            // gecici bunu yapıyorum daha sonra bu işlemler service classlarına alınacak haberin olsun
            IEnumerable<GetCasesDto> entityDto = _mapper.Map<IEnumerable<Case>, IEnumerable<GetCasesDto>>(result);
            return Ok(new
            {
                totalCount,
                entityDto,
            });
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Post([FromBody] CreateCaseDto createCaseDto) // şimdilik dto şeklinde gönderiyoruz bunları CQRS pattern'e göre düzenlicez
        {
            Case _case = _mapper.Map<Case>(createCaseDto);
            var result = await _caseWriteRepository.AddAsync(_case);
            await _caseWriteRepository.SaveAsync();

            if (result)
            {
                return Ok("başarıyla kaydedildi panpa");
            }
            return BadRequest("kaydetme başarısız");
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
        public async Task<IActionResult> Upload([FromHeader] IFormFileCollection files)// Request.Form.Files ==>angulardan aldığımızda fromform kullanmak yerine request ederek dosyamıza ulasıcağız 
        {
           List<(string fileName, string pathOrContainer)> datas =  await _storageService.UploadAsync("cases-image", files);
           await _casePdfFileWriteRepository.AddRangeAsync(datas.Select(r => new CasePdfFile
            {
                FileName = r.fileName,
                CreatedDate = DateTime.Now,
                Path = r.pathOrContainer,
                DataState = Domain.Enums.DataState.Active,
                Storage =_storageService.StorageName,
                

            }).ToList()) ;
            return Ok();
        }
    }
} 
