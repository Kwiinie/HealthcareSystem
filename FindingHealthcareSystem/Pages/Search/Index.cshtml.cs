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
        private readonly IProfessionalService _professionalService;
        private readonly ISpecialtyService _specialtyService;
        private readonly ILocationService _locationService;

        public IndexModel(IProfessionalService professionalService,
                         ISpecialtyService specialtyService,
                         ILocationService locationService)
        {
            _professionalService = professionalService;
            _specialtyService = specialtyService;
            _locationService = locationService;
        }

        /// <summary>
        /// LIST OF ITEMS
        /// </summary>
        public IEnumerable<ProfessionalDto> Professionals { get; set; } = new List<ProfessionalDto>();
        public IEnumerable<SpecialtyDto> Specialties { get; set; } = new List<SpecialtyDto>();

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
        public string Specialty { get; set; }

        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }

        /// <summary>
        /// GET LIST PROFESSIONALS
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            //FETCH SPECIALTIES FOR SELECT INPUT
            Specialties = await _specialtyService.GetAllSpecialties();

            //FETCH LOCATIONS FOR SELECT INPUT
            Provinces = await _locationService.GetProvinces();

            await ResolveLocationNames();

            //GET LIST PROFESSIONALS
            Professionals = await _professionalService.SearchAsync(ProvinceName, DistrictName, WardName, Specialty, SearchKeyword);
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