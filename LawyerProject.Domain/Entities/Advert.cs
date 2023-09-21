﻿using LawyerProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Domain.Entities
{
    public class Advert : BaseEntity
    {
        public CaseType CaseType { get; set; } 
        public DateTime CaseDate { get; set; }
        public decimal Price { get; set; }  
        public string City { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string CasePlace { get; set; }



        /*-->ilanlar
** DavaKonusu
** DavaTarihi  -->cakişma bakilacak
** DavaUcreti
** şehir
** ilçe
** açık adres
** duruşma yeri(7.sulh mahkemesi vs)


*/
    }
}
