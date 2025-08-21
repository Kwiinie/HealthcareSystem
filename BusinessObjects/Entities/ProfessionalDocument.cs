using BusinessObjects.Commons;
using BusinessObjects.Enums;

namespace BusinessObjects.Entities;

public partial class ProfessionalDocument : BaseEntity
{
    public int ProfessionalId { get; set; }
    public DocumentType DocumentType { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string DocumentUrl { get; set; } = string.Empty;
    public string? DocumentNumber { get; set; } // Certificate/License number
    public DateOnly? IssueDate { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string? IssuingAuthority { get; set; } // Which institution issued the certificate
    public DocumentVerificationStatus VerificationStatus { get; set; }
    public string? AdminNotes { get; set; } // Admin comments during verification
    public int? ReviewedByUserId { get; set; } // Which admin reviewed this
    public DateTime? ReviewedAt { get; set; }
    public string? RejectionReason { get; set; }

    // File metadata
    public long FileSizeBytes { get; set; }
    public string FileExtension { get; set; } = string.Empty;
    public string? OriginalFileName { get; set; }

    // Navigation properties
    public virtual Professional Professional { get; set; } = null!;
    public virtual User? ReviewedByUser { get; set; }
}