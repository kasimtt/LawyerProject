using AutoMapper;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = LawyerProject.Domain.Entities;
namespace LawyerProject.Application.Features.Commands.CreateCase
{
    public class CreateCaseCommandHandler : IRequestHandler<CreateCaseCommandRequest, CreateCaseCommandResponse>
    {
        private readonly ICaseWriteRepository _caseWriteRepository;
        private readonly IMapper _mapper;
        public CreateCaseCommandHandler(ICaseWriteRepository caseWriteRepository, IMapper mapper)
        {
            _caseWriteRepository = caseWriteRepository;
            _mapper = mapper;
        }
        public async Task<CreateCaseCommandResponse> Handle(CreateCaseCommandRequest request, CancellationToken cancellationToken)
        {
            C.Case _case = _mapper.Map<C.Case>(request);

            await _caseWriteRepository.AddAsync(_case);
            await _caseWriteRepository.SaveAsync();

            return new CreateCaseCommandResponse
            {
                success = true,
            };
        }
    }
}
