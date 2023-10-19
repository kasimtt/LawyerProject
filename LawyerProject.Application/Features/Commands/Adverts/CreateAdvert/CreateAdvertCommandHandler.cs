using AutoMapper;
using LawyerProject.Application.Repositories.AdvertRepositories;
using LawyerProject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Features.Commands.Adverts.CreateAdvert
{
    public class CreateAdvertCommandHandler : IRequestHandler<CreateAdvertCommandRequest, CreateAdvertCommandResponse>
    {
        private readonly IAdvertWriteRepository _advertWriteRepository;
        private readonly IMapper _mapper;

        public CreateAdvertCommandHandler(IAdvertWriteRepository advertWriteRepository, IMapper mapper)
        {
            _advertWriteRepository = advertWriteRepository;
            _mapper = mapper;
        }

        public async Task<CreateAdvertCommandResponse> Handle(CreateAdvertCommandRequest request, CancellationToken cancellationToken)
        {
           Advert advert = _mapper.Map<Advert>(request);
           bool result =  await _advertWriteRepository.AddAsync(advert);
            if (result)
            {
                await _advertWriteRepository.SaveAsync();
            }
            return new CreateAdvertCommandResponse
            {
                Success = result,
            };

            
            

        }
    }
}
