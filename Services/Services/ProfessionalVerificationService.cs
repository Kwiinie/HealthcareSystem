using BusinessObjects.DTOs.Professional;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using BusinessObjects.Commons;

namespace Services.Services
{
    public class ProfessionalVerificationService : IProfessionalVerificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileUploadService _fileUploadService;
        
        // Business Rules Configuration
        private static readonly Dictionary<DocumentType, bool> RequiredDocuments = new()
        {
            { DocumentType.MedicalDegree, true },
            { DocumentType.MedicalLicense, true },
            { DocumentType.IdentityDocument, true },
            { DocumentType.SpecialtyCertificate, false }, // Optional, depends on specialty
            { DocumentType.PracticeCertificate, false },
            { DocumentType.ContinuingEducationCertificate, false }
        };

        private static readonly Dictionary<DocumentType, int> DocumentValidityMonths = new()
        {
            { DocumentType.MedicalLicense, 60 }, // 5 years
            { DocumentType.SpecialtyCertificate, 36 }, // 3 years
            { DocumentType.PracticeCertificate, 24 }, // 2 years
            { DocumentType.ContinuingEducationCertificate, 12 }, // 1 year
            { DocumentType.MedicalDegree, 0 }, // Never expires
            { DocumentType.IdentityDocument, 0 } // Never expires (handled separately)
        };

