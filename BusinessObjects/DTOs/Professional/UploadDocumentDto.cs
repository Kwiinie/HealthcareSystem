using BusinessObjects.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Professional
{
    public class UploadDocumentDto
    {
        public int ProfessionalId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string DocumentName { get; set; } = string.Empty;
        public string? DocumentNumber { get; set; }
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public string? IssuingAuthority { get; set; }
        public IFormFile DocumentFile { get; set; } = null!;
    }
}
