using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs.Schedule
{
    public class ScheduleExceptionDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public bool IsClosed { get; set; }
        public TimeOnly? NewStartTime { get; set; }
        public TimeOnly? NewEndTime { get; set; }
        public string? Note { get; set; }
    }
}
