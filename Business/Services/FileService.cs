using Business.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Business.Services;

public class FileService : IFileService
{
    public async Task DeleteFile(string fileName, string folder)
        {
            string folderPath = Path.Combine(WebHostEnviromentHelper.GetWebRootPath(), folder);
            string path = Path.Combine(folderPath, fileName);          
            if(File.Exists(path)) {
                await Task.Run(() => File.Delete(path));
            }
        }

        public async Task<string> SaveFile(IFormFile file, string folder)
        {
            string fileName = $"{Guid.NewGuid()}.jpeg";
            string folderPath = Path.Combine(WebHostEnviromentHelper.GetWebRootPath(), folder);
            string path = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
}