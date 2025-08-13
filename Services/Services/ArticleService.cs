using AutoMapper;
using BusinessObjects.DTOs.Article;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddArticleAsync(ArticleDTO articleDto)
        {
            if (articleDto == null) throw new ArgumentNullException(nameof(articleDto));

            // Map DTO to entity (this will handle the basic properties but not ImgUrls)
            var article = _mapper.Map<Article>(articleDto);

            // Add article to repository
            await _unitOfWork.ArticleRepository.AddAsync(article);

            // Save to get the article ID
            await _unitOfWork.SaveChangesAsync();

            // Now handle the gallery images
            if (articleDto.ImgUrls != null && articleDto.ImgUrls.Any())
            {
                foreach (var imgUrl in articleDto.ImgUrls)
                {
                    // Create and add an ArticleImage for each URL in the collection
                    var articleImage = new ArticleImage
                    {
                        ArticleId = article.Id,
                        ImgUrl = imgUrl
                    };

                    await _unitOfWork.ArticleImageRepository.AddAsync(articleImage);
                }

                // Save again to persist the article images
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _unitOfWork.ArticleRepository.GetByIdAsync(id);
            if (article != null)
            {
                article.IsDeleted = true;
                _unitOfWork.ArticleRepository.Update(article);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ArticleDTO>> GetAllArticlesAsync()
        {
            var result = await _unitOfWork.ArticleRepository.GetAllAsync();
            if(result != null)
            {
                return _mapper.Map<IEnumerable<ArticleDTO>>(result);
            }
            else
            {
                return new List<ArticleDTO>();
            }
        }

        public async Task<ArticleDTO> GetArticleByIdAsync(int id)
        {
            var details = await _unitOfWork.ArticleRepository.FindAsync(a => a.Id == id, "ArticleImages,Category");

            return _mapper.Map<ArticleDTO>(details);
        }

        public async Task<bool> UpdateArticleAsync(ArticleDTO articleDto)
        {
            var article = await _unitOfWork.ArticleRepository.GetByIdAsync(articleDto.Id);
            if (article == null)
            {
                throw new KeyNotFoundException($"Article with ID {articleDto.Id} not found");
            }

            // Update article properties
            article.Title = articleDto.Title;
            article.Content = articleDto.Content;
            article.CategoryId = articleDto.CategoryId;
            article.IsDeleted = articleDto.IsDeleted;
            article.ImgUrl = articleDto.ImgUrl;


            // Only update ImgUrl if it's provided in the DTO
            if (!string.IsNullOrEmpty(articleDto.ImgUrl))
            {
                article.ImgUrl = articleDto.ImgUrl;
            }

            // Update the article in the repository
            _unitOfWork.ArticleRepository.Update(article);

            // Handle article images if provided in the DTO
            if (articleDto.ImgUrls != null && articleDto.ImgUrls.Any())
            {
                var artImgRepo = _unitOfWork.GetRepository<ArticleImage>();
                // Get current article images
                var existingImages = await artImgRepo
                    .FindAllAsync(ai => ai.ArticleId == article.Id);

                // Identify new images to add
                var existingUrls = existingImages.Select(img => img.ImgUrl).ToList();
                var newUrls = articleDto.ImgUrls.Except(existingUrls).ToList();

                // Add new images
                foreach (var newUrl in newUrls)
                {
                    await _unitOfWork.ArticleImageRepository.AddAsync(new ArticleImage
                    {
                        ArticleId = article.Id,
                        ImgUrl = newUrl
                    });
                }

                // Identify images to remove (images that exist in the database but not in the DTO)
                var urlsToRemove = existingUrls.Except(articleDto.ImgUrls).ToList();
                foreach (var urlToRemove in urlsToRemove)
                {
                    var imageToRemove = existingImages.FirstOrDefault(img => img.ImgUrl == urlToRemove);
                    if (imageToRemove != null)
                    {
                        _unitOfWork.ArticleImageRepository.Remove(imageToRemove);
                    }
                }
            }

            // Save all changes
            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }
    }
}
