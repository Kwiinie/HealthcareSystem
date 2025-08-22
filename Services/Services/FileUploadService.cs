using BusinessObjects.Commons;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly Cloudinary _cloudinary;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly string[] _allowedDocumentExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };

        public FileUploadService(
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            
            // Initialize Cloudinary
            var cloudinaryUrl = configuration.GetConnectionString("CloudinarySettings") ??
                              "cloudinary://617478869489255:anr6kzuQuHhCx8bScRMtuvsMSuY@djsifikxn";
            _cloudinary = new Cloudinary(cloudinaryUrl);
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

        public async Task<Result<string>> UploadAsync(IFormFile file, string folder)
        {
            return await UploadDocumentAsync(file, folder);
        }

        public async Task<Result<string>> UploadDocumentAsync(IFormFile file, string folder)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return Result<string>.Failure("Không có tệp nào được chọn");

                if (!await ValidateDocumentFile(file))
                    return Result<string>.Failure("Tệp không hợp lệ. Vui lòng tải lên tệp PDF, JPG, JPEG hoặc PNG dưới 10MB");

                // Generate unique public ID
                var publicId = $"{folder}/{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(file.FileName)}";
                
                using var stream = file.OpenReadStream();
                
                // Check if the file is an image to use appropriate upload parameters
                if (IsImageFile(file))
                {
                    // Use ImageUploadParams for image files to enable Cloudinary optimizations
                    var imageUploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        PublicId = publicId,
                        Folder = folder,
                        UseFilename = true,
                        UniqueFilename = true,
                        Overwrite = false,
                        // Add image optimization for certificates
                        Transformation = new Transformation()
                            .Quality("auto:good") // Automatic quality optimization
                            .FetchFormat("auto") // Automatic format selection (WebP when supported)
                            .Flags("progressive") // Progressive JPEG loading
                    };

                    var uploadResult = await _cloudinary.UploadAsync(imageUploadParams);

                    if (uploadResult.Error != null)
                        return Result<string>.Failure($"Lỗi upload: {uploadResult.Error.Message}");

                    return Result<string>.Success(uploadResult.SecureUrl.ToString());
                }
                else
                {
                    // Use RawUploadParams for PDF files
                    var rawUploadParams = new RawUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        PublicId = publicId,
                        Folder = folder,
                        UseFilename = true,
                        UniqueFilename = true,
                        Overwrite = false
                    };

                    var uploadResult = await _cloudinary.UploadAsync(rawUploadParams);

                    if (uploadResult.Error != null)
                        return Result<string>.Failure($"Lỗi upload: {uploadResult.Error.Message}");

                    return Result<string>.Success(uploadResult.SecureUrl.ToString());
                }
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Lỗi khi tải lên tệp: {ex.Message}");
            }
        }

        public async Task<bool> ValidateDocumentFile(IFormFile file, int maxSizeMB = 10)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            // Check file size (default: 10MB max for documents)
            if (file.Length > maxSizeMB * 1024 * 1024)
            {
                return false;
            }

            // Check file extension
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !_allowedDocumentExtensions.Contains(extension))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if the uploaded file is an image type that should use Cloudinary's image optimization features
        /// </summary>
        /// <param name="file">The uploaded file to check</param>
        /// <returns>True if the file is an image (jpg, jpeg, png), False if it's a PDF or other document type</returns>
        private bool IsImageFile(IFormFile file)
        {
            if (file == null)
                return false;

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            
            return !string.IsNullOrEmpty(extension) && imageExtensions.Contains(extension);
        }
    }
    
}
