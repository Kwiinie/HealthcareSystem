using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
    public enum UserStatus
    {
        [Display(Name = "Đang hoạt động")]
        Active,

        [Display(Name = "Không hoạt động")]
        Inactive
    }

}
