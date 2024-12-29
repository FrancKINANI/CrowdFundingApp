// Services/FileService.cs
using Microsoft.AspNetCore.Hosting;

namespace CrowdFundingApp.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadsFolder;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadsFolder = Path.Combine(environment.WebRootPath, "uploads", "projects");
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null) return null;

            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(_uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/projects/{uniqueFileName}";
        }

        public void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            string filePath = Path.Combine(_environment.WebRootPath, imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}