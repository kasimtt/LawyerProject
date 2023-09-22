using AutoMapper;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Mappers
{
    public class CasesProfile : Profile
    {
        public CasesProfile() {
        
           CreateMap<Case,CreateCaseDto>().ReverseMap();
           CreateMap<Case,UpdateCaseDto>().ReverseMap();
           CreateMap<Case,GetCasesDto>().ReverseMap();
        }
    }
}
