using BusinessObjects.DTOs.Professional;
using BusinessObjects.Commons;
using BusinessObjects.Enums;

namespace Services.Interfaces
{
    public interface IProfessionalVerificationService
    {
        Task<Result<ProfessionalDocumentDto>> UploadDocumentAsync(UploadDocumentDto uploadDto);
        Task<Result<bool>> VerifyDocumentAsync(VerifyDocumentDto verifyDto);
        Task<DocumentVerificationSummaryDto> GetVerificationSummaryAsync(int professionalId);
        Task<List<ProfessionalDocumentDto>> GetExpiringDocumentsAsync(int daysAhead = 30);
        Task<bool> CanProfessionalProvideServicesAsync(int professionalId);
        Task<List<ProfessionalDocumentDto>> GetDocumentsByProfessionalAsync(int professionalId);
        Task<List<ProfessionalDocumentDto>> GetPendingVerificationDocumentsAsync();
        Task<Result<bool>> RequestAdditionalDocumentsAsync(int professionalId, string reason, List<DocumentType> requiredDocuments);
    }
}
