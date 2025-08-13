using BusinessObjects.Commons;
using BusinessObjects.Dtos.User;
using BusinessObjects.DTOs.Department;
using BusinessObjects.DTOs.Facility;
using BusinessObjects.LocationModels;
using FindingHealthcareSystem.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;
using Services.Services;

namespace FindingHealthcareSystem.Pages
{
    public class IndexModel : BasePageModel
    {
        private readonly ILocationService _locationService;
        private readonly IDepartmentService _departmentService;
        private readonly IFacilityTypeService _facilityTypeService;

        public List<Province> Provinces { get; set; }
        public List<DepartmentDto> Departments { get; set; }
        public List<FacilityTypeDto> FacilityTypes { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SelectedProvinceCode { get; set; }


        public IndexModel(ILocationService locationService, IDepartmentService departmentService, IFacilityTypeService facilityTypeService, IHttpContextAccessor httpContextAccessor)
        {
            _locationService = locationService;
            _departmentService = departmentService;
            _facilityTypeService = facilityTypeService;
        }


        public async Task OnGetAsync()
        {
            //Provinces = await _locationService.GetProvinces(); 
            Departments = await _departmentService.GetAllDepartments();
            FacilityTypes = await _facilityTypeService.GetAllFacilityTypes();

        }

        public async Task OnPostAsync()
        {
            Provinces = await _locationService.GetProvinces();

        }
    }

}
