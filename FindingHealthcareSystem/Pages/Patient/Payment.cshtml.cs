using BusinessObjects.DTOs.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Security.Claims;

namespace FindingHealthcareSystem.Pages.Patient
{
    public class PaymentModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService; // Add this if you need to verify user identity

        public PaymentModel(
            IPaymentService paymentService,
            IUserService userService = null) // Optional dependency
        {
            _paymentService = paymentService;
            _userService = userService;
        }

        public List<PaymentDto> PatientPayments { get; set; } = new List<PaymentDto>();

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? userId)
        {
            // First check if UserId is provided in the route
            if (userId.HasValue)
            {
                UserId = userId.Value;
            }

            // If no UserId specified, try to get the current logged-in user's ID
            if (UserId <= 0 && User.Identity.IsAuthenticated)
            {
                // This assumes you have ClaimTypes.NameIdentifier in your user claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int currentUserId))
                {
                    UserId = currentUserId;
                }
            }

            // Security check - only allow users to see their own payments unless they're admin
            // Remove or modify this if not needed for your application
            if (UserId > 0 && _userService != null)
            {
                bool isAdmin = User.IsInRole("Admin");
                bool isOwnAccount = User.Identity.IsAuthenticated &&
                                   User.FindFirst(ClaimTypes.NameIdentifier)?.Value == UserId.ToString();

                if (!isAdmin && !isOwnAccount)
                {
                    return Forbid(); // Return 403 if trying to access another user's payments
                }
            }

            // Get payments for the specified user
            if (UserId > 0)
            {
                PatientPayments = await _paymentService.GetPaymentsByPatientIdAsync(UserId);
            }

            return Page();
        }
    }
}
