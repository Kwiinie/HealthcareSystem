using BusinessObjects.DTOs.Article;
using BusinessObjects.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Article
{
    public class EditModel : PageModel
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

        public EditModel(
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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Article = await _articleService.GetArticleByIdAsync(id);
            if (Article == null)
            {
                return NotFound();
            }

            // Load categories for dropdown
            Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // First check if this is an image operation
            if (!string.IsNullOrEmpty(ImageAction) && !string.IsNullOrEmpty(ImageUrl))
            {
                return await HandleImageOperation();
            }

            try
            {
                // Get existing article to preserve properties that shouldn't be overwritten
                var existingArticle = await _articleService.GetArticleByIdAsync(Article.Id);
                if (existingArticle == null)
                {
                    return NotFound();
                }

                // Update form-editable properties
                existingArticle.Title = Article.Title;
                existingArticle.CategoryId = Article.CategoryId;
                existingArticle.Content = Article.Content;

                // Handle main image upload if provided
                if (MainImageFile != null && MainImageFile.Length > 0)
                {
                    // Check if the file is valid
                    if (await _fileUploadService.ValidateImageFile(MainImageFile))
                    {
                        string mainImageUrl = await _fileUploadService.UploadImageAsync(MainImageFile, "articles");
                        existingArticle.ImgUrl = mainImageUrl;
                    }
                    else
                    {
                        ModelState.AddModelError("Ảnh chính", "Vui lòng up ảnh đúng định dạng (JPG, JPEG, PNG) dưới 5MB.");
                        Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                        Article = existingArticle;
                        return Page();
                    }
                }

                // Handle gallery image uploads if any
                if (GalleryImageFiles != null && GalleryImageFiles.Any(f => f.Length > 0))
                {
                    // Initialize the collection if needed
                    if (existingArticle.ImgUrls == null)
                    {
                        existingArticle.ImgUrls = new List<string>();
                    }

                    // Validate and upload each gallery image
                    foreach (var file in GalleryImageFiles.Where(f => f.Length > 0))
                    {
                        if (await _fileUploadService.ValidateImageFile(file))
                        {
                            string imageUrl = await _fileUploadService.UploadImageAsync(file, "articles");
                            existingArticle.ImgUrls.Add(imageUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("Ảnh thư viện", "Một trong số ảnh bạn up không đúng định dạng. Vui lòng up ảnh đúng định dạng (JPG, JPEG, PNG) dưới 5MB.");
                            Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                            Article = existingArticle;
                            return Page();
                        }
                    }
                }

                // Update the article in the database
                bool updateSuccess = await _articleService.UpdateArticleAsync(existingArticle);

                if (updateSuccess)
                {
                    // Set status message in TempData to show on Index page
                    TempData["StatusMessage"] = "Bài viết cập nhật thành công!";
                    return RedirectToPage("/Admin/Article/Index");
                }
                else
                {
                    StatusMessage = "Lỗi: Không thể cập nhật bài viết.";
                    Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                    Article = existingArticle; // Return the modified article
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Log the error
                StatusMessage = $"Lỗi cập nhật: {ex.Message}";
                Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                return Page();
            }
        }

        private async Task<IActionResult> HandleImageOperation()
        {
            try
            {
                // Get the current article
                var article = await _articleService.GetArticleByIdAsync(Article.Id);
                if (article == null)
                {
                    return NotFound();
                }

                if (ImageAction == "delete")
                {
                    // Check if it's the main image
                    bool isMainImage = article.ImgUrl == ImageUrl;

                    // Delete the image from file system
                    await _fileUploadService.DeleteImageAsync(ImageUrl);

                    // Update the article DTO
                    if (isMainImage)
                    {
                        article.ImgUrl = null;
                    }
                    else if (article.ImgUrls != null)
                    {
                        article.ImgUrls.Remove(ImageUrl);
                    }

                    StatusMessage = "Xóa ảnh thành công";
                }
                else if (ImageAction == "setMain")
                {
                    // Get previous main image (if any)
                    string previousMainImage = article.ImgUrl;

                    // Set new main image
                    article.ImgUrl = ImageUrl;

                    // Move previous main image to gallery if it exists
                    if (!string.IsNullOrEmpty(previousMainImage) && previousMainImage != ImageUrl)
                    {
                        if (article.ImgUrls == null)
                        {
                            article.ImgUrls = new List<string>();
                        }

                        if (!article.ImgUrls.Contains(previousMainImage))
                        {
                            article.ImgUrls.Add(previousMainImage);
                        }
                    }

                    // Remove new main image from gallery
                    if (article.ImgUrls != null)
                    {
                        article.ImgUrls.Remove(ImageUrl);
                    }

                    StatusMessage = "Set ảnh chính thành công.";
                }

                // Update the article in the database
                await _articleService.UpdateArticleAsync(article);

                // Return to same page with updated article
                return RedirectToPage(new { id = article.Id });
            }
            catch (Exception ex)
            {
                StatusMessage = $"Lỗi ảnh: {ex.Message}";
                Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
                return Page();
            }
        }
    }
}