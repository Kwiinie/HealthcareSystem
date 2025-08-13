using BusinessObjects.DTOs.Payment;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Payment
{
    public class IndexModel : PageModel
    {
        private readonly IPaymentService _paymentService;

        public IndexModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public List<PaymentDto> Payments { get; set; } = new List<PaymentDto>();

        [BindProperty(SupportsGet = true)]
        public PaymentStatus? StatusFilter { get; set; }

        public async Task OnGetAsync()
        {
            Payments = await _paymentService.GetAllPaymentsAsync();

            if (StatusFilter.HasValue)
            {
                Payments = Payments.Where(p => p.PaymentStatus == StatusFilter.Value).ToList();
            }
        }
    }
}
