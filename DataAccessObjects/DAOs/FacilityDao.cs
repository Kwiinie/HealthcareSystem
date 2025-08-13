using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Entities;
using DataAccessObjects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects.DAOs
{
    public class FacilityDao : IFacilityDao
    {
        private readonly FindingHealthcareSystemContext _context;

        public FacilityDao(FindingHealthcareSystemContext context)
        {
            _context = context;
        }

        public async Task<Facility> GetByIdWithRelationsAsync(int id)
        {
            return await _context.Facilities
                .Include(f => f.Type)
                .Include(f => f.PublicServices)
                .Include(f => f.FacilityDepartments)
                    .ThenInclude(fd => fd.Department)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Facility> CreateAsync(Facility facility)
        {
            await _context.Facilities.AddAsync(facility);
            await _context.SaveChangesAsync();
            return facility;
        }

        public async Task CreateFacilityDepartmentsAsync(List<FacilityDepartment> facilityDepartments)
        {
            await _context.FacilityDepartments.AddRangeAsync(facilityDepartments);
        }

        public async Task UpdateFacilityDepartmentsAsync(int facilityId, List<int> departmentIds)
        {
            var existingFacilityDepartments = await _context.FacilityDepartments
                .Where(fd => fd.FacilityId == facilityId)
                .ToListAsync();

            var activeDepartmentIds = existingFacilityDepartments
                .Where(fd => !fd.IsDeleted)
                .Select(fd => fd.DepartmentId)
                .ToList();

            var departmentsToAdd = departmentIds.Cast<int?>().Except(activeDepartmentIds).ToList();
            var departmentsToRemove = activeDepartmentIds.Except(departmentIds.Cast<int?>()).ToList();

            // Soft delete: Set IsDeleted = true
            if (departmentsToRemove.Count > 0)
            {
                foreach (var facilityDepartment in existingFacilityDepartments
                             .Where(fd => departmentsToRemove.Contains(fd.DepartmentId)))
                {
                    facilityDepartment.IsDeleted = true;
                    facilityDepartment.UpdatedAt = DateTime.UtcNow.AddHours(7);
                }
            }

            // Restore any previously deleted entries
            var restoreFacilityDepartments = existingFacilityDepartments
                .Where(fd => fd.IsDeleted && departmentIds.Contains((int)fd.DepartmentId))
                .ToList();

            foreach (var fd in restoreFacilityDepartments)
            {
                fd.IsDeleted = false;
                fd.UpdatedAt = DateTime.UtcNow.AddHours(7);
                departmentsToAdd.Remove(fd.DepartmentId ?? 0); // Prevent duplicate addition
            }

            // Insert new FacilityDepartments
            if (departmentsToAdd.Count > 0)
            {
                var newFacilityDepartments = departmentsToAdd.Select(deptId => new FacilityDepartment
                {
                    FacilityId = facilityId,
                    DepartmentId = deptId,
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    IsDeleted = false
                }).ToList();

                await _context.FacilityDepartments.AddRangeAsync(newFacilityDepartments);
            }

            await _context.SaveChangesAsync();
        }


    }
}
