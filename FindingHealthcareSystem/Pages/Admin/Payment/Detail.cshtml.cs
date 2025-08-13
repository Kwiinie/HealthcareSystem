using BusinessObjects.DTOs.Appointment;
using BusinessObjects.DTOs.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace FindingHealthcareSystem.Pages.Admin.Payment
{
    public class DetailModel : PageModel
    {
        private readonly IPaymentService _paymentService;
        private readonly IAppointmentService _appointmentService;

        public DetailModel(IPaymentService paymentService, IAppointmentService appointmentService)
        {
            _paymentService = paymentService;
            _appointmentService = appointmentService;
        }

        public PaymentDto Payment { get; set; }
        public AppointmentDTO Appointment { get; set; }
        [BindProperty]
        public int PaymentId { get; set; }


        //public async Task<IActionResult> OnGetAsync(int id)
        //{
        //    Payment = await _paymentService.GetPaymentByIdAsync(id);

        //    if (Payment.AppointmentId.HasValue)
        //    {
        //        Appointment = await _appointmentService.GetAsync(Payment.AppointmentId.Value);
        //    }

        //    return Page();
        //}
    }
}
