using BusinessObjects.DTOs.Facility;
using BusinessObjects.DTOs.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Facility.Service
{
    public class EditModel : PageModel
    {
        private readonly IFacilityService _facilityService;
        private readonly IDepartmentService _departmentService;
        private readonly IPublicServiceLayer _publicServiceLayer;
        private readonly IFacilityTypeService _facilityTypeService;

        public EditModel(IFacilityService facilityService, IDepartmentService departmentService, IPublicServiceLayer publicServiceLayer, IFacilityTypeService facilityTypeService)
        {
            _facilityService = facilityService;
            _departmentService = departmentService;
            _publicServiceLayer = publicServiceLayer;
            _facilityTypeService = facilityTypeService;
        }

        [BindProperty(SupportsGet = true)]
        public int FacilityId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ServiceId { get; set; }

        [BindProperty]
        public FacilityDto Facility { get; set; }

        [BindProperty]
        public List<FacilityTypeDto> FacilityTypes { get; set; }

        [BindProperty]
        public List<ServiceDto> Services { get; set; }

        [BindProperty]
        public ServiceDto Service { get; set; }

        public bool IsEdit { get; private set; } = false;

        public async Task OnGet()
        {
            Facility = await _facilityService.GetById(FacilityId);
            if (Facility == null)
            {
                throw new Exception("Facility not found");
            }
            Services = await _publicServiceLayer.GetAllFacilities() ?? new List<ServiceDto>();
            FacilityTypes = await _facilityTypeService.GetAllActiveFacilityTypes();
        }

        public async Task<IActionResult> OnGetAllServicesAsync()
        {
            Services = await _publicServiceLayer.GetAllFacilities();
            return Page();
        }

        public async Task<IActionResult> OnPostAddServiceAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _publicServiceLayer.Create(FacilityId, Service);
            //return RedirectToPage("/Facility/Detail", new { id = FacilityId });
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateServiceAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _publicServiceLayer.Update(FacilityId, ServiceId, Service);
            //return RedirectToPage("/Facility/Detail", new { id = FacilityId });
            return Page();
        }
    }
}
