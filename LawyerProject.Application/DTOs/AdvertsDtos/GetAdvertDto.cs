using LawyerProject.Domain.Entities;
using LawyerProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.DTOs.AdvertsDtos
{
    public class GetAdvertDto : BaseGetDto
    {
        //public User? User { get; set; }
        public int ObjectId { get; set; }
        public CaseType CaseType { get; set; }
        public DateTime CaseDate { get; set; }
        public decimal Price { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string CasePlace { get; set; }

    }
}
