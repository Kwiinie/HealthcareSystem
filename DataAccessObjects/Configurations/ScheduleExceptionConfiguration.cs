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
    public class ScheduleExceptionConfiguration : IEntityTypeConfiguration<ScheduleException>
    {
        public void Configure(EntityTypeBuilder<ScheduleException> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.NewStartTime).HasColumnType("time");
            entity.Property(e => e.NewEndTime).HasColumnType("time");
            entity.Property(e => e.Note).HasMaxLength(1000);

            entity.HasOne<Professional>()
                  .WithMany(p => p.ScheduleExceptions)
                  .HasForeignKey(e => e.ProfessionalId)
                  .HasConstraintName("FK_ScheduleException_Professional")
                  .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable(t => t.HasCheckConstraint(
                "CK_ScheduleException_TimeOrClosed",
                "[IsClosed] = 1 OR [NewStartTime] < [NewEndTime]"
            ));

            entity.HasData(
        // BS1: 26/08 nghỉ đột xuất; 29/08 chỉ làm 09:00–12:00
        new ScheduleException { Id = 1, ProfessionalId = 1, Date = new DateOnly(2025, 8, 26), IsClosed = true, NewStartTime = new TimeOnly(0, 0), NewEndTime = new TimeOnly(0, 0), Note = "Nghỉ đột xuất" },
        new ScheduleException { Id = 2, ProfessionalId = 1, Date = new DateOnly(2025, 8, 29), IsClosed = false, NewStartTime = new TimeOnly(9, 0), NewEndTime = new TimeOnly(12, 0), Note = "Chỉ làm buổi sáng" },

        // BS2: 27/08 rút ngắn giờ 10:00–15:00
        new ScheduleException { Id = 3, ProfessionalId = 2, Date = new DateOnly(2025, 8, 27), IsClosed = false, NewStartTime = new TimeOnly(10, 0), NewEndTime = new TimeOnly(15, 0), Note = "Họp nội bộ" },

        // BS3: 24/08 (Chủ nhật) vốn không làm, thêm exception đóng cho rõ; 28/08 chỉ làm 08:30–11:30
        new ScheduleException { Id = 4, ProfessionalId = 3, Date = new DateOnly(2025, 8, 24), IsClosed = true, NewStartTime = new TimeOnly(0, 0), NewEndTime = new TimeOnly(0, 0), Note = "Đóng phòng khám" },
        new ScheduleException { Id = 5, ProfessionalId = 3, Date = new DateOnly(2025, 8, 28), IsClosed = false, NewStartTime = new TimeOnly(8, 30), NewEndTime = new TimeOnly(11, 30), Note = "Khám buổi sáng" },

        // BS4: 21/08 chỉ làm 08:00–12:00
        new ScheduleException { Id = 6, ProfessionalId = 4, Date = new DateOnly(2025, 8, 21), IsClosed = false, NewStartTime = new TimeOnly(8, 0), NewEndTime = new TimeOnly(12, 0), Note = "Đi hội thảo chiều" },

        // BS5: 23/08 (Thứ 7) nghỉ; 29/08 chỉ làm 08:00–11:00
        new ScheduleException { Id = 7, ProfessionalId = 5, Date = new DateOnly(2025, 8, 23), IsClosed = true, NewStartTime = new TimeOnly(0, 0), NewEndTime = new TimeOnly(0, 0), Note = "Nghỉ riêng" },
        new ScheduleException { Id = 8, ProfessionalId = 5, Date = new DateOnly(2025, 8, 29), IsClosed = false, NewStartTime = new TimeOnly(8, 0), NewEndTime = new TimeOnly(11, 0), Note = "Chỉ sáng" }
    );
        }
    }
}
