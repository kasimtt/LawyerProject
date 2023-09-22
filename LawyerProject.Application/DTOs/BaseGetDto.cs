﻿using LawyerProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.DTOs
{
    public class BaseGetDto
    {
      
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        //public DataState DataState { get; set; } = DataState.Active;
    }
}
