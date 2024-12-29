// Services/IFileService.cs
using Microsoft.AspNetCore.Http;

namespace CrowdFundingApp.Services
{
    public interface IFileService
    {
        Task<string> UploadImageAsync(IFormFile file);
        void DeleteImage(string imageUrl);
    }
}