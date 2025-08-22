using AutoMapper;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.DTOs.Schedule;
using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class ActiveScheduleResolver : IValueResolver<Professional, ProfessionalDto, ScheduleDto?>
    {
        public ScheduleDto? Resolve(Professional src, ProfessionalDto dest, ScheduleDto? destMember, ResolutionContext ctx)
        {
            if (src.Schedules == null || !src.Schedules.Any()) return null;

            var today = DateOnly.FromDateTime(DateTime.Today);

            var covering = src.Schedules
                .Where(s => s.StartDate <= today && today <= s.EndDate)
                .OrderByDescending(s => s.EndDate)
                .FirstOrDefault();

            if (covering != null) return ctx.Mapper.Map<ScheduleDto>(covering);

            var upcoming = src.Schedules
                .Where(s => s.StartDate > today)
                .OrderBy(s => s.StartDate)
                .FirstOrDefault();

            return upcoming != null ? ctx.Mapper.Map<ScheduleDto>(upcoming) : null;
        }
    }

}
