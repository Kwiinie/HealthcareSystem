using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
    public enum FacilityStatus
    {
        [Display(Name = "Đang hoạt động")]
        Active,

        [Display(Name = "Tạm ngưng")]
        Inactive,

        [Display(Name = "Đóng cửa")]
        Closed
    }
}
