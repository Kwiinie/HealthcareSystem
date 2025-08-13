using BusinessObjects.DTOs.Auth;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        [BindProperty]
        public LoginDto loginDto { get; set; }

        public string ErrorMessage { get; set; }

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(loginDto);

                if (result.Success)
                {

                    var accountJson = JsonConvert.SerializeObject(result.Data);
                    HttpContext.Session.SetString("User", accountJson);

                    if(result.Data.Role == "Admin")
                    {
                        return Redirect("/admin/dashboard");
                    }

                    return Redirect("/");
                }
                else
                {
                    ErrorMessage = result.ErrorMessage;
                }
            }

            return Page();
        }


    }
}
