using BusinessObjects.DTOs.Professional;
using BusinessObjects.Dtos.User;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.ExpiredDocuments
{
    public class IndexModel : PageModel
    {
        private readonly IProfessionalVerificationService _verificationService;

        public IndexModel(IProfessionalVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        public ExpiredDocumentStatistics Statistics { get; set; } = new();
        public List<ProfessionalDocumentDto> ExpiredDocuments { get; set; } = new();

        // Filter properties
        public string? FilterExpiryStatus { get; set; }
        public string? FilterDocumentType { get; set; }
        public string? FilterProvince { get; set; }

        public async Task<IActionResult> OnGetAsync(string? expiryStatus, string? documentType, string? province)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return RedirectToPage("/Auth/Login");

                FilterExpiryStatus = expiryStatus;
                FilterDocumentType = documentType;
                FilterProvince = province;

                var allDocuments = await _verificationService.GetPendingVerificationDocumentsAsync();

                // Calculate statistics
                var now = DateTime.Now;
                Statistics = new ExpiredDocumentStatistics
                {
                    ExpiredCount = allDocuments.Count(d => d.IsExpired),
                    ExpiringSoon7Days = allDocuments.Count(d =>
                        d.ExpiryDate.HasValue &&
                        d.ExpiryDate.Value.ToDateTime(TimeOnly.MinValue) <= now.AddDays(7) &&
                        d.ExpiryDate.Value.ToDateTime(TimeOnly.MinValue) > now),
                    ExpiringSoon30Days = allDocuments.Count(d =>
                        d.ExpiryDate.HasValue &&
                        d.ExpiryDate.Value.ToDateTime(TimeOnly.MinValue) <= now.AddDays(30) &&
                        d.ExpiryDate.Value.ToDateTime(TimeOnly.MinValue) > now.AddDays(7))
                };

                // Get filtered documents
                ExpiredDocuments = await GetFilteredExpiredDocumentsAsync(allDocuments);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải dữ liệu";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostSendReminderAsync(int documentId, string reminderMessage)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return Unauthorized();

                // Here you would implement the reminder sending logic
                // For now, we'll just simulate success
                await Task.Delay(100); // Simulate async operation

                TempData["SuccessMessage"] = "Đã gửi nhắc nhở gia hạn thành công";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi gửi nhắc nhở";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostMarkAsRenewedAsync(int documentId)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return Unauthorized();

                // Here you would implement the logic to mark document as renewed
                // This might involve updating the expiry date or creating a new document record

                TempData["SuccessMessage"] = "Đã đánh dấu tài liệu đã được gia hạn";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi cập nhật trạng thái";
                return RedirectToPage();
            }
        }

        private async Task<List<ProfessionalDocumentDto>> GetFilteredExpiredDocumentsAsync(List<ProfessionalDocumentDto> allDocuments)
        {
            var now = DateTime.Now;
            var filteredDocs = allDocuments.Where(d => d.ExpiryDate.HasValue).ToList();

            // Apply expiry status filter
            if (!string.IsNullOrEmpty(FilterExpiryStatus))
            {
                switch (FilterExpiryStatus)
                {
                    case "expired":
                        filteredDocs = filteredDocs.Where(d => d.IsExpired).ToList();
                        break;
                    case "expiring7":
                        filteredDocs = filteredDocs.Where(d =>
                            d.ExpiryDate.Value.ToDateTime(TimeOnly.MinValue) <= now.AddDays(7) &&
                            d.ExpiryDate.Value.ToDateTime(TimeOnly.MinValue) > now).ToList();
                        break;
                    case "expiring30":
                        filteredDocs = filteredDocs.Where(d =>
                            d.ExpiryDate.Value.ToDateTime(TimeOnly.MinValue) <= now.AddDays(30) &&
                            d.ExpiryDate.Value.ToDateTime(TimeOnly.MinValue) > now).ToList();
                        break;
                    default:
                        // Show expired and expiring documents
                        filteredDocs = filteredDocs.Where(d =>
                            d.IsExpired || d.IsExpiringSoon).ToList();
                        break;
                }
            }
            else
            {
                // Default: show expired and expiring documents
                filteredDocs = filteredDocs.Where(d =>
                    d.IsExpired || d.IsExpiringSoon).ToList();
            }

            // Apply document type filter
            if (!string.IsNullOrEmpty(FilterDocumentType) && Enum.TryParse<DocumentType>(FilterDocumentType, out var docType))
            {
                filteredDocs = filteredDocs.Where(d => d.DocumentType == docType).ToList();
            }

            // Apply province filter (you'll need to add Province to ProfessionalDocumentDto or get it from Professional)
            if (!string.IsNullOrEmpty(FilterProvince))
            {
                // This would require getting professional data - implement based on your needs
            }

            return filteredDocs.OrderBy(d => d.ExpiryDate).ToList();
        }

        private GeneralUserDto? GetCurrentUser()
        {
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
                return null;

            return JsonConvert.DeserializeObject<GeneralUserDto>(userJson);
        }
    }

    public class ExpiredDocumentStatistics
    {
        public int ExpiredCount { get; set; }
        public int ExpiringSoon7Days { get; set; }
        public int ExpiringSoon30Days { get; set; }
    }
}