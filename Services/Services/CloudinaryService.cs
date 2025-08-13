using AutoMapper;
using BusinessObjects.DTOs.Article;
using BusinessObjects.Entities;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Services.Interfaces;
using Services.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        //Setup cloudinary 
        public CloudinaryService(IOptions<CloudinarySettings> config,IUnitOfWork unitOfWork, IMapper mapper)
        {
                 
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = new Cloudinary(acc);
        }

        //UploadImageAsync nhận file Stream và upload lên Cloudinary.

        public async Task<ImageUploadResult> UploadImageArticle(IFormFile file, int articleId)
        {
            try
            {
                // Kiểm tra Article có tồn tại không
                var articleEntity = await _unitOfWork.ArticleRepository.GetByIdAsync(articleId);
                if (articleEntity == null)
                {
                    throw new Exception("Article not found");
                }

                // Kiểm tra file hợp lệ
                if (file == null || file.Length == 0)
                {
                    throw new Exception("No file provided or file is empty");
                }

                // Validate casc loai file co the bo vo 
                var validImageTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!validImageTypes.Contains(file.ContentType))
                {
                    throw new Exception("Invalid file type. Only JPEG, PNG, and GIF are allowed.");
                }

                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception("Error uploading image: " + uploadResult.Error?.Message);
                }

                // Lưu ảnh vào bảng ArticleImages
                var articleImage = new ArticleImage
                {
                    ImgUrl = uploadResult.Url.ToString(),
                    ArticleId = articleEntity.Id
                };

                await _unitOfWork.ArticleImageRepository.AddAsync(articleImage);

                // Lưu thay đổi vào database
                var isSaved = await _unitOfWork.SaveChangesAsync() > 0;
                if (!isSaved)
                {
                    throw new Exception("Failed to save image to the database");
                }

                return new ImageUploadResult { Url = uploadResult.Url };
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần
                Console.WriteLine("Error: " + ex.Message);
                throw; // Ném lại exception để xử lý ở chỗ gọi hàm
            }
        }

    }
}
