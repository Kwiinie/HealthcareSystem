using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Payment
{
    public class PaymentResponseDto
    {
        public string? OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public string? TransactionOrderId { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentId { get; set; }
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? ResponseCode { get; set; }
        public string? ReturnMsg { get; set; }
        public string? CardType { get; set; }
    }
}
