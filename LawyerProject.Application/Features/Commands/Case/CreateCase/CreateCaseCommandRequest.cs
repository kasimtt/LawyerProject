using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Features.Commands.CreateCase
{
    public class CreateCaseCommandRequest: IRequest<CreateCaseCommandResponse>
    {
        public int IdUserFK { get; set; }
        public int CaseNumber { get; set; }
        public string CaseNot { get; set; } = string.Empty;
        public string CaseDescription { get; set; } = string.Empty;
        public CaseType CaseType { get; set; }
        public string Files { get; set; } = string.Empty;
        public DateTime? CaseDate { get; set; }
    }
}
