using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FindingHealthcareSystem.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
