using BusinessObjects.DTOs.Article;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Article
{
    public class DetailModel : PageModel
    {
        private readonly IArticleService _articleService;

        public DetailModel(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public ArticleDTO Article { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            Article = await _articleService.GetArticleByIdAsync(id);
            if (Article == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
