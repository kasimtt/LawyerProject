using AutoMapper;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Features.Commands.CreateCase
{
    public class CreateCaseCommandHandle : IRequestHandler<CreateCaseCommandRequest, CreateCaseCommandResponse>
    {
        private readonly ICaseWriteRepository _caseWriteRepository;
        private readonly IMapper _mapper;
        public CreateCaseCommandHandle(ICaseWriteRepository caseWriteRepository, IMapper mapper)
        {
            _caseWriteRepository = caseWriteRepository;
            _mapper = mapper;
        }
        public async Task<CreateCaseCommandResponse> Handle(CreateCaseCommandRequest request, CancellationToken cancellationToken)
        {
            Case _case = _mapper.Map<Case>(request);

            await _caseWriteRepository.AddAsync(_case);
            await _caseWriteRepository.SaveAsync();

            return new CreateCaseCommandResponse
            {
                success = true,
            };
        }
    }
}
