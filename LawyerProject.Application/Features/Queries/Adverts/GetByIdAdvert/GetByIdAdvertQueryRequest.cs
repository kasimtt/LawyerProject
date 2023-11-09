using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Features.Queries.Adverts.GetByIdAdvert
{
    public class GetByIdAdvertQueryRequest : IRequest<GetByIdAdvertQueryResponse>
    {
        public string UserNameOrEmail { get; set; }
    }
}
