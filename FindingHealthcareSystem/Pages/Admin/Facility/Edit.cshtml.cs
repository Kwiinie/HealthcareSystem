using BusinessObjects.DTOs.Facility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Admin.Facility
{
    public class EditModel : PageModel
    {
        private readonly IFacilityService _facilityService;
        private readonly IFacilityTypeService _facilityTypeService;

        public EditModel(IFacilityService facilityService, IFacilityTypeService facilityTypeService)
        {
            _facilityService = facilityService;
            _facilityTypeService = facilityTypeService;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public FacilityDto Facility { get; set; }

        public List<FacilityTypeDto> FacilityTypes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Facility = await _facilityService.GetById(Id);
            if (Facility == null)
            {
                return NotFound();
            }

            FacilityTypes = await _facilityTypeService.GetAllFacilityTypes();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                FacilityTypes = await _facilityTypeService.GetAllFacilityTypes();
                return Page();
            }

            await _facilityService.Update(Id, Facility);
            return RedirectToPage("Index");
        }
    }
}
