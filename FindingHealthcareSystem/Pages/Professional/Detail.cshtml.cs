using BusinessObjects.DTOs.Professional;
using BusinessObjects.DTOs.Service;
using BusinessObjects.Entities;
using FindingHealthcareSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Services;

namespace FindingHealthcareSystem.Pages.Professional
{
    public class DetailModel : BasePageModel
    {
        private readonly IProfessionalService _professionalService;
        public DetailModel (IProfessionalService professionalService)
        {
            _professionalService = professionalService;
        }


        public ProfessionalDto Professional { get; set; }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        public async Task OnGet()
        {
            Professional = await _professionalService.GetById(Id);
        }

    }
}
