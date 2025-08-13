using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.AdminDashboard
{
    public class MonthlyPaymentDto
    {
        public int Month { get; set; } 
        public decimal Total { get; set; }
    }
}
