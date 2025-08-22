using BusinessObjects.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class Schedule : BaseEntity
    {
        public int ProfessionalId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Professional? Professional { get; set; }
        public ICollection<WorkingDate> WorkingDates { get; set; }

    }
}
