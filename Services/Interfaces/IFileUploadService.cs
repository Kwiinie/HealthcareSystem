using BusinessObjects.Commons;
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
        
        // New methods for document uploads
        Task<Result<string>> UploadAsync(IFormFile file, string folder);
        Task<Result<string>> UploadDocumentAsync(IFormFile file, string folder);
        Task<bool> ValidateDocumentFile(IFormFile file, int maxSizeMB = 10);
    }
}
