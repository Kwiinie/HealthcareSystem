using BusinessObjects.DTOs.AdminDashboard;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountFacilitiesAsync()
        {
            return await _unitOfWork.GetRepository<Facility>().CountAsync();
        }

        public async Task<int> CountProfessionalsAsync()
        {
            return await _unitOfWork.GetRepository<Professional>().CountAsync();
        }

        public async Task<int> CountPatientsAsync()
        {
            return await _unitOfWork.GetRepository<Patient>().CountAsync();
        }

        public async Task<int> CountPaymentsAsync()
        {
            return await _unitOfWork.GetRepository<Payment>().CountAsync();
        }
        public async Task<List<AppointmentStatusDistributionDto>> GetAppointmentStatusDistributionAsync()
        {
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();

            var result = appointments
                .GroupBy(a => a.Status)
                .Select(g => new AppointmentStatusDistributionDto
                {
                    Label = g.Key switch
                    {
                        AppointmentStatus.AwaitingPayment => "Chờ thanh toán",
                        AppointmentStatus.Pending => "Chờ xác nhận",
                        AppointmentStatus.Confirmed => "Đã xác nhận",
                        AppointmentStatus.Completed => "Hoàn thành",
                        AppointmentStatus.Rescheduled => "Dời lịch",
                        AppointmentStatus.Cancelled => "Đã hủy",
                        AppointmentStatus.Rejected => "Từ chối",
                        AppointmentStatus.Expired => "Thanh toán thất bại",
                        _ => g.Key.ToString()
                    },
                    Count = g.Count()

                })
                .ToList();

            return result;
        }

        public async Task<List<MonthlyPaymentDto>> GetMonthlyPaymentStatsAsync()
        {
            var payments = await _unitOfWork.GetRepository<Payment>().GetAllAsync();

            var result = payments
                .Where(p => p.PaymentStatus == PaymentStatus.Completed && p.PaymentDate != null)
                .GroupBy(p => new { p.PaymentDate.Value.Year, p.PaymentDate.Value.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new MonthlyPaymentDto
                {
                    Month = g.Key.Month,
                    Total = g.Sum(p => p.Price.Value) 
                })
                .ToList();

            return result;
        }

        public async Task<List<MonthlyAppointmentDto>> GetMonthlyAppointmentStatsAsync()
        {
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();

            var result = appointments
                .Where(a => a.Date.HasValue &&
                      (a.Status == AppointmentStatus.Confirmed || a.Status == AppointmentStatus.Completed || a.Status == AppointmentStatus.Rescheduled))
                .GroupBy(a => a.Date.Value.Month)
                .OrderBy(g => g.Key)
                .Select(g => new MonthlyAppointmentDto
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToList();

            return result;
        }

        public async Task<List<PaymentByProviderTypeDto>> GetPaymentByProviderTypeAsync()
        {
            var appointments = await _unitOfWork.AppointmentRepository
        .FindAllAsync(a => a.Payment != null &&
                           a.Payment.PaymentStatus == PaymentStatus.Completed);

            var result = appointments
                .GroupBy(a => a.ProviderType)
                .Select(g => new PaymentByProviderTypeDto
                {
                    Label = g.Key switch
                    {
                        ProviderType.Facility => "Cơ sở y tế",
                        ProviderType.Professional => "Chuyên gia y tế",
                        _ => "Khác"
                    },
                    Total = g.Sum(a => a.Payment.Price.Value)
                })
                .ToList();

            return result;
        }

        public async Task<List<ProvinceDistributionDto>> GetHealthcareDistributionByProvinceAsync()
        {
            var facilities = await _unitOfWork.GetRepository<Facility>().GetAllAsync();
            var professionals = await _unitOfWork.GetRepository<Professional>().GetAllAsync();

            // Normalize: remove "Tỉnh"/"Thành phố", trim + ToLowerInvariant
            string Normalize(string provinceName)
            {
                return provinceName
                    .Replace("Tỉnh", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("Thành phố", "", StringComparison.OrdinalIgnoreCase)
                    .Trim()
                    .ToLowerInvariant();
            }

            var facilityGroup = facilities
                .GroupBy(f => Normalize(f.Province))
                .ToDictionary(g => g.Key, g => g.Count());

            var professionalGroup = professionals
                .GroupBy(p => Normalize(p.Province))
                .ToDictionary(g => g.Key, g => g.Count());

            var allProvinces = facilityGroup.Keys
                .Union(professionalGroup.Keys)
                .Distinct();

            var result = allProvinces.Select(provinceKey => new ProvinceDistributionDto
            {
                Province = provinceKey,
                FacilityCount = facilityGroup.ContainsKey(provinceKey) ? facilityGroup[provinceKey] : 0,
                ProfessionalCount = professionalGroup.ContainsKey(provinceKey) ? professionalGroup[provinceKey] : 0
            }).ToList();

            return result;
        }


    }
}
