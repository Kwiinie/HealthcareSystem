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
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.EndDate).HasColumnType("date");

            entity.HasOne(e => e.Professional)
                  .WithMany(p => p.Schedules)
                  .HasForeignKey(e => e.ProfessionalId)
                  .HasConstraintName("FK_Schedule_Professional")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.WorkingDates)
                  .WithOne(w => w.Schedule)
                  .HasForeignKey(w => w.ScheduleId)
                  .HasConstraintName("FK_WorkingDate_Schedule")
                  .OnDelete(DeleteBehavior.Cascade);

            entity.ToTable(t => t.HasCheckConstraint(
                "CK_Schedule_DateRange",
                "[StartDate] <= [EndDate]"
            ));

            entity.HasData(
       new Schedule { Id = 1, ProfessionalId = 1, StartDate = new DateOnly(2025, 8, 21), EndDate = new DateOnly(2025, 8, 29) },
       new Schedule { Id = 2, ProfessionalId = 2, StartDate = new DateOnly(2025, 8, 21), EndDate = new DateOnly(2025, 8, 29) },
       new Schedule { Id = 3, ProfessionalId = 3, StartDate = new DateOnly(2025, 8, 21), EndDate = new DateOnly(2025, 8, 29) },
       new Schedule { Id = 4, ProfessionalId = 4, StartDate = new DateOnly(2025, 8, 21), EndDate = new DateOnly(2025, 8, 29) },
       new Schedule { Id = 5, ProfessionalId = 5, StartDate = new DateOnly(2025, 8, 21), EndDate = new DateOnly(2025, 8, 29) }
   );
        }
    }
}
