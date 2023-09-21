using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LawyerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CasesController : ControllerBase
    {
        private readonly ICaseReadRepository _caseReadRepository; //test icin şimdilik repositoryler ile calışıyoruz
        private readonly ICaseWriteRepository _caseWriteRepository;
        public CasesController(ICaseReadRepository caseReadRepository, ICaseWriteRepository caseWriteRepository )
        {
            _caseReadRepository = caseReadRepository;
            _caseWriteRepository = caseWriteRepository; 
        }


        [HttpGet("getall")]
        public IActionResult Get()
        {
           // throw new Exception("hata var kardaş hele bak buraya");
            var result = _caseReadRepository.GetAll();
            return Ok(result);
        }

        [HttpPost("Add")]
        public IActionResult Post([FromBody]Case _case) // şimdilik case şeklinde gönderiyoruz bunları CQRS pattern'e göre düzenlicez
        {
            _caseWriteRepository.Add(_case);
            return Ok();
        }
    }
}
