using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Professional
{
    public class ProfessionalDocumentDto
    {
        public int? Id { get; set; }
        public int ProfessionalId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string DocumentUrl { get; set; } = string.Empty;
        public string? DocumentNumber { get; set; }
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public string? IssuingAuthority { get; set; }
        public DocumentVerificationStatus VerificationStatus { get; set; }
        public string? AdminNotes { get; set; }
        public int? ReviewedByUserId { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public string? RejectionReason { get; set; }
        public long FileSizeBytes { get; set; }
        public string FileExtension { get; set; } = string.Empty;
        public string? OriginalFileName { get; set; }

        // Additional properties for display
        public string? ReviewedByName { get; set; }
        public string DocumentTypeName { get; set; } = string.Empty;
        public string VerificationStatusName { get; set; } = string.Empty;
        public bool IsExpiringSoon { get; set; } // Within 30 days
        public bool IsExpired { get; set; }
        public string? ProfessionalName { get; set; } // Professional's full name for display
        public DateTime? CreatedAt { get; set; } // Document creation timestamp
    }
}
