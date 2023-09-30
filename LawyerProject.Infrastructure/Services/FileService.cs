

using LawyerProject.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace LawyerProject.Infrastructure.Services
{
    public class FileService 
    {
        //bunu merkezi bir yere alıcaz haberin olsun panpa
        private async Task<string> RenameFileAsync(string path, string fileName)
        {
            return await Task.Run<string>(() =>
            {
                string oldName = Path.GetFileNameWithoutExtension(fileName);
                string extension = Path.GetExtension(fileName);
                string newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
                bool fileIsExists = false;
                int fileIndex = 0;
                do
                {
                    if (File.Exists($"{path}\\{newFileName}"))
                    {
                        fileIsExists = true;
                        fileIndex++;
                        newFileName = $"{NameOperation.CharacterRegulatory(oldName + "-" + fileIndex)}{extension}";
                    }
                    else
                    {
                        fileIsExists = false;
                    }
                } while (fileIsExists);

                return newFileName;
            });

        }

   
    }
}
