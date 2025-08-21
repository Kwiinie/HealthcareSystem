using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Enums
{
    public enum DocumentType
    {
        [Display(Name = "Bằng tốt nghiệp bác sĩ")]
        MedicalDegree,

        [Display(Name = "Giấy phép hành nghề")]
        MedicalLicense,

        [Display(Name = "Chứng chỉ chuyên khoa")]
        SpecialtyCertificate,

        [Display(Name = "Chứng chỉ thực hành")]
        PracticeCertificate,

        [Display(Name = "Chứng minh thư / Căn cước công dân")]
        IdentityDocument,

        [Display(Name = "Chứng chỉ đào tạo liên tục")]
        ContinuingEducationCertificate,

        [Display(Name = "Chứng chỉ khác")]
        Other
    }
}
