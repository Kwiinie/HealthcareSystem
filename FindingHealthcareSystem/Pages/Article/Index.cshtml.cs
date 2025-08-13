using BusinessObjects.DTOs.Article;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Article
{
    public class IndexModel : PageModel
    {
        private readonly IArticleService _articleService;

        public IndexModel(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public IEnumerable<ArticleDTO>? Articles { get; set; }
        public async Task OnGetAsync()
        {
            var allArticles = await _articleService.GetAllArticlesAsync();
            Articles = allArticles.Where(a => a.IsDeleted == false).ToList();


        }
    }
}
