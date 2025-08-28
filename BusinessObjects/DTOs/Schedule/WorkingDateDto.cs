using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Schedule
{
    public class WorkingDateDto
    {
        public int Weekday { get; set; }                 
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeSpan SlotDuration { get; set; }
        public TimeSpan SlotBuffer { get; set; }
    }
}
