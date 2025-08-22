using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.Configurations
{
    public class WorkingDateConfiguration : IEntityTypeConfiguration<WorkingDate>
    {
        public void Configure(EntityTypeBuilder<WorkingDate> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartTime).HasColumnType("time");
            entity.Property(e => e.EndTime).HasColumnType("time");
            entity.Property(e => e.SlotDuration).HasColumnType("time");
            entity.Property(e => e.SlotBuffer).HasColumnType("time");

            entity.HasIndex(e => new { e.ScheduleId, e.Weekday, e.StartTime, e.EndTime }).IsUnique();

            entity.HasIndex(e => new { e.ScheduleId, e.Weekday });


            entity.ToTable(t => t.HasCheckConstraint(
                "CK_WorkingDate_TimeRange",
                "[StartTime] < [EndTime]"
            ));
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_WorkingDate_Weekday",
                "[Weekday] BETWEEN 1 AND 7"
            ));

            entity.HasData(
        // ---------- Bác sĩ 1 (ScheduleId=1): Nghỉ thứ 3. Thứ 2 tách 2 buổi, Thứ 4 sáng, Thứ 6 chiều
        // Slot 20', Buffer 10'
        new WorkingDate { Id = 1, ScheduleId = 1, Weekday = 2, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(11, 30), SlotDuration = TimeSpan.FromMinutes(20), SlotBuffer = TimeSpan.FromMinutes(10) }, // Mon AM
        new WorkingDate { Id = 2, ScheduleId = 1, Weekday = 2, StartTime = new TimeOnly(13, 30), EndTime = new TimeOnly(17, 0), SlotDuration = TimeSpan.FromMinutes(20), SlotBuffer = TimeSpan.FromMinutes(10) }, // Mon PM
        new WorkingDate { Id = 3, ScheduleId = 1, Weekday = 4, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(12, 0), SlotDuration = TimeSpan.FromMinutes(20), SlotBuffer = TimeSpan.FromMinutes(10) }, // Wed AM
        new WorkingDate { Id = 4, ScheduleId = 1, Weekday = 6, StartTime = new TimeOnly(13, 30), EndTime = new TimeOnly(17, 0), SlotDuration = TimeSpan.FromMinutes(20), SlotBuffer = TimeSpan.FromMinutes(10) }, // Fri PM

        // ---------- Bác sĩ 2 (ScheduleId=2): Làm Mon–Fri 09:00–17:00 (1 buổi), nghỉ Sat & Sun
        // Slot 30', Buffer 5'
        new WorkingDate { Id = 5, ScheduleId = 2, Weekday = 2, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(5) },
        new WorkingDate { Id = 6, ScheduleId = 2, Weekday = 3, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(5) },
        new WorkingDate { Id = 7, ScheduleId = 2, Weekday = 4, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(5) },
        new WorkingDate { Id = 8, ScheduleId = 2, Weekday = 5, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(5) },
        new WorkingDate { Id = 9, ScheduleId = 2, Weekday = 6, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(17, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(5) },

        // ---------- Bác sĩ 3 (ScheduleId=3): Nghỉ thứ 3. Mon PM, Wed AM, Thu PM, Sat AM
        // Slot 25', Buffer 5'
        new WorkingDate { Id = 10, ScheduleId = 3, Weekday = 2, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(17, 0), SlotDuration = TimeSpan.FromMinutes(25), SlotBuffer = TimeSpan.FromMinutes(5) },  // Mon PM
        new WorkingDate { Id = 11, ScheduleId = 3, Weekday = 4, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(12, 0), SlotDuration = TimeSpan.FromMinutes(25), SlotBuffer = TimeSpan.FromMinutes(5) },  // Wed AM
        new WorkingDate { Id = 12, ScheduleId = 3, Weekday = 5, StartTime = new TimeOnly(13, 0), EndTime = new TimeOnly(17, 0), SlotDuration = TimeSpan.FromMinutes(25), SlotBuffer = TimeSpan.FromMinutes(5) },  // Thu PM
        new WorkingDate { Id = 13, ScheduleId = 3, Weekday = 7, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(12, 0), SlotDuration = TimeSpan.FromMinutes(25), SlotBuffer = TimeSpan.FromMinutes(5) },  // Sat AM

        // ---------- Bác sĩ 4 (ScheduleId=4): Mon–Fri 07:00–16:00 (1 buổi), nghỉ Sat & Sun
        // Slot 15', Buffer 5'
        new WorkingDate { Id = 14, ScheduleId = 4, Weekday = 2, StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(16, 0), SlotDuration = TimeSpan.FromMinutes(15), SlotBuffer = TimeSpan.FromMinutes(5) },
        new WorkingDate { Id = 15, ScheduleId = 4, Weekday = 3, StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(16, 0), SlotDuration = TimeSpan.FromMinutes(15), SlotBuffer = TimeSpan.FromMinutes(5) },
        new WorkingDate { Id = 16, ScheduleId = 4, Weekday = 4, StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(16, 0), SlotDuration = TimeSpan.FromMinutes(15), SlotBuffer = TimeSpan.FromMinutes(5) },
        new WorkingDate { Id = 17, ScheduleId = 4, Weekday = 5, StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(16, 0), SlotDuration = TimeSpan.FromMinutes(15), SlotBuffer = TimeSpan.FromMinutes(5) },
        new WorkingDate { Id = 18, ScheduleId = 4, Weekday = 6, StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(16, 0), SlotDuration = TimeSpan.FromMinutes(15), SlotBuffer = TimeSpan.FromMinutes(5) },

        // ---------- Bác sĩ 5 (ScheduleId=5): Tue tách 2 buổi, Mon & Thu chỉ sáng, Sat sáng; nghỉ Wed & Fri
        // Slot 30', Buffer 15'
        new WorkingDate { Id = 19, ScheduleId = 5, Weekday = 2, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(12, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(15) }, // Mon AM
        new WorkingDate { Id = 20, ScheduleId = 5, Weekday = 3, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(11, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(15) }, // Tue AM
        new WorkingDate { Id = 21, ScheduleId = 5, Weekday = 3, StartTime = new TimeOnly(14, 0), EndTime = new TimeOnly(18, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(15) }, // Tue PM
        new WorkingDate { Id = 22, ScheduleId = 5, Weekday = 5, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(12, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(15) }, // Thu AM
        new WorkingDate { Id = 23, ScheduleId = 5, Weekday = 7, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(11, 0), SlotDuration = TimeSpan.FromMinutes(30), SlotBuffer = TimeSpan.FromMinutes(15) }  // Sat AM
    );
        }
    }
}
