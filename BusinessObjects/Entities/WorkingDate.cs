using BusinessObjects.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public class  WorkingDate : BaseEntity
    {
        public int ScheduleId { get; set; }
        public int Weekday {  get; set; } //sun:1, mon:2, tue:3, wed:4, thu:5, fri:6, sat:7 
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeSpan SlotDuration { get; set; }
        public TimeSpan SlotBuffer {  get; set; }
        public Schedule Schedule { get; set; }
    }
}
