using BusinessObjects.DTOs.Department;
using BusinessObjects.DTOs.Facility;
using BusinessObjects.DTOs.Professional;
using BusinessObjects.LocationModels;
using FindingHealthcareSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interfaces;

namespace FindingHealthcareSystem.Pages.Search
{
    public class IndexModel : BasePageModel
    {
        private readonly IFacilityService _facilityService;
        private readonly IProfessionalService _professionalService;
        private readonly ISpecialtyService _specialtyService;
        private readonly IDepartmentService _departmentService;
        private readonly ILocationService _locationService;


        public IndexModel(IFacilityService facilityService, IProfessionalService professionalService,
                         ISpecialtyService specialtyService, IDepartmentService departmentService, 
                         ILocationService locationService)
        {
            _facilityService = facilityService;
            _professionalService = professionalService;
            _specialtyService = specialtyService;
            _departmentService = departmentService;
            _locationService = locationService;
        }

        /// <summary>
        /// LIST OF ITEMS
        /// </summary>
        public IEnumerable<SearchingFacilityDto> Facilities { get; set; } = new List<SearchingFacilityDto>();
        public IEnumerable<ProfessionalDto> Professionals { get; set; } = new List<ProfessionalDto>();
        public IEnumerable<SpecialtyDto> Specialties { get; set; } = new List<SpecialtyDto>();
        public IEnumerable<DepartmentDto> Departments { get; set; } = new List<DepartmentDto>();

        /// <summary>
        /// LOCATION ITEMS
        /// </summary>
        public List<Province> Provinces { get; set; }

        /// <summary>
        /// PROPERTY TO HOLD FORM INPUTS
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchKeyword { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Province { get; set; }

        [BindProperty(SupportsGet = true)]
        public string District { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Ward { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Department { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Specialty { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ProviderType { get; set; } = "facility"; // Default to facility

        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }


        /// <summary>
        /// GET LIST FACILITES/PROFESSIONALS
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            //FETCH SPECIALTIES, DEPARTMENTS FOR SELECT INPUT
            Specialties = await _specialtyService.GetAllSpecialties();
            Departments = await _departmentService.GetAllDepartments();

            //FETCH LOCATIONS FOR SELECT INPUT
            Provinces = await _locationService.GetProvinces();

            await ResolveLocationNames();


            //GET LIST FACILITIES/PROFESSIONALS
            if (ProviderType == "facility" || string.IsNullOrEmpty(ProviderType))
            {
                Facilities = await _facilityService.SearchAsync(SearchKeyword, ProvinceName, DistrictName, WardName, Department);
                Professionals = new List<ProfessionalDto>();
            }
            else if (ProviderType == "professional")
            {
                Professionals = await _professionalService.SearchAsync(ProvinceName, DistrictName, WardName, Specialty, SearchKeyword);
                Facilities = new List<SearchingFacilityDto>();
            }
        }



        /// <summary>
        /// GETTING PRO/DIS/WARD NAME BASED ON THEIR CODES
        /// </summary>
        /// <returns></returns>
        private async Task ResolveLocationNames()
        {
            if (!string.IsNullOrEmpty(Province))
            {
                var province = Provinces.FirstOrDefault(p => p.Code == Province);
                if (province != null)
                {
                    ProvinceName = province.Name;

                    if (!string.IsNullOrEmpty(District))
                    {
                        var districts = await _locationService.GetCities(Province);
                        var district = districts.FirstOrDefault(d => d.Code == District);
                        if (district != null)
                        {
                            DistrictName = district.Name;
                            if (!string.IsNullOrEmpty(Ward))
                            {
                                var wards = await _locationService.GetWards(District);
                                var ward = wards.FirstOrDefault(w => w.Code == Ward);
                                if (ward != null)
                                {
                                    WardName = ward.Name;
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// GETTING DISTRICTS BASED ON PROVINCE
        /// </summary>
        /// <param name="provinceCode"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDistrictsAsync(string provinceCode)
        {
            if (string.IsNullOrEmpty(provinceCode))
            {
                return new JsonResult(new List<District>());
            }

            var districts = await _locationService.GetCities(provinceCode);
            return new JsonResult(districts);
        }

        /// <summary>
        /// GETTING WARDS BASED ON DISTRICT
        /// </summary>
        /// <param name="districtCode"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetWardsAsync(string districtCode)
        {
            if (string.IsNullOrEmpty(districtCode))
            {
                return new JsonResult(new List<Ward>());
            }

            var wards = await _locationService.GetWards(districtCode);
            return new JsonResult(wards);
        }

    }
}
