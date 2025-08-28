using BusinessObjects.DTOs.Professional;
using BusinessObjects.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Professional.Documents
{
    public class IndexModel : PageModel
    {
        private readonly IProfessionalVerificationService _verificationService;
        private readonly IProfessionalService _professionalService;

        public IndexModel(IProfessionalVerificationService verificationService, IProfessionalService professionalService)
        {
            _verificationService = verificationService;
            _professionalService = professionalService;
        }

        public List<ProfessionalDocumentDto> Documents { get; set; } = new();
        public DocumentVerificationSummaryDto Summary { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                    return RedirectToPage("/Auth/Login");

                var professional = await _professionalService.GetProfessionalByUserIdAsync(currentUser.Id);
                if (professional == null)
                    return RedirectToPage("/Professional/Profile/Create");

                Documents = await _verificationService.GetDocumentsByProfessionalAsync(professional.Id);
                Summary = await _verificationService.GetVerificationSummaryAsync(professional.Id);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải dữ liệu";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(UploadDocumentDto uploadDto)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                    return RedirectToPage("/Auth/Login");

                var professional = await _professionalService.GetProfessionalByUserIdAsync(currentUser.Id);
                if (professional == null)
                    return BadRequest("Không tìm thấy thông tin bác sĩ");

                uploadDto.ProfessionalId = professional.Id;

                var result = await _verificationService.UploadDocumentAsync(uploadDto);
                
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Tải lên chứng chỉ thành công! Chúng tôi sẽ xác thực trong thời gian sớm nhất.";
                }
                else
                {
                    TempData["ErrorMessage"] = result.ErrorMessage;
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi tải lên chứng chỉ";
                return RedirectToPage();
            }
        }

        public async Task<IActionResult> OnGetDocumentStatusAsync(int documentId)
        {
            try
            {
                var currentUser = GetCurrentUser();
                if (currentUser == null)
                    return Unauthorized();

                var professional = await _professionalService.GetProfessionalByUserIdAsync(currentUser.Id);
                if (professional == null)
                    return BadRequest();

                var documents = await _verificationService.GetDocumentsByProfessionalAsync(professional.Id);
                var document = documents.FirstOrDefault(d => d.Id == documentId);

                if (document == null)
                    return NotFound();

                return new JsonResult(new
                {
                    status = document.VerificationStatus.ToString(),
                    statusName = document.VerificationStatusName,
                    adminNotes = document.AdminNotes,
                    rejectionReason = document.RejectionReason,
                    reviewedAt = document.ReviewedAt
                });
            }
            catch
            {
                return BadRequest();
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
}
