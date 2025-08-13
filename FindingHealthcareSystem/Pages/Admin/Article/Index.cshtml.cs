using BusinessObjects.DTOs.Article;
using BusinessObjects.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Article
{
    public class IndexModel : PageModel
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public IndexModel(IArticleService articleService, ICategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        public IEnumerable<ArticleDTO>? Articles { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
        public ArticleDTO Article { get; set; } = new();
        public const int PageSize = 10; // Number of articles per page
        public int CurrentPage { get; set; } = 1; // Current page
        public int TotalPages { get; set; }

        // To store filter values for maintaining state between requests
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? CreatedDate { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        public async Task OnGetAsync(int page = 1)
        {
            // Load all categories for the dropdown
            Categories = await _categoryService.GetAllCategoriesAsync();

            // Get all articles
            var allArticles = await _articleService.GetAllArticlesAsync();

            // Set current page
            CurrentPage = page < 1 ? 1 : page;

            // Calculate total pages
            TotalPages = (int)Math.Ceiling(allArticles.Count() / (double)PageSize);

            // Apply pagination
            Articles = allArticles
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnPostSearchAsync(string? searchTerm, int? categoryId, DateTime? createdDate, int page = 1)
        {
            // Load all categories for the dropdown
            Categories = await _categoryService.GetAllCategoriesAsync();

            // Save filter parameters
            SearchTerm = searchTerm;
            CategoryId = categoryId;
            CreatedDate = createdDate;

            // Get all articles first
            var allArticles = await _articleService.GetAllArticlesAsync();
            var filteredArticles = allArticles.AsQueryable();

            // Filter by title (if provided)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredArticles = filteredArticles.Where(a => a.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by category (if provided)
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                filteredArticles = filteredArticles.Where(a => a.CategoryId == categoryId.Value);
            }

            // Filter by creation date (if provided)
            if (createdDate.HasValue)
            {
                filteredArticles = filteredArticles.Where(a => a.CreatedAt.Date == createdDate.Value.Date);
            }

            // Calculate total pages
            TotalPages = (int)Math.Ceiling(filteredArticles.Count() / (double)PageSize);

            // Set current page
            CurrentPage = page < 1 ? 1 : page;
            CurrentPage = CurrentPage > TotalPages ? TotalPages : CurrentPage;
            CurrentPage = TotalPages == 0 ? 1 : CurrentPage;

            // Apply pagination
            Articles = filteredArticles
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int articleId, bool currentStatus)
        {
            try
            {
                var article = await _articleService.GetArticleByIdAsync(articleId);
                currentStatus = article.IsDeleted;
                if (article == null)
                {
                    StatusMessage = $"Error: Article with ID {articleId} not found.";
                    return RedirectToPage();
                }

                if (currentStatus == true)
                {
                    article.IsDeleted = false;
                }
                else
                {
                    article.IsDeleted = true;

                }

                bool updateSuccess = await _articleService.UpdateArticleAsync(article);

                if (updateSuccess)
                {
                    StatusMessage = article.IsDeleted
                        ? $"Bài viết '{article.Title}' đã dừng xuất bản."
                        : $"Bài viết '{article.Title}' đã xuất bản.";
                }
                else
                {
                    StatusMessage = $"Lỗi: Không thể thay đổi trạng thái bài viết.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
