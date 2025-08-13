using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadImageAsync(IFormFile file, string entityType);
        Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> files, string entityType);
        Task DeleteImageAsync(string imageUrl);
        Task<bool> ValidateImageFile(IFormFile file, int maxSizeMB = 5);
    }
}
