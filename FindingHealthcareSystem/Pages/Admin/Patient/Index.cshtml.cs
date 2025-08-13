using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace FindingHealthcareSystem.Pages.Admin.Patient
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        // Original patient list - will hold all patients
        public IEnumerable<PatientDTO> AllPatients { get; set; }

        // Paginated and filtered patients to display
        public IEnumerable<PatientDTO> Patients { get; set; }

        // Pagination properties
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public int TotalPatients { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        // Sorting and filtering properties
        public string CurrentSort { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string GenderSort { get; set; }
        public string StatusSort { get; set; }
        public string CurrentFilter { get; set; }
        public string StatusFilter { get; set; }
        public string GenderFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString,
                                   string status, string gender, int? pageIndex)
        {
            // Set up sorting
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "date" ? "date_desc" : "date";
            GenderSort = sortOrder == "gender" ? "gender_desc" : "gender";
            StatusSort = sortOrder == "status" ? "status_desc" : "status";

            // Handle search string and pagination reset
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            // Set current filter values
            CurrentFilter = searchString;
            StatusFilter = status;
            GenderFilter = gender;
            PageIndex = pageIndex ?? 1;

            // Get all patients first
            AllPatients = await _userService.GetAllPatientAsync();
            var filteredPatients = AllPatients;

            // Apply filtering
            bool hasFiltersOrSearch = false;

            // Apply search filter for fullname, phone number, or email (approximate search)
            if (!string.IsNullOrEmpty(searchString))
            {
                hasFiltersOrSearch = true;
                searchString = searchString.ToLower();
                filteredPatients = filteredPatients.Where(p =>
                    (p.User.Fullname != null && p.User.Fullname.ToLower().Contains(searchString)) ||
                    (p.User.PhoneNumber != null && p.User.PhoneNumber.ToLower().Contains(searchString)) ||
                    (p.User.Email != null && p.User.Email.ToLower().Contains(searchString)));
            }

            // Apply status filter
            if (!string.IsNullOrEmpty(status))
            {
                hasFiltersOrSearch = true;
                filteredPatients = filteredPatients.Where(p => p.User.Status == status);
            }

            // Apply gender filter
            if (!string.IsNullOrEmpty(gender))
            {
                hasFiltersOrSearch = true;
                filteredPatients = filteredPatients.Where(p => p.User.Gender == gender);
            }

            // Apply sorting only if specified or if we have filters/search applied
            if (!string.IsNullOrEmpty(sortOrder) || hasFiltersOrSearch)
            {
                filteredPatients = sortOrder switch
                {
                    "name_desc" => filteredPatients.OrderByDescending(p => p.User.Fullname),
                    "date" => filteredPatients.OrderBy(p => p.User.Birthday),
                    "date_desc" => filteredPatients.OrderByDescending(p => p.User.Birthday),
                    "gender" => filteredPatients.OrderBy(p => p.User.Gender),
                    "gender_desc" => filteredPatients.OrderByDescending(p => p.User.Gender),
                    "status" => filteredPatients.OrderBy(p => p.User.Status),
                    "status_desc" => filteredPatients.OrderByDescending(p => p.User.Status),
                    _ => hasFiltersOrSearch ? filteredPatients.OrderBy(p => p.User.Fullname) : filteredPatients
                };
            }

            // Calculate total and pagination
            TotalPatients = filteredPatients.Count();
            TotalPages = (int)Math.Ceiling(TotalPatients / (double)PageSize);

            // Create paginated result
            Patients = filteredPatients
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize);
        }



        public async Task<IActionResult> OnPostApproveAsync(int professionalId)
        {
            return await ProcessRequest(professionalId);
        }



        private async Task<IActionResult> ProcessRequest(int userid)
        {


            try
            {


                var user = await _userService.GetUserByIdAsync(userid);
                user.Status = UserStatus.Inactive.ToString();

                await _userService.UpdateUserStatus(user);

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi xử lý yêu cầu: {ex.Message}");
                return Page();
            }
        }
    }
}
