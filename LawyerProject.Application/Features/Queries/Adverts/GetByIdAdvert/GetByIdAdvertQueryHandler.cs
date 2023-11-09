using AutoMapper;
using LawyerProject.Application.DTOs.AdvertsDtos;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.Exceptions;
using LawyerProject.Application.Features.Queries.Cases.GetByIdCase;
using LawyerProject.Application.Repositories.AdvertRepositories;
using LawyerProject.Domain.Entities;
using LawyerProject.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;

        public GetByIdAdvertQueryHandler(IAdvertReadRepository advertReadRepository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _advertReadRepository = advertReadRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<GetByIdAdvertQueryResponse> Handle(GetByIdAdvertQueryRequest request, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByEmailAsync(request.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.UserNameOrEmail);
                if (user == null)
                {
                    throw new NotFoundUserException();
                }
            }


            IEnumerable<Advert> advert = _advertReadRepository.GetAll().Where(a => a.IdUserFK == user.Id
            && a.DataState == Domain.Enums.DataState.Active).ToList();
            IEnumerable<GetAdvertDto> getAdvertDto = _mapper.Map<IEnumerable<Advert>, IEnumerable<GetAdvertDto>>(advert);

            return new GetByIdAdvertQueryResponse
            {
                Advert = getAdvertDto,
            };
        }
    }
}
