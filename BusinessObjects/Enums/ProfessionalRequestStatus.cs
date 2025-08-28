using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Enums
{
    public enum ProfessionalRequestStatus
    {
        [Display(Name = "Chờ nộp hồ sơ")]
        AwaitingDocuments,

        [Display(Name = "Đang chờ duyệt hồ sơ")]
        Pending,

        [Display(Name = "Đang xác thực chứng chỉ")]
        DocumentVerification,

        [Display(Name = "Cần bổ sung hồ sơ")]
        RequiresAdditionalDocuments,

        [Display(Name = "Đã phê duyệt")]
        Approved,

        [Display(Name = "Bị từ chối")]
        Rejected,

        [Display(Name = "Tạm đình chỉ")]
        Suspended
    }

}
