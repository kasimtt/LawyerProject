using AutoMapper;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.Features.Queries.Cases.GetByUserIdCase;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Domain.Entities;
using LawyerProject.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public GetByUserIdCaseQueryHandler(ICaseReadRepository caseReadRepository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _caseReadRepository = caseReadRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<GetByUserIdCaseQueryResponse> Handle(GetByUserIdCaseQueryRequest request, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
            }

            IEnumerable<Case> cases = _caseReadRepository.Table.Include(c => c.User).Where(c => c.IdUserFK == user.Id).ToList();
            IEnumerable<GetCaseDto> getCaseDtos = _mapper.Map<IEnumerable<Case>, IEnumerable<GetCaseDto>>(cases).ToList();

            return new GetByUserIdCaseQueryResponse
            {
                GetCasesDto = getCaseDtos,
                TotalCount = getCaseDtos.Count()
            };
        }
    }
}
