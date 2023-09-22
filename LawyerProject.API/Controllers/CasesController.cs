using AutoMapper;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Application.RequestParameters;
using LawyerProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LawyerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasesController : ControllerBase
    {
        private readonly ICaseReadRepository _caseReadRepository; //test icin şimdilik repositoryler ile calışıyoruz
        private readonly ICaseWriteRepository _caseWriteRepository;
        private readonly IMapper _mapper;
        public CasesController(ICaseReadRepository caseReadRepository, ICaseWriteRepository caseWriteRepository, IMapper mapper)
        {
            _caseReadRepository = caseReadRepository;
            _caseWriteRepository = caseWriteRepository;
            _mapper = mapper;
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
        public IActionResult Post([FromBody] CreateCaseDto createCaseDto) // şimdilik case şeklinde gönderiyoruz bunları CQRS pattern'e göre düzenlicez
        {
            Case _case = _mapper.Map<Case>(createCaseDto);
            var result = _caseWriteRepository.Add(_case);
            _caseWriteRepository.Save();

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
            return BadRequest("Güncelleme yapılamadı");

        }
    }
}
