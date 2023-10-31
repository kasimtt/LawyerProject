using T= LawyerProject.Application.DTOs.TokenDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        T.Token CreateAccessToken(int minute);
        string CreateRefreshToken();
    }
}
