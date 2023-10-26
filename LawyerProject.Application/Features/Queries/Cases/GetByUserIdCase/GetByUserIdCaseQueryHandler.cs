using AutoMapper;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.Features.Queries.Cases.GetByUserIdCase;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Features.Queries.Cases.GetByUserIdCase
{
    public class GetByUserIdCaseQueryHandler : IRequestHandler<GetByUserIdCaseQueryRequest, GetByUserIdCaseQueryResponse>
    {
        private readonly ICaseReadRepository _caseReadRepository;
        private readonly IMapper _mapper;

        public GetByUserIdCaseQueryHandler(ICaseReadRepository caseReadRepository, IMapper mapper)
        {
            _caseReadRepository = caseReadRepository;
            _mapper = mapper;
        }

        public async Task<GetByUserIdCaseQueryResponse> Handle(GetByUserIdCaseQueryRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<Case> cases = _caseReadRepository.Table.Include(c => c.User).Where(c => c.IdUserFK == request.Id).ToList();
            IEnumerable<GetCaseDto> getCaseDtos = _mapper.Map<IEnumerable<Case>,IEnumerable<GetCaseDto>>(cases).ToList();

            return new GetByUserIdCaseQueryResponse
            {
                GetCasesDto = getCaseDtos,
                TotalCount = getCaseDtos.Count()
            };
        }
    }
}
