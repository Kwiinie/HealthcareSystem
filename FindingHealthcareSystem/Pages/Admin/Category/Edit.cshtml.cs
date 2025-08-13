using BusinessObjects.DTOs.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Category
{ 
    public class EditModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public CategoryDTO Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Category = await _categoryService.GetCategoryByIdAsync(id);

            if (Category == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thể loại!";
                return RedirectToPage("Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //var existingCategory = await _categoryService.GetCategoryByIdAsync(Category.Id.Value);
            //if (existingCategory == null)
            //{
            //    TempData["ErrorMessage"] = "Không tìm thấy thể loại!";
            //    return RedirectToPage("Index");
            //}

            //existingCategory.Name = Category.Name;
            //existingCategory.Description = Category.Description;
            //existingCategory.IsDeleted = Category.IsDeleted;
            //existingCategory.UpdatedAt = DateTime.UtcNow;

            //await _categoryService.UpdateCategoryAsync(existingCategory);

            //TempData["SuccessMessage"] = "Cập nhật thể loại thành công!";
            //return RedirectToPage("Index");
            Category.UpdatedAt = DateTime.UtcNow;

            // Just update the Category object that was bound from the form
            await _categoryService.UpdateCategoryAsync(Category);

            TempData["SuccessMessage"] = "Cập nhật thể loại thành công!";
            return RedirectToPage("Index");
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["SuccessMessage"] = "Xóa thể loại thành công!";
            return RedirectToPage("Index");
        }
    }
}
