using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace FindingHealthcareSystem.Pages.Auth
{
    public class ResendVerificationModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        public ResendVerificationModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _authService.ResendEmailVerificationAsync(Email);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Email xác nhận đã được gửi lại thành công! Vui lòng kiểm tra hộp thư của bạn.";
                    return RedirectToPage("/Auth/Login");
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage;
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return Page();
            }
        }
    }
}
