using BusinessObjects.DTOs.Article;
using BusinessObjects.DTOs.Category;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Services;
using System.Security.Claims;
using BusinessObjects.Dtos.User;

namespace FindingHealthcareSystem.Pages.Admin.Article
{
    public class CreateModel : PageModel
    {
        private readonly IArticleService _articleService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public ArticleDTO Article { get; set; } = new ArticleDTO();

        [BindProperty]
        public IFormFile MainImageFile { get; set; }

        [BindProperty]
        public List<IFormFile> GalleryImageFiles { get; set; } = new List<IFormFile>();

        // Properties for image operations
        [BindProperty]
        public string ImageAction { get; set; }

        [BindProperty]
        public string ImageUrl { get; set; }

        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();

        [TempData]
        public string StatusMessage { get; set; }

        public CreateModel(
            IArticleService articleService,
            IFileUploadService fileUploadService,
            ICategoryService categoryService,
            IWebHostEnvironment environment)
        {
            _articleService = articleService;
            _fileUploadService = fileUploadService;
            _categoryService = categoryService;
            _environment = environment;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Load categories for dropdown
            Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Set the created date to now
                Article.CreatedAt = DateTime.Now;

                // Assuming this is created by the first admin - you would typically get the current user ID here
                Article.CreatedById = 1;

                // Initialize the ImgUrls collection if needed
                if (Article.ImgUrls == null)
                {
                    Article.ImgUrls = new List<string>();
                }

                // Handle main image upload if provided
                if (MainImageFile != null && MainImageFile.Length > 0)
                {
                    // Check if the file is valid
                    if (await _fileUploadService.ValidateImageFile(MainImageFile))
                    {
                        string mainImageUrl = await _fileUploadService.UploadImageAsync(MainImageFile, "articles");
                        Article.ImgUrl = mainImageUrl;
                    }
                    else
                    {
                        ModelState.AddModelError("Ảnh chính", "Vui lòng up ảnh đúng định dạng (JPG, JPEG, PNG) dưới 5MB.");
                        Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                        return Page();
                    }
                }

                // Handle gallery image uploads if any
                if (GalleryImageFiles != null && GalleryImageFiles.Any(f => f.Length > 0))
                {
                    // Validate and upload each gallery image
                    foreach (var file in GalleryImageFiles.Where(f => f.Length > 0))
                    {
                        if (await _fileUploadService.ValidateImageFile(file))
                        {
                            string imageUrl = await _fileUploadService.UploadImageAsync(file, "articles");
                            Article.ImgUrls.Add(imageUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("Ảnh thư viện", "Một trong số ảnh bạn up không đúng định dạng. Vui lòng up ảnh đúng định dạng (JPG, JPEG, PNG) dưới 5MB.");
                            Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                            return Page();
                        }
                    }
                }

                // Set the article as not deleted (published) by default
                Article.IsDeleted = false;

                // Add the article to the database
                await _articleService.AddArticleAsync(Article);

                // Set status message in TempData to show on Index page
                TempData["StatusMessage"] = "Bài viết đã được tạo thành công!";
                return RedirectToPage("/Admin/Article/Index");
            }
            catch (Exception ex)
            {
                // Log the error
                StatusMessage = $"Lỗi tạo bài viết: {ex.Message}";
                Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                return Page();
            }
        }
    }
}
