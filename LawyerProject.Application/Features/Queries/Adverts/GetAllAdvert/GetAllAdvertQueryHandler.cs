using AutoMapper;
using LawyerProject.Application.DTOs.AdvertsDtos;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.Features.Queries.Cases.GetAllCase;
using LawyerProject.Application.Repositories.AdvertRepositories;
using LawyerProject.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Features.Queries.Adverts.GetAllAdvert
{
    public class GetAllAdvertQueryHandler : IRequestHandler<GetAllAdvertQueryRequest, GetAllAdvertQueryResponse>
    {
        private readonly IAdvertReadRepository _advertReadRepository;
        private readonly IMapper _mapper;

        public GetAllAdvertQueryHandler(IAdvertReadRepository advertReadRepository, IMapper mapper)
        {
            _advertReadRepository = advertReadRepository;
            _mapper = mapper;
        }

        public async Task<GetAllAdvertQueryResponse> Handle(GetAllAdvertQueryRequest request, CancellationToken cancellationToken)
        {
           int totalCount =  _advertReadRepository.GetAll(false).Count();
           var result = _advertReadRepository.GetAll(false).Skip(request.Pagination.Page*request.Pagination.Size).Take(request.Pagination.Size).ToList();

            IEnumerable<GetAdvertDto> entityDto = _mapper.Map<IEnumerable<Advert>,IEnumerable<GetAdvertDto>>(result).ToList();

            return new GetAllAdvertQueryResponse
            {
                Adverts = entityDto,
                TotalCount = totalCount,
            };


        }
    }


}
