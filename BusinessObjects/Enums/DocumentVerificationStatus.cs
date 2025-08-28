using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Enums
{
    public enum DocumentVerificationStatus
    {
        [Display(Name = "Chờ xác thực")]
        PendingVerification,

        [Display(Name = "Đang xem xét")]
        UnderReview,

        [Display(Name = "Đã xác thực")]
        Verified,

        [Display(Name = "Cần bổ sung thông tin")]
        RequiresAdditionalInfo,

        [Display(Name = "Bị từ chối")]
        Rejected,

        [Display(Name = "Hết hạn")]
        Expired,

        [Display(Name = "Cần gia hạn")]
        RequiresRenewal
    }
}