        private const int ExpiryWarningDays = 30;
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB
        private static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };

        public ProfessionalVerificationService(IUnitOfWork unitOfWork, IFileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        public async Task<BusinessObjects.Commons.Result<ProfessionalDocumentDto>> UploadDocumentAsync(UploadDocumentDto uploadDto)
        {
            try
            {
                // Validate file
                var fileValidation = ValidateDocumentFile(uploadDto.DocumentFile);
                if (!fileValidation.IsSuccess)
                    return BusinessObjects.Commons.Result<ProfessionalDocumentDto>.Failure(fileValidation.ErrorMessage);

                // Check if professional exists
                var professional = await _unitOfWork.ProfessionalRepository.GetByIdAsync(uploadDto.ProfessionalId);
                if (professional == null)
                    return BusinessObjects.Commons.Result<ProfessionalDocumentDto>.Failure("Không tìm thấy thông tin bác sĩ");

                // Check if document type already exists (for unique documents)
                if (IsUniqueDocumentType(uploadDto.DocumentType))
                {
                    var existingDoc = await GetExistingDocument(uploadDto.ProfessionalId, uploadDto.DocumentType);
                    if (existingDoc != null && existingDoc.VerificationStatus != DocumentVerificationStatus.Rejected)
                    {
                        return BusinessObjects.Commons.Result<ProfessionalDocumentDto>.Failure(
                            $"Loại chứng chỉ {GetDocumentTypeName(uploadDto.DocumentType)} đã được tải lên trước đó");
                    }
                }

                // Upload file to cloud storage
                var uploadResult = await _fileUploadService.UploadAsync(
                    uploadDto.DocumentFile, 
                    $"professional-documents/{uploadDto.ProfessionalId}");

                if (!uploadResult.IsSuccess)
                    return BusinessObjects.Commons.Result<ProfessionalDocumentDto>.Failure(uploadResult.ErrorMessage);

                // Create document entity
                var document = new ProfessionalDocument
                {
                    ProfessionalId = uploadDto.ProfessionalId,
                    DocumentType = uploadDto.DocumentType,
                    DocumentName = uploadDto.DocumentName,
                    DocumentUrl = uploadResult.Data,
                    DocumentNumber = uploadDto.DocumentNumber,
                    IssueDate = uploadDto.IssueDate,
                    ExpiryDate = uploadDto.ExpiryDate,
                    IssuingAuthority = uploadDto.IssuingAuthority,
                    VerificationStatus = DocumentVerificationStatus.PendingVerification,
                    FileSizeBytes = uploadDto.DocumentFile.Length,
                    FileExtension = Path.GetExtension(uploadDto.DocumentFile.FileName),
                    OriginalFileName = uploadDto.DocumentFile.FileName,
                    CreatedAt = DateTime.UtcNow
                };

                // Validate document details
                var detailValidation = ValidateDocumentDetails(document);
                if (!detailValidation.IsSuccess)
                    return BusinessObjects.Commons.Result<ProfessionalDocumentDto>.Failure(detailValidation.ErrorMessage);

                await _unitOfWork.ProfessionalDocumentRepository.AddAsync(document);
                await _unitOfWork.SaveChangesAsync();

                // Update professional status if needed
                await UpdateProfessionalStatusAfterUpload(uploadDto.ProfessionalId);

                var documentDto = MapToDto(document);
                return BusinessObjects.Commons.Result<ProfessionalDocumentDto>.Success(documentDto);
            }
            catch (Exception ex)
            {
                return BusinessObjects.Commons.Result<ProfessionalDocumentDto>.Failure($"Lỗi khi tải lên tài liệu: {ex.Message}");
            }
        }

        public async Task<BusinessObjects.Commons.Result<bool>> VerifyDocumentAsync(VerifyDocumentDto verifyDto)
        {
            try
            {
                var document = await _unitOfWork.ProfessionalDocumentRepository.GetByIdAsync(verifyDto.DocumentId);
                if (document == null)
                    return BusinessObjects.Commons.Result<bool>.Failure("Không tìm thấy tài liệu");

                // Business rule: Only admins can verify documents
                var previousStatus = document.VerificationStatus;
                
                document.VerificationStatus = verifyDto.VerificationStatus;
                document.AdminNotes = verifyDto.AdminNotes;
                document.RejectionReason = verifyDto.RejectionReason;
                document.ReviewedByUserId = verifyDto.ReviewedByUserId;
                document.ReviewedAt = DateTime.UtcNow;
                document.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.ProfessionalDocumentRepository.UpdateAsync(document);

                // Update professional status based on overall document verification status
                await UpdateProfessionalStatusAfterVerification(document.ProfessionalId);

                await _unitOfWork.SaveChangesAsync();

                return BusinessObjects.Commons.Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BusinessObjects.Commons.Result<bool>.Failure($"Lỗi khi xác thực tài liệu: {ex.Message}");
            }
        }

        public async Task<DocumentVerificationSummaryDto> GetVerificationSummaryAsync(int professionalId)
        {
            var documents = await _unitOfWork.ProfessionalDocumentRepository.GetByProfessionalIdAsync(professionalId);
            var professional = await _unitOfWork.ProfessionalRepository.GetByIdAsync(professionalId);

            var summary = new DocumentVerificationSummaryDto
            {
                ProfessionalId = professionalId,
                ProfessionalName = professional?.Fullname ?? "Không xác định",
                TotalDocuments = documents.Count,
                VerifiedDocuments = documents.Count(d => d.VerificationStatus == DocumentVerificationStatus.Verified),
                PendingDocuments = documents.Count(d => d.VerificationStatus == DocumentVerificationStatus.PendingVerification || 
                                                        d.VerificationStatus == DocumentVerificationStatus.UnderReview),
                RejectedDocuments = documents.Count(d => d.VerificationStatus == DocumentVerificationStatus.Rejected),
                LatestSubmissionDate = documents.OrderByDescending(d => d.CreatedAt).FirstOrDefault()?.CreatedAt
            };

            // Check required documents
            var missingRequired = new List<string>();
            var submittedDocTypes = documents.Select(d => d.DocumentType).ToHashSet();
            
            foreach (var requiredDoc in RequiredDocuments.Where(r => r.Value))
            {
                if (!submittedDocTypes.Contains(requiredDoc.Key))
                {
                    missingRequired.Add(GetDocumentTypeName(requiredDoc.Key));
                }
            }

            summary.MissingRequiredDocuments = missingRequired;
            summary.HasRequiredDocuments = missingRequired.Count == 0;
            summary.AllDocumentsVerified = summary.HasRequiredDocuments && 
                                         summary.PendingDocuments == 0 && 
                                         summary.RejectedDocuments == 0;

            return summary;
        }

        public async Task<List<ProfessionalDocumentDto>> GetExpiringDocumentsAsync(int daysAhead = 30)
        {
            var cutoffDate = DateOnly.FromDateTime(DateTime.Now.AddDays(daysAhead));
            var documents = await _unitOfWork.ProfessionalDocumentRepository.GetExpiringDocumentsAsync(cutoffDate);
            
            return documents.Where(d => d.ExpiryDate.HasValue && d.ExpiryDate <= cutoffDate)
                           .Select(MapToDto)
                           .ToList();
        }

        public async Task<bool> CanProfessionalProvideServicesAsync(int professionalId)
        {
            var summary = await GetVerificationSummaryAsync(professionalId);
            var professional = await _unitOfWork.ProfessionalRepository.GetByIdAsync(professionalId);

            // Business rules for providing services:
            // 1. Professional must be approved
            // 2. All required documents must be submitted and verified
            // 3. No expired critical documents
            
            if (professional?.RequestStatus != ProfessionalRequestStatus.Approved)
                return false;

            if (!summary.AllDocumentsVerified)
                return false;

            // Check for expired critical documents
            var documents = await _unitOfWork.ProfessionalDocumentRepository.GetByProfessionalIdAsync(professionalId);
            var criticalDocTypes = new[] { DocumentType.MedicalLicense, DocumentType.PracticeCertificate };
            
            foreach (var doc in documents.Where(d => criticalDocTypes.Contains(d.DocumentType)))
            {
                if (IsDocumentExpired(doc))
                    return false;
            }

            return true;
        }

        public async Task<List<ProfessionalDocumentDto>> GetDocumentsByProfessionalAsync(int professionalId)
        {
            var documents = await _unitOfWork.ProfessionalDocumentRepository.GetByProfessionalIdAsync(professionalId);
            return documents.Select(MapToDto).ToList();
        }

        public async Task<List<ProfessionalDocumentDto>> GetPendingVerificationDocumentsAsync()
        {
            var pendingDocuments = await _unitOfWork.ProfessionalDocumentRepository.GetByVerificationStatusAsync(
                DocumentVerificationStatus.PendingVerification);
            return pendingDocuments.Select(MapToDto).ToList();
        }

        public async Task<Result<bool>> RequestAdditionalDocumentsAsync(int professionalId, string reason, List<DocumentType> requiredDocuments)
        {
            try
            {
                var professional = await _unitOfWork.ProfessionalRepository.GetByIdAsync(professionalId);
                if (professional == null)
                    return Result<bool>.Failure("Không tìm thấy thông tin bác sĩ");

                // Update professional status to require additional documents
                professional.RequestStatus = ProfessionalRequestStatus.RequiresAdditionalDocuments;
                professional.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.ProfessionalRepository.UpdateAsync(professional);
                await _unitOfWork.SaveChangesAsync();

                // TODO: Send notification to professional about required documents
                // This could be implemented using email service or in-app notifications

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Lỗi khi yêu cầu tài liệu bổ sung: {ex.Message}");
            }
        }

        // Private helper methods
        private BusinessObjects.Commons.Result<bool> ValidateDocumentFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BusinessObjects.Commons.Result<bool>.Failure("Vui lòng chọn tệp để tải lên");

            if (file.Length > MaxFileSizeBytes)
                return BusinessObjects.Commons.Result<bool>.Failure($"Kích thước tệp không được vượt quá {MaxFileSizeBytes / (1024 * 1024)}MB");

            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
                return BusinessObjects.Commons.Result<bool>.Failure($"Chỉ chấp nhận các định dạng: {string.Join(", ", AllowedExtensions)}");

            return BusinessObjects.Commons.Result<bool>.Success(true);
        }

        private BusinessObjects.Commons.Result<bool> ValidateDocumentDetails(ProfessionalDocument document)
        {
            // Validate expiry date for documents that should have one
            if (DocumentValidityMonths.ContainsKey(document.DocumentType) && 
                DocumentValidityMonths[document.DocumentType] > 0)
            {
                if (!document.ExpiryDate.HasValue)
                    return BusinessObjects.Commons.Result<bool>.Failure($"Loại chứng chỉ {GetDocumentTypeName(document.DocumentType)} cần có ngày hết hạn");

                if (document.ExpiryDate.Value < DateOnly.FromDateTime(DateTime.Now))
                    return BusinessObjects.Commons.Result<bool>.Failure("Chứng chỉ đã hết hạn");
            }

            // Validate license number for critical documents
            var documentsRequiringNumber = new[] { DocumentType.MedicalLicense, DocumentType.IdentityDocument };
            if (documentsRequiringNumber.Contains(document.DocumentType) && string.IsNullOrWhiteSpace(document.DocumentNumber))
            {
                return BusinessObjects.Commons.Result<bool>.Failure($"Loại chứng chỉ {GetDocumentTypeName(document.DocumentType)} cần có số hiệu");
            }

            return BusinessObjects.Commons.Result<bool>.Success(true);
        }

        private bool IsUniqueDocumentType(DocumentType documentType)
        {
            // These document types should only have one instance per professional
            var uniqueTypes = new[] { 
                DocumentType.MedicalDegree, 
                DocumentType.MedicalLicense, 
                DocumentType.IdentityDocument 
            };
            return uniqueTypes.Contains(documentType);
        }

        private async Task<ProfessionalDocument?> GetExistingDocument(int professionalId, DocumentType documentType)
        {
            var documents = await _unitOfWork.ProfessionalDocumentRepository.GetByProfessionalIdAsync(professionalId);
            return documents.FirstOrDefault(d => d.DocumentType == documentType);
        }

        private async Task UpdateProfessionalStatusAfterUpload(int professionalId)
        {
            var professional = await _unitOfWork.ProfessionalRepository.GetByIdAsync(professionalId);
            if (professional == null) return;

            var summary = await GetVerificationSummaryAsync(professionalId);
            
            // If all required documents are uploaded, move to pending verification
            if (summary.HasRequiredDocuments && professional.RequestStatus == ProfessionalRequestStatus.AwaitingDocuments)
            {
                professional.RequestStatus = ProfessionalRequestStatus.Pending;
                await _unitOfWork.ProfessionalRepository.UpdateAsync(professional);
            }
        }

        private async Task UpdateProfessionalStatusAfterVerification(int professionalId)
        {
            var professional = await _unitOfWork.ProfessionalRepository.GetByIdAsync(professionalId);
            if (professional == null) return;

            var summary = await GetVerificationSummaryAsync(professionalId);
            
            if (summary.AllDocumentsVerified)
            {
                professional.RequestStatus = ProfessionalRequestStatus.Approved;
            }
            else if (summary.RejectedDocuments > 0)
            {
                professional.RequestStatus = ProfessionalRequestStatus.RequiresAdditionalDocuments;
            }
            else if (summary.PendingDocuments > 0)
            {
                professional.RequestStatus = ProfessionalRequestStatus.DocumentVerification;
            }

            await _unitOfWork.ProfessionalRepository.UpdateAsync(professional);
        }

        private bool IsDocumentExpired(ProfessionalDocument document)
        {
            if (!document.ExpiryDate.HasValue) return false;
            return document.ExpiryDate.Value < DateOnly.FromDateTime(DateTime.Now);
        }

        private string GetDocumentTypeName(DocumentType documentType)
        {
            return documentType switch
            {
                DocumentType.MedicalDegree => "Bằng tốt nghiệp bác sĩ",
                DocumentType.MedicalLicense => "Giấy phép hành nghề",
                DocumentType.SpecialtyCertificate => "Chứng chỉ chuyên khoa",
                DocumentType.PracticeCertificate => "Chứng chỉ thực hành",
                DocumentType.IdentityDocument => "Chứng minh thư/Căn cước công dân",
                DocumentType.ContinuingEducationCertificate => "Chứng chỉ đào tạo liên tục",
                _ => "Chứng chỉ khác"
            };
        }

        private ProfessionalDocumentDto MapToDto(ProfessionalDocument document)
        {
            return new ProfessionalDocumentDto
            {
                Id = document.Id,
                ProfessionalId = document.ProfessionalId,
                DocumentType = document.DocumentType,
                DocumentName = document.DocumentName,
                DocumentUrl = document.DocumentUrl,
                DocumentNumber = document.DocumentNumber,
                IssueDate = document.IssueDate,
                ExpiryDate = document.ExpiryDate,
                IssuingAuthority = document.IssuingAuthority,
                VerificationStatus = document.VerificationStatus,
                AdminNotes = document.AdminNotes,
                ReviewedByUserId = document.ReviewedByUserId,
                ReviewedAt = document.ReviewedAt,
                RejectionReason = document.RejectionReason,
                FileSizeBytes = document.FileSizeBytes,
                FileExtension = document.FileExtension,
                OriginalFileName = document.OriginalFileName,
                DocumentTypeName = GetDocumentTypeName(document.DocumentType),
                VerificationStatusName = GetVerificationStatusName(document.VerificationStatus),
                IsExpired = IsDocumentExpired(document),
                IsExpiringSoon = document.ExpiryDate.HasValue && 
                               document.ExpiryDate.Value <= DateOnly.FromDateTime(DateTime.Now.AddDays(ExpiryWarningDays))
            };
        }

        private string GetVerificationStatusName(DocumentVerificationStatus status)
        {
            return status switch
            {
                DocumentVerificationStatus.PendingVerification => "Chờ xác thực",
                DocumentVerificationStatus.UnderReview => "Đang xem xét",
                DocumentVerificationStatus.Verified => "Đã xác thực",
                DocumentVerificationStatus.RequiresAdditionalInfo => "Cần bổ sung thông tin",
                DocumentVerificationStatus.Rejected => "Bị từ chối",
                DocumentVerificationStatus.Expired => "Hết hạn",
                DocumentVerificationStatus.RequiresRenewal => "Cần gia hạn",
                _ => status.ToString()
            };
        }
    }
}
