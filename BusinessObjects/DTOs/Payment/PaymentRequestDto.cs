using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Payment
{
    public class PaymentRequestDto
    {
        public int AppointmentId { get; set; }
        public float Amount { get; set; }
    }
}
