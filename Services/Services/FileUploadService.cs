using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public FileUploadService(
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> UploadImageAsync(IFormFile file, string entityType)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            if (!await ValidateImageFile(file))
            {
                throw new ArgumentException("Invalid file. Please upload a valid image file (JPG, JPEG, PNG) under 5MB.");
            }

            // Normalize the entity type to ensure consistent folder naming
            entityType = string.IsNullOrEmpty(entityType) ? "misc" : entityType.ToLower();

            // Create directory if it doesn't exist
            string uploadsFolder = Path.Combine(_environment.WebRootPath, "images", entityType);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique filename
            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            string imageUrl = $"/images/{entityType}/{uniqueFileName}";

            return imageUrl;
        }

        public async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> files, string entityType)
        {
            List<string> uploadedUrls = new List<string>();

            if (files == null)
            {
                return uploadedUrls;
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string imageUrl = await UploadImageAsync(file, entityType);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        uploadedUrls.Add(imageUrl);
                    }
                }
            }

            return uploadedUrls;
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            try
            {
                // Extract the file path from the URL
                Uri uri = new Uri(imageUrl);
                string relativePath = uri.LocalPath;

                // Remove the leading slash if present
                if (relativePath.StartsWith("/"))
                {
                    relativePath = relativePath.Substring(1);
                }

                // Get the full file path
                string filePath = Path.Combine(_environment.WebRootPath, relativePath);

                // Check if file exists before deleting
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception)
            {
                // Log the error but don't throw, as this may be part of a larger operation
            }
        }

        public async Task<bool> ValidateImageFile(IFormFile file, int maxSizeMB = 5)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            // Check file size (default: 5MB max)
            if (file.Length > maxSizeMB * 1024 * 1024)
            {
                return false;
            }

            // Check file extension
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
            {
                return false;
            }


            return true;
        }
    }
    
}
