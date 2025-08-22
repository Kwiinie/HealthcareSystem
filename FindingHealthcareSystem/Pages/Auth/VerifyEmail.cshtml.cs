using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Auth
{
    public class VerifyEmailModel : PageModel
    {
        private readonly IAuthService _authService;

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public VerifyEmailModel(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                IsSuccess = false;
                ErrorMessage = "Liên kết xác nhận không hợp lệ. Vui lòng kiểm tra lại email của bạn.";
                return Page();
            }

            var result = await _authService.VerifyEmailAsync(token, email);
            
            if (result.Success)
            {
                IsSuccess = true;
                TempData["SuccessMessage"] = "Email đã được xác nhận thành công! Bạn có thể đăng nhập ngay bây giờ.";
            }
            else
            {
                IsSuccess = false;
                ErrorMessage = result.ErrorMessage ?? "Đã xảy ra lỗi khi xác nhận email.";
            }

            return Page();
        }
    }
}
