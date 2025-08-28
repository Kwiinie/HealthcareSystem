using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Professional
{
    public class VerifyDocumentDto
    {
        public int DocumentId { get; set; }
        public DocumentVerificationStatus VerificationStatus { get; set; }
        public string? AdminNotes { get; set; }
        public string? RejectionReason { get; set; }
        public int ReviewedByUserId { get; set; }
    }
}
