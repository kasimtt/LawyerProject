using AutoMapper;
using LawyerProject.Application.DTOs.AdvertsDtos;
using LawyerProject.Application.Features.Queries.Cases.GetByIdCase;
using LawyerProject.Application.Repositories.AdvertRepositories;
using LawyerProject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Features.Queries.Adverts.GetByIdAdvert
{
    public class GetByIdAdvertQueryHandler : IRequestHandler<GetByIdAdvertQueryRequest, GetByIdAdvertQueryResponse>
    {
        private readonly IAdvertReadRepository _advertReadRepository;
        private readonly IMapper _mapper;

        public GetByIdAdvertQueryHandler(IAdvertReadRepository advertReadRepository, IMapper mapper)
        {
            _advertReadRepository = advertReadRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdAdvertQueryResponse> Handle(GetByIdAdvertQueryRequest request, CancellationToken cancellationToken)
        {
           Advert advert = _advertReadRepository.GetById(request.Id);
           GetAdvertDto getAdvertDto = _mapper.Map<GetAdvertDto>(advert);

            return new GetByIdAdvertQueryResponse
            {
                Advert = getAdvertDto,
            };
        }
    }
}
