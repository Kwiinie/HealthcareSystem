using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
    public enum ProfessionalRequestStatus
    {
        [Display(Name = "Đang chờ duyệt")]
        Pending,

        [Display(Name = "Đã phê duyệt")]
        Approved,

        [Display(Name = "Bị từ chối")]
        Rejected
    }

}
