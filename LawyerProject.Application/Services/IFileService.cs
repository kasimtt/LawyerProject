using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Services
{
    public interface IFileService
    {
        Task<List<(string fileName,string path)>> UploadAsync(string path, IFormFileCollection files);
        Task<bool> CopyFileAsync(string path, IFormFile file);

    }
}
