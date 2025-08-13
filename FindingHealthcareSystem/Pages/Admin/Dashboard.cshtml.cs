using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using System.Text.Json;

namespace FindingHealthcareSystem.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IDashboardService _dashboardService;

        public int TotalFacilities { get; set; }
        public int TotalProfessionals { get; set; }
        public int TotalPatients { get; set; }
        public int TotalPayments { get; set; }
        public string AppointmentStatusChartJson { get; set; } = "[]";//PIE CHART
        public string MonthlyPaymentChartJson { get; set; } = "[]"; //BAR CHART
        public string MonthlyAppointmentChartJson { get; set; } = "[]"; //LINE CHART
        public string RevenueByProviderChartJson { get; set; } = "[]"; //DOUGHNUT CHART
        public string ProvinceDistributionJson { get; set; } //MAP





        public DashboardModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task OnGetAsync()
        {
            //STATS CARD
            TotalFacilities = await _dashboardService.CountFacilitiesAsync();
            TotalProfessionals = await _dashboardService.CountProfessionalsAsync();
            TotalPatients = await _dashboardService.CountPatientsAsync();
            TotalPayments = await _dashboardService.CountPaymentsAsync();

            //PIE CHART
            var data = await _dashboardService.GetAppointmentStatusDistributionAsync();

            AppointmentStatusChartJson = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            //BAR CHART
            var monthlyPayments = await _dashboardService.GetMonthlyPaymentStatsAsync();
            MonthlyPaymentChartJson = JsonSerializer.Serialize(monthlyPayments, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            //LINE CHART
            var monthlyAppointments = await _dashboardService.GetMonthlyAppointmentStatsAsync();
            MonthlyAppointmentChartJson = JsonSerializer.Serialize(monthlyAppointments, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            //DOUGHNUT CHART
            var byProviderType = await _dashboardService.GetPaymentByProviderTypeAsync();
            RevenueByProviderChartJson = JsonSerializer.Serialize(byProviderType, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var provinceDistribution = await _dashboardService.GetHealthcareDistributionByProvinceAsync();
            ProvinceDistributionJson = JsonSerializer.Serialize(provinceDistribution, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        }
    }
}
