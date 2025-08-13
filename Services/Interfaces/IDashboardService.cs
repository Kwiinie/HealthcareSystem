using BusinessObjects.DTOs.AdminDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDashboardService
    {
        Task<int> CountFacilitiesAsync();
        Task<int> CountProfessionalsAsync();
        Task<int> CountPatientsAsync();
        Task<int> CountPaymentsAsync();
        Task<List<AppointmentStatusDistributionDto>> GetAppointmentStatusDistributionAsync();
        Task<List<MonthlyPaymentDto>> GetMonthlyPaymentStatsAsync();
        Task<List<MonthlyAppointmentDto>> GetMonthlyAppointmentStatsAsync();
        Task<List<PaymentByProviderTypeDto>> GetPaymentByProviderTypeAsync();
        Task<List<ProvinceDistributionDto>> GetHealthcareDistributionByProvinceAsync();
    }
}
