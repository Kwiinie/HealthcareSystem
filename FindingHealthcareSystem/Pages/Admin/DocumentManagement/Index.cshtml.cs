using BusinessObjects.DTOs.Professional;
using BusinessObjects.Dtos.User;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.DocumentManagement
{
    public class IndexModel : PageModel
    {
        private readonly IProfessionalVerificationService _verificationService;

        public IndexModel(IProfessionalVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        public DocumentManagementStatistics Statistics { get; set; } = new();
        public List<ProfessionalDocumentDto> RecentDocuments { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return RedirectToPage("/Auth/Login");

                var allDocuments = await _verificationService.GetPendingVerificationDocumentsAsync();

                Statistics = new DocumentManagementStatistics
                {
                    TotalDocuments = allDocuments.Count,
                    PendingDocuments = allDocuments.Count(d => d.VerificationStatus == DocumentVerificationStatus.PendingVerification),
                    VerifiedDocuments = allDocuments.Count(d => d.VerificationStatus == DocumentVerificationStatus.Verified),
                    ExpiredDocuments = allDocuments.Count(d => d.IsExpired),
                    ExpiringSoonDocuments = allDocuments.Count(d => d.IsExpiringSoon && !d.IsExpired),
                    RejectedDocuments = allDocuments.Count(d => d.VerificationStatus == DocumentVerificationStatus.Rejected)
                };

                RecentDocuments = allDocuments
                    .OrderByDescending(d => d.CreatedAt)
                    .Take(20)
                    .ToList();

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải dữ liệu";
                return Page();
            }
        }

        public async Task<IActionResult> OnGetDocumentTypeStatsAsync()
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return Unauthorized();

                var allDocuments = await _verificationService.GetPendingVerificationDocumentsAsync();
                var stats = allDocuments
                    .GroupBy(d => d.DocumentType)
                    .Select(g => new
                    {
                        type = g.Key.ToString(),
                        typeName = g.First().DocumentTypeName,
                        count = g.Count()
                    })
                    .ToList();

                return new JsonResult(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra" });
            }
        }

        public async Task<IActionResult> OnGetVerificationStatusStatsAsync()
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return Unauthorized();

                var allDocuments = await _verificationService.GetPendingVerificationDocumentsAsync();
                var stats = allDocuments
                    .GroupBy(d => d.VerificationStatus)
                    .Select(g => new
                    {
                        status = g.Key.ToString(),
                        statusName = g.First().VerificationStatusName,
                        count = g.Count()
                    })
                    .ToList();

                return new JsonResult(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra" });
            }
        }

        private GeneralUserDto? GetCurrentUser()
        {
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
                return null;

            return JsonConvert.DeserializeObject<GeneralUserDto>(userJson);
        }
    }

    public class DocumentManagementStatistics
    {
        public int TotalDocuments { get; set; }
        public int PendingDocuments { get; set; }
        public int VerifiedDocuments { get; set; }
        public int ExpiredDocuments { get; set; }
        public int ExpiringSoonDocuments { get; set; }
        public int RejectedDocuments { get; set; }
    }
}