using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Professional
{
    public class DocumentVerificationSummaryDto
    {
        public int ProfessionalId { get; set; }
        public string ProfessionalName { get; set; } = string.Empty;
        public int TotalDocuments { get; set; }
        public int VerifiedDocuments { get; set; }
        public int PendingDocuments { get; set; }
        public int RejectedDocuments { get; set; }
        public bool HasRequiredDocuments { get; set; }
        public bool AllDocumentsVerified { get; set; }
        public List<string> MissingRequiredDocuments { get; set; } = new();
        public DateTime? LatestSubmissionDate { get; set; }
    }
}
