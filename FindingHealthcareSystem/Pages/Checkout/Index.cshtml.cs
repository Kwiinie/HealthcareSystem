using FindingHealthcareSystem.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Checkout
{
    public class IndexModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public IndexModel(IPaymentService paymentService, IHubContext<NotificationHub> hubContext)
        {
            _paymentService = paymentService;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _paymentService.ExecutePaymentAsync(Request.Query);

                if (response.ResponseCode == "00")
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
                    {
                        message = "1 bệnh nhân vừa đặt lịch và thanh toán thành công",
                        type = "success"
                    });

                    return Redirect("/checkout/success");
                }
                else
                {
                    return Redirect("/checkout/fail");
                }
            }
            catch (Exception)
            {
                return Redirect("/checkout/fail");
            }
        }
    }
}
