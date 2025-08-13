using BusinessObjects.DTOs.Payment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentAsync(PaymentRequestDto requestDto, HttpContext httpContext);
        Task<PaymentResponseDto> ExecutePaymentAsync(IQueryCollection collections);
        Task<List<PaymentDto>> GetPaymentsByPatientIdAsync(int userId);
        Task<List<PaymentDto>> GetAllPaymentsAsync();
        Task<PaymentDto> GetPaymentByIdAsync(int paymentId);
    }
}
