using BusinessObjects.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Category
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [BindProperty]
        public CategoryDTO Category { get; set; }
        public async Task OnGet()
        {
            //Category = (await _categoryService.GetAllCategories()).ToList();
        }
        public async Task OnPostAsync()
        {
            await _categoryService.AddCategoryAsync(Category);
            RedirectToPage("Index");
        }
    }
}
