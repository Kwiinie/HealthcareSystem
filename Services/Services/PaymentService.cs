using AutoMapper;
using BusinessObjects.DTOs.Payment;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Services.Helpers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PaymentService : IPaymentService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly VNPayHelper _vnPayHelper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, VNPayHelper vnPayHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _vnPayHelper = vnPayHelper;
        }

        public async Task<string> CreatePaymentAsync(PaymentRequestDto requestDto, HttpContext httpContext)
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(requestDto.AppointmentId);
            if (appointment == null)
            {
                throw new Exception("Không tìm thấy lịch hẹn");
            }

            var transactionId = DateTime.Now.Ticks.ToString();
            requestDto.Amount = requestDto.Amount;
            var paymentUrl = _vnPayHelper.CreatePaymentUrl(requestDto, httpContext);

            var payment = new Payment
            {
                Price = (decimal)requestDto.Amount,
                PaymentStatus = PaymentStatus.Pending,
                PaymentMethod = "VnPay",
                PaymentDate = DateTime.UtcNow,
                TransactionId = transactionId
            };

            payment.Appointments.Add(appointment);
            payment.PaymentUrl = paymentUrl;

            _unitOfWork.GetRepository<Payment>().AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            appointment.PaymentId = payment.Id;
            appointment.Status = AppointmentStatus.AwaitingPayment;
            _unitOfWork.AppointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return paymentUrl;
        }

        public async Task<PaymentResponseDto> ExecutePaymentAsync(IQueryCollection collections)
        {
            var response = _vnPayHelper.PaymentExecute(collections);
            var orderInfo = response.OrderDescription;

            var appointmentId = ExtractAppointmentIdFromOrderInfo(orderInfo);

            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);
            if (appointment == null || appointment.PaymentId == null)
            {
                throw new Exception("Không tìm thấy thanh toán hoặc lịch hẹn.");
            }

            var payment = await _unitOfWork.GetRepository<Payment>().GetByIdAsync(appointment.PaymentId.Value);
            if (payment == null)
            {
                throw new Exception("Không tìm thấy thông tin thanh toán.");
            }

            payment.TransactionId = response.TransactionId;
            payment.PaymentStatus = response.ResponseCode == "00" ? PaymentStatus.Completed : PaymentStatus.Failed;
            payment.PaymentDate = collections["vnp_PayDate"] != StringValues.Empty
                ? DateTime.ParseExact(collections["vnp_PayDate"], "yyyyMMddHHmmss", CultureInfo.InvariantCulture)
                : (DateTime?)null;

            appointment.Status = response.ResponseCode == "00"
                ? AppointmentStatus.Pending
                : AppointmentStatus.Expired;

            _unitOfWork.AppointmentRepository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        private int ExtractAppointmentIdFromOrderInfo(string orderInfo)
        {
            var parts = orderInfo.Split(" ");
            if (int.TryParse(parts.Last(), out var appointmentId))
            {
                return appointmentId;
            }

            throw new Exception("Không thể lấy ID lịch hẹn từ mô tả giao dịch.");
        }

        public async Task<List<PaymentDto>> GetPaymentsByPatientIdAsync(int userId)
        {
            var appointmentRepo =  _unitOfWork.GetRepository<Appointment>();
            var patientRepo = _unitOfWork.GetRepository<Patient>();

            var patient = await patientRepo.FindAsync(p => p.UserId == userId);

            var appointments = await appointmentRepo.FindAllAsync(
                predicate: a => a.PatientId == patient.Id && a.PaymentId != null,
                includeProperties: "Payment"

            );

            var payments = appointments
                .Where(a => a.Payment != null)
                .Select(a => a.Payment!)
                .DistinctBy(p => p.Id)
                .ToList();

            return _mapper.Map<List<PaymentDto>>(payments);
        }
        public async Task<List<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _unitOfWork.GetRepository<Payment>().FindAllAsync(
                predicate: p => true,
                includeProperties: "Appointments"
            );

            return _mapper.Map<List<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _unitOfWork.GetRepository<Payment>().FindAsync(
                predicate: p => true,
                includeProperties: "Appointments"
            );
            return _mapper.Map<PaymentDto>(payment);
        }
    }
}
