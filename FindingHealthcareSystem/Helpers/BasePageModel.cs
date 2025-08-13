using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace FindingHealthcareSystem.Helpers
{
    public class BasePageModel : PageModel
    {
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

    }
}
