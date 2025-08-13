using BusinessObjects.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Category
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchCreatedFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchCreatedTo { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchUpdatedFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? SearchUpdatedTo { get; set; }

        [BindProperty(SupportsGet = true)]
        public string StatusFilter { get; set; }

        public async Task OnGet()
        {
            // Get all categories first
            var allCategories = await _categoryService.GetAllCategoriesAsync();

            // Apply filters
            var filteredCategories = allCategories;

            // Filter by name
            if (!string.IsNullOrWhiteSpace(SearchName))
            {
                filteredCategories = filteredCategories.Where(c =>
                    c.Name != null && c.Name.Contains(SearchName, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by created date range
            if (SearchCreatedFrom.HasValue)
            {
                filteredCategories = filteredCategories.Where(c =>
                    c.CreatedAt?.Date >= SearchCreatedFrom.Value.Date);
            }

            if (SearchCreatedTo.HasValue)
            {
                filteredCategories = filteredCategories.Where(c =>
                    c.CreatedAt?.Date <= SearchCreatedTo.Value.Date);
            }

            // Filter by updated date range
            if (SearchUpdatedFrom.HasValue)
            {
                filteredCategories = filteredCategories.Where(c =>
                    c.UpdatedAt?.Date >= SearchUpdatedFrom.Value.Date);
            }

            if (SearchUpdatedTo.HasValue)
            {
                filteredCategories = filteredCategories.Where(c =>
                    c.UpdatedAt?.Date <= SearchUpdatedTo.Value.Date);
            }

            // Filter by status
            if (!string.IsNullOrEmpty(StatusFilter))
            {
                if (StatusFilter == "active")
                {
                    filteredCategories = filteredCategories.Where(c => !(bool)c.IsDeleted);
                }
                else if (StatusFilter == "deleted")
                {
                    filteredCategories = filteredCategories.Where(c => (bool)c.IsDeleted);
                }
                // If "all" is selected, no filtering needed
            }

            Categories = filteredCategories;
        }

        public async Task OnPostResetAsync()
        {
            // Reset search filters and redirect to the page
            RedirectToPage();
        }
    }
}
