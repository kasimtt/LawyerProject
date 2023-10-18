using C= LawyerProject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LawyerProject.Application.Repositories;
using LawyerProject.Application.Repositories.CaseRepositories;
using AutoMapper;

namespace LawyerProject.Application.Features.Commands.Case.UpdateCase
{
    public class UpdateCaseCommandHandler : IRequestHandler<UpdateCaseCommandRequest, UpdateCaseCommandResponse>
    {
        private readonly ICaseWriteRepository _caseWriteRepository;
        private readonly IMapper _mapper;

        public UpdateCaseCommandHandler(ICaseWriteRepository caseWriteRepository, IMapper mapper)
        {
           _caseWriteRepository = caseWriteRepository;
            _mapper = mapper;

        }
        public async Task<UpdateCaseCommandResponse> Handle(UpdateCaseCommandRequest request, CancellationToken cancellationToken)
        {
            C.Case _case = _mapper.Map<C.Case>(request);
             bool result = _caseWriteRepository.Update(_case);
            await _caseWriteRepository.SaveAsync();
           
            if (result)
            {
                await _caseWriteRepository.SaveAsync();
                return new UpdateCaseCommandResponse
                {
                    success = true
                };
            }
            else
            {
                return new UpdateCaseCommandResponse { success = false };
            }

        }
    }
}
