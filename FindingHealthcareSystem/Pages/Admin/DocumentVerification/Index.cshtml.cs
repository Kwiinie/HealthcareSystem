using BusinessObjects.DTOs.Professional;
using BusinessObjects.Dtos.User;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.DocumentVerification
{
    public class IndexModel : PageModel
    {
        private readonly IProfessionalVerificationService _verificationService;

        public IndexModel(IProfessionalVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        public List<ProfessionalDocumentDto> Documents { get; set; } = new();
        public DocumentVerificationStatistics Statistics { get; set; } = new();

        // Filter properties
        public string? FilterStatus { get; set; }
        public string? FilterDocumentType { get; set; }
        public DateTime? FilterFromDate { get; set; }

        public async Task<IActionResult> OnGetAsync(string? status, string? documentType, DateTime? fromDate)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return RedirectToPage("/Auth/Login");

                FilterStatus = status;
                FilterDocumentType = documentType;
                FilterFromDate = fromDate;

                // Get all documents for verification
                Documents = await GetFilteredDocumentsAsync();
                Statistics = await GetDocumentStatisticsAsync();

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải dữ liệu";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostVerifyDocumentAsync(int documentId, string verificationStatus, string? adminNotes)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return Unauthorized();

                Console.WriteLine($"Verifying document ID: {documentId}, Status: {verificationStatus}, Admin: {currentUser.Id}");

                var verifyDto = new VerifyDocumentDto
                {
                    DocumentId = documentId,
                    VerificationStatus = DocumentVerificationStatus.Verified, // Always set to Verified for this handler
                    AdminNotes = adminNotes,
                    ReviewedByUserId = currentUser.Id
                };

                var result = await _verificationService.VerifyDocumentAsync(verifyDto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Đã xác thực chứng chỉ thành công";
                    Console.WriteLine("Verification successful");
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage ?? "Có lỗi xảy ra khi xác thực";
                    Console.WriteLine($"Verification failed: {result.ErrorMessage}");
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in verification: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xử lý xác thực";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostRejectDocumentAsync(int documentId, string rejectionReason, string? adminNotes)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return Unauthorized();

                Console.WriteLine($"Rejecting document ID: {documentId}, Reason: {rejectionReason}, Admin: {currentUser.Id}");

                var verifyDto = new VerifyDocumentDto
                {
                    DocumentId = documentId,
                    VerificationStatus = DocumentVerificationStatus.Rejected,
                    RejectionReason = rejectionReason,
                    AdminNotes = adminNotes,
                    ReviewedByUserId = currentUser.Id
                };

                var result = await _verificationService.VerifyDocumentAsync(verifyDto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Đã từ chối chứng chỉ thành công";
                    Console.WriteLine("Rejection successful");
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage ?? "Có lỗi xảy ra khi từ chối";
                    Console.WriteLine($"Rejection failed: {result.ErrorMessage}");
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in rejection: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi từ chối chứng chỉ";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnPostSetUnderReviewAsync(int documentId, string? adminNotes)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return Unauthorized();

                Console.WriteLine($"Setting under review for document ID: {documentId}, Admin: {currentUser.Id}");

                var verifyDto = new VerifyDocumentDto
                {
                    DocumentId = documentId,
                    VerificationStatus = DocumentVerificationStatus.UnderReview,
                    AdminNotes = adminNotes,
                    ReviewedByUserId = currentUser.Id
                };

                var result = await _verificationService.VerifyDocumentAsync(verifyDto);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Đã đặt trạng thái đang xem xét thành công";
                    Console.WriteLine("Set under review successful");
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage ?? "Có lỗi xảy ra khi đặt trạng thái";
                    Console.WriteLine($"Set under review failed: {result.ErrorMessage}");
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in set under review: {ex.Message}");
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi đặt trạng thái";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnGetDocumentDetailsAsync(int documentId)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return Unauthorized();

                var documents = await _verificationService.GetPendingVerificationDocumentsAsync();
                var document = documents.FirstOrDefault(d => d.Id == documentId);

                if (document == null)
                    return NotFound();

                return new JsonResult(new
                {
                    success = true,
                    data = new
                    {
                        id = document.Id,
                        professionalId = document.ProfessionalId,
                        professionalName = document.ProfessionalName,
                        documentName = document.DocumentName,
                        documentTypeName = document.DocumentTypeName,
                        documentNumber = document.DocumentNumber,
                        issueDate = document.IssueDate?.ToString("dd/MM/yyyy"),
                        expiryDate = document.ExpiryDate?.ToString("dd/MM/yyyy"),
                        issuingAuthority = document.IssuingAuthority,
                        documentUrl = document.DocumentUrl,
                        verificationStatus = document.VerificationStatus.ToString(),
                        verificationStatusName = document.VerificationStatusName,
                        adminNotes = document.AdminNotes,
                        rejectionReason = document.RejectionReason,
                        createdAt = document.CreatedAt?.ToString("dd/MM/yyyy HH:mm"),
                        reviewedByName = document.ReviewedByName,
                        reviewedAt = document.ReviewedAt?.ToString("dd/MM/yyyy HH:mm"),
                        fileSizeBytes = document.FileSizeBytes,
                        fileExtension = document.FileExtension,
                        originalFileName = document.OriginalFileName,
                        isExpired = document.IsExpired,
                        isExpiringSoon = document.IsExpiringSoon
                    }
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra" });
            }
        }
        public async Task<IActionResult> OnGetExpiringDocumentsAsync(int daysAhead = 30)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser?.Role != BusinessObjects.Enums.Role.Admin.ToString())
                    return Unauthorized();

                var expiringDocs = await _verificationService.GetExpiringDocumentsAsync(daysAhead);

                return new JsonResult(new
                {
                    success = true,
                    data = expiringDocs.Select(d => new
                    {
                        id = d.Id,
                        professionalName = d.ProfessionalName,
                        documentName = d.DocumentName,
                        documentTypeName = d.DocumentTypeName,
                        expiryDate = d.ExpiryDate?.ToString("dd/MM/yyyy"),
                        daysUntilExpiry = d.ExpiryDate.HasValue
                            ? (d.ExpiryDate.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days
                            : (int?)null
                    })
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra" });
            }
        }

        private async Task<List<ProfessionalDocumentDto>> GetFilteredDocumentsAsync()
        {
            var allDocuments = await _verificationService.GetPendingVerificationDocumentsAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(FilterStatus) && Enum.TryParse<DocumentVerificationStatus>(FilterStatus, out var status))
            {
                allDocuments = allDocuments.Where(d => d.VerificationStatus == status).ToList();
            }

            if (!string.IsNullOrEmpty(FilterDocumentType) && Enum.TryParse<DocumentType>(FilterDocumentType, out var docType))
            {
                allDocuments = allDocuments.Where(d => d.DocumentType == docType).ToList();
            }

            if (FilterFromDate.HasValue)
            {
                allDocuments = allDocuments.Where(d => d.CreatedAt >= FilterFromDate.Value).ToList();
            }

            return allDocuments.OrderByDescending(d => d.CreatedAt).ToList();
        }

        private async Task<DocumentVerificationStatistics> GetDocumentStatisticsAsync()
        {
            var allDocuments = await _verificationService.GetPendingVerificationDocumentsAsync();

            return new DocumentVerificationStatistics
            {
                PendingVerification = allDocuments.Count(d => d.VerificationStatus == DocumentVerificationStatus.PendingVerification),
                UnderReview = allDocuments.Count(d => d.VerificationStatus == DocumentVerificationStatus.UnderReview),
                Verified = allDocuments.Count(d => d.VerificationStatus == DocumentVerificationStatus.Verified),
                Rejected = allDocuments.Count(d => d.VerificationStatus == DocumentVerificationStatus.Rejected)
            };
        }

        private GeneralUserDto? GetCurrentUser()
        {
            var userJson = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(userJson))
                return null;

            return JsonConvert.DeserializeObject<GeneralUserDto>(userJson);
        }
    }

    public class DocumentVerificationStatistics
    {
        public int PendingVerification { get; set; }
        public int UnderReview { get; set; }
        public int Verified { get; set; }
        public int Rejected { get; set; }
    }
}