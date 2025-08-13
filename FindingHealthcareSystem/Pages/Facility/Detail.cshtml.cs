using BusinessObjects.DTOs.Department;
using BusinessObjects.DTOs.Facility;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Entities;
using BusinessObjects.LocationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Facility
{
    public class DetailModel : PageModel
    {
        private readonly IFacilityService _facilityService;
        private readonly IDepartmentService _departmentService;
        private readonly IPublicServiceLayer _publicServiceLayer;
        private readonly IFacilityTypeService _facilityTypeService;
        private readonly ILocationService _locationService;

        public DetailModel(
            IFacilityService facilityService,
            IDepartmentService departmentService,
            IPublicServiceLayer publicServiceLayer,
            IFacilityTypeService facilityTypeService,
            ILocationService locationService)
        {
            _facilityService = facilityService;
            _departmentService = departmentService;
            _publicServiceLayer = publicServiceLayer;
            _facilityTypeService = facilityTypeService;
            _locationService = locationService;
        }

        [BindProperty(SupportsGet = true)]
        public int FacilityId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int DepartmentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ServiceId { get; set; }

        [BindProperty]
        public FacilityDto Facility { get; set; }

        [BindProperty]
        public List<DepartmentDto> Departments { get; set; }

        [BindProperty]
        public List<FacilityTypeDto> FacilityTypes { get; set; }

        [BindProperty]
        public List<ServiceDto> Services { get; set; }

        [BindProperty]
        public ServiceDto Service { get; set; }

        public bool IsEdit { get; private set; } = false;

        public async Task<IActionResult> OnGetAsync()
        {
            Facility = await _facilityService.GetById(FacilityId);
            if (Facility == null)
            {
                return NotFound("Facility not found");
            }

            Departments = await _departmentService.GetAllDepartments();
            Services = await _publicServiceLayer.GetServicesByFacilityId(FacilityId) ?? new List<ServiceDto>();
            FacilityTypes = await _facilityTypeService.GetAllActiveFacilityTypes();

            return Page();
        }

        public async Task<IActionResult> OnPostEditFacilityAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            var existingFacility = await _facilityService.GetById(FacilityId);
            if (existingFacility == null)
            {
                return NotFound("Facility not found");
            }

            await _facilityService.Update(FacilityId, Facility);
            return RedirectToPage("/Facility/Detail", new { FacilityId });
        }

        public async Task<IActionResult> OnPostDeleteFacilityAsync()
        {
            var existingFacility = await _facilityService.GetById(FacilityId);
            if (existingFacility == null)
            {
                return NotFound("Facility not found");
            }

            await _facilityService.DeleteAsync(FacilityId);
            return RedirectToPage("/Facility/Index");
        }

        public async Task<IActionResult> OnPostAddService()
        {
           

             var Name = Service.Name;
             var Price = Service.Price;
            var Description = Service.Description;

            await _publicServiceLayer.Create(FacilityId, Service);

            return RedirectToPage("/Facility/Detail", new { FacilityId });
        }

        // Cập nhật dịch vụ
        public async Task<IActionResult> OnPostEditServiceAsync()
        {
         

            var existingService = await _publicServiceLayer.GetPublicServiceById(ServiceId);
            if (existingService == null)
            {
                return NotFound();
            }

            existingService.Name = Service.Name;
            existingService.Price = Service.Price;
            existingService.Description = Service.Description;

            await _publicServiceLayer.Update(FacilityId, ServiceId, existingService);
            return RedirectToPage(); // Hoặc thay bằng trang bạn muốn quay về
        }

        // Xóa dịch vụ
        public async Task<IActionResult> OnPostDeleteServiceAsync()
        {
            var existingService = await _publicServiceLayer.GetPublicServiceById(ServiceId);
            if (existingService == null)
            {
                return NotFound();
            }

            await _publicServiceLayer.Delete( ServiceId);

            return RedirectToPage(); // Hoặc thay bằng trang bạn muốn quay về
        }


    }
}
