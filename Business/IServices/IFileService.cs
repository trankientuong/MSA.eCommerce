using Microsoft.AspNetCore.Http;

namespace Business.IServices
{
    public interface IFileService
    {
        Task<string> SaveFile(IFormFile file, string folder);

        Task DeleteFile(string fileName, string folder);
    }
}