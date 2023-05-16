using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CI_Platform.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminUserPageRepositoty _adminUserPageRepo;
        private readonly IAdminCMSPageRepository _adminCMSPageRepo;
        private readonly IAdminMissionPageRepository _adminMissionPageRepo;
        private readonly IAdminMissionApplicationsRepository _adminMissionApps;
        private readonly IAdminStoryRepository _adminStories;
        private readonly IAdminSkillRepository _adminSkills;
        private readonly IAdminMissionThemeRepository _adminThemes;
        private readonly IAdminBannerRepository _adminBanners;
        private readonly IAdminVolunteering _volunteerings;
        private readonly IWebHostEnvironment _webHostEnvironment;
       

        public AdminController(IAdminUserPageRepositoty adminUserPageRepo,
            IAdminCMSPageRepository adminCMSPage,
            IWebHostEnvironment webHostEnvironment,
            IAdminMissionPageRepository adminMissionPageRepo,
            IAdminStoryRepository adminStoryRepo,
            IAdminSkillRepository adminSkills,
            IAdminMissionThemeRepository adminThemes,
            IAdminBannerRepository adminBanners,
            IAdminVolunteering volunteerings,
            IAdminMissionApplicationsRepository adminMissionApplications)
        {
            _adminUserPageRepo = adminUserPageRepo;
            _adminCMSPageRepo = adminCMSPage;
            _adminMissionPageRepo = adminMissionPageRepo;
            _webHostEnvironment = webHostEnvironment;
            _adminMissionApps = adminMissionApplications;
            _adminSkills = adminSkills;
            _adminThemes = adminThemes;
            _adminBanners = adminBanners;
            _volunteerings = volunteerings;
            _adminStories = adminStoryRepo;
        }
        /* ------------------------- User ----------------------- */

        public IActionResult User()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                IEnumerable<AdminUserTableViewModel> viewModel = _adminUserPageRepo.UsersList("", 1);
                return View("User/User", viewModel);
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";
                return RedirectToAction("Login", "Auth");
            }
            
        }

        public IActionResult GetUsers(string SearchText,string PageIndex) 
        {
            if(SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminUserTableViewModel> viewModel = _adminUserPageRepo.UsersList(SearchText,Convert.ToInt32(PageIndex));
            //return JsonSerializer.Serialize(viewModel);
            return PartialView("~/Views/Admin/User/UserDetails.cshtml", viewModel);
        }

        [HttpPost]
        public string GetUpdatedUsers(string SearchText, string PageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminUserTableViewModel> viewModel = _adminUserPageRepo.UsersList(SearchText, Convert.ToInt32(PageIndex));
            return JsonSerializer.Serialize(viewModel);
            //return PartialView("~/Views/Admin/User/UserDetails.cshtml", viewModel);
        }

        public IActionResult AddNewUser()
        {
            AdminAddUserViewModel viewModel = new();
            return PartialView("~/Views/Admin/User/AddNewUser.cshtml", viewModel);
        }

        public IActionResult EditUser(long UserId)
        {
            
            AdminAddUserViewModel viewModel = _adminUserPageRepo.getUserDetails(UserId);
            return PartialView("~/Views/Admin/User/AddNewUser.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveUserDetails(AdminAddUserViewModel viewModel)
        {
            if (viewModel.Name == "" ||viewModel.Name!.Length > 16)
            {
                TempData["UserEditError"] = "InValid Name";
                return RedirectToAction("User", "Admin");
            }
            if (viewModel.Surname == "" || viewModel.Surname!.Length > 16)
            {
                TempData["UserEditError"] = "InValid SurName";
                return RedirectToAction("User", "Admin");
            }
            if (viewModel.Email == "" || viewModel.Email!.Length > 128 || viewModel.Email.Length < 8)
            {
                TempData["UserEditError"] = "InValid Email";
                return RedirectToAction("User", "Admin");
            }
            if(viewModel.UserId == "0")
            {
                if (_adminUserPageRepo.ExistUser(viewModel.Email))
                {
                    TempData["UserEditError"] = "Email Already Used";
                    return RedirectToAction("User", "Admin");
                }
            }
            
            if (viewModel.EmployeeId == "" || viewModel.EmployeeId!.Length > 16)
            {
                TempData["UserEditError"] = "Invalid EmployeeId";
                return RedirectToAction("User", "Admin");
            }
            if (viewModel.Department == "" || viewModel.Department!.Length > 16)
            {
                TempData["UserEditError"] = "Invalid department";
                return RedirectToAction("User", "Admin");
            }
            if (viewModel.ProfileText == "")
            {
                TempData["UserEditError"] = "Invalid ProfileText";
                return RedirectToAction("User", "Admin");
            }
            if (viewModel.City == "0")
            {
                TempData["UserEditError"] = "Invalid City";
                return RedirectToAction("User", "Admin");
            }
            if (viewModel.Country == "0")
            {
                TempData["UserEditError"] = "Invalid Country";
                return RedirectToAction("User", "Admin");
            }

            _adminUserPageRepo.SaveUserDetails(viewModel);
            if(viewModel.UserId == "0")
            {
                TempData["UserEditSuccess"] = "User Added Successfully";
            }
            else
            {
                TempData["UserEditSuccess"] = "User Updated Successfully";
            };
            return RedirectToAction("User", "Admin");
        }

        public IActionResult GetLocationDetails(string Email)
        {
            return Json(_adminUserPageRepo.getUserLocation(Email));
        }
        [HttpPost]

        public void DeleteUser(long UserId)
        {
            _adminUserPageRepo.DeleteUser(UserId);
        }

        /* ------------------------- CMS Page ----------------------- */

        public IActionResult CMSPage()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                IEnumerable<AdminCmsPageViewModel> viewModel = _adminCMSPageRepo.getCmsPages("", 1);
                return View("CMS/CMSPage", viewModel);
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";
                return RedirectToAction("Login", "Auth");
            }
            
        }

        public IActionResult GetCmsPages()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                IEnumerable<AdminCmsPageViewModel> viewModel = _adminCMSPageRepo.getCmsPages("", 1);
                return PartialView("~/Views/Admin/CMS/CMSDetails.cshtml", viewModel);
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";
                return RedirectToAction("Login", "Auth");
            }
            
        }

        public string GetUpdatedCms(string SearchText, string PageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminCmsPageViewModel> viewModel = _adminCMSPageRepo.getCmsPages(SearchText, Convert.ToInt32(PageIndex));
            return JsonSerializer.Serialize(viewModel);
        }

        public IActionResult AddNewCmsPage()
        {

            AdminAddCmsViewModel viewModel = new AdminAddCmsViewModel();
            return PartialView("~/Views/Admin/CMS/AddEditCMS.cshtml", viewModel);
        }

        public IActionResult EditCmsPage(long CmsId)
        {
            AdminAddCmsViewModel viewModel = _adminCMSPageRepo.getCmsDetails(CmsId);
            return PartialView("~/Views/Admin/CMS/AddEditCMS.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveCmsDetails(AdminAddCmsViewModel viewModel)
        {
            if(viewModel.Description == null)
            {
                TempData["Description"] = "Description can not be Empty!! Please Try again.";
                return RedirectToAction("CMSPage", "Admin");
            }
            if(viewModel.Id == "0")
            {
                if (_adminCMSPageRepo.IsSlugExist(viewModel.Slug))
                {
                    TempData["Description"] = "Slug is already Used!! Please Try again.";
                    return RedirectToAction("CMSPage", "Admin");
                }
            }
            
            if (ModelState.IsValid)
            {
                _adminCMSPageRepo.SaveCmsPage(viewModel);
                TempData["Success"] = "Succcess";
                return RedirectToAction("CMSPage", "Admin");
            }
            else
            {
                return RedirectToAction("CMSPage", "Admin");
            }
            
        }

        public void DeleteCmsPage(long CmsId)
        {
            _adminCMSPageRepo.DeleteCmsPage(CmsId);
        }

        /* ------------------------- Mission ----------------------- */

        public IActionResult Mission()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                return View("Mission/Mission");
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";
                return RedirectToAction("Login", "Auth");
            }
            
        }

        public IActionResult GetMissions()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                IEnumerable<AdminMissionTableViewModel> viewModel = _adminMissionPageRepo.GetAdminMissions("", 1);
                return PartialView("~/Views/Admin/Mission/MissionDetails.cshtml", viewModel);
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";
                return RedirectToAction("Login", "Auth");
            }
            
        }

        public string GetUpdatedMissions(string SearchText, string PageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminMissionTableViewModel> viewModel = _adminMissionPageRepo.GetAdminMissions(SearchText, Convert.ToInt32(PageIndex));
            return JsonSerializer.Serialize(viewModel);
        }

        public void DeleteMission(long MissionId)
        {
            _adminMissionPageRepo.DeleteMission(MissionId);
        }

        public IActionResult AddNewMission()
        {
            AdminAddEditMissionViewModel viewModel = new AdminAddEditMissionViewModel();
            return PartialView("~/Views/Admin/Mission/AddEditMission.cshtml",viewModel);
        }

        public IActionResult EditMission(long MissionId)
        {
            string x = _webHostEnvironment.WebRootPath;
            AdminAddEditMissionViewModel viewModel = _adminMissionPageRepo.GetMissionDetails(MissionId, x);
            return PartialView("~/Views/Admin/Mission/AddEditMission.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveMissionDetails(AdminAddEditMissionViewModel formData)
        {
            if (formData.Availability == "0" || formData.Skills!.Length <= 0 || formData.StartDate >= formData.EndDate || formData.CityId == "0" || formData.CountryId == "0" || formData.ThemeId == "0" || formData.Description == "")
            {
                TempData["ErrorMessage"] = "Trying to Modify Scripts !!! Please Try Again";
                return View("Mission/Mission");
            }
            if (formData.MissionType == "Go")
            {
                if(formData.GoalObjectiveText == "" || formData.GoalObjectiveText == null || formData.GoalValue <=0)
                {
                    TempData["ErrorMessage"] = "Trying to Modify Scripts !!! Please Try Again";
                    return View("Mission/Mission");
                }
            }
            else
            {
                if(formData.RegistrationDeadline == null || formData.RegistrationDeadline == DateTime.MinValue)
                {
                    TempData["ErrorMessage"] = "Trying to Modify Scripts !!! Please Try Again";
                    return View("Mission/Mission");
                }
            }
            _adminMissionPageRepo.SaveMissionDetails(formData, _webHostEnvironment.WebRootPath);
            if(formData.MissionId == 0)
            {
                TempData["SuccessMessage"] = "Mission Added Successfully";
            }
            else
            {
                TempData["SuccessMessage"] = "Mission Updated Successfully";
            }
            
            return View("Mission/Mission");
        }

        public string GetMissionLoc(string Mid)
        {
            AdminAddEditMissionViewModel viewModel = _adminMissionPageRepo.GetMissionDetails(Convert.ToInt64(Mid), _webHostEnvironment.WebRootPath);
            return JsonSerializer.Serialize(viewModel);
        }
        public IActionResult GetMediaPaths(string Mid)
        {
            return Json(_adminMissionPageRepo.GetMedias(Convert.ToInt64(Mid)));
        }
        public string GetMissionThemes()
        {
            //IEnumerable<MissionTheme> themes = 
            return JsonSerializer.Serialize(_adminMissionPageRepo.getMissionThemes());
        }


        /* ------------------------- Mission Application ----------------------- */

        public IActionResult MissionApplications()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                return View("Mission Application/MissionApplications");
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";
                return RedirectToAction("Login", "Auth");
            }
            
        }

        public IActionResult GetMissionApplications()
         {
            IEnumerable<AdminMissionAppicationTableViewModel> viewModel = _adminMissionApps.GetMissionApplications("",1);
            return PartialView("~/Views/Admin/Mission Application/MissionApplicationDetails.cshtml",viewModel);
        }

        [HttpPost]
        public void UpdateStatus(string AppId, string Status)
        {
            _adminMissionApps.UpdateStatus(Convert.ToInt64(AppId), Status);
        }

        public string GetUpdatedApplications(string SearchText, string PageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminMissionAppicationTableViewModel> viewModel = _adminMissionApps.GetMissionApplications(SearchText, Convert.ToInt32(PageIndex));
            return JsonSerializer.Serialize(viewModel);
        }

        /* ------------------------- Mission Themes --------------------------------- */

        public IActionResult MissionThemes()
        {
            
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                return View("Mission Theme/MissionThemes");
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
                
        }

        public IActionResult GetAdminThemes()
        {
            IEnumerable<AdminMissionThemeViewModel> viewModel = _adminThemes.GetThemes("", 1);
            return PartialView("~/Views/Admin/Mission Theme/MissionThemeDetails.cshtml", viewModel);
        }

        public string GetUpdatedThemes(string SearchText, string PageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminMissionThemeViewModel> viewModel = _adminThemes.GetThemes(SearchText, Convert.ToInt32(PageIndex));
            return JsonSerializer.Serialize(viewModel);
        }

        public IActionResult SaveTheme(string ThemeId, string Title, string Status)
        {

            if (Title == null || Title == "") return Json("Title is Empty");
            string status = _adminThemes.SaveTheme(Convert.ToInt64(ThemeId), Title, Status);
            return Json(status);
        }
        public void DeleteTheme(string ThemeId)
        {
            _adminThemes.DeleteTheme(Convert.ToInt64(ThemeId));

        }

        /* ------------------------- Skills --------------------------------- */

        public IActionResult Skills()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                return View("Skills/Skills");
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
            
        }

        public IActionResult GetSkills()
        {
            IEnumerable<AdminSkillTableViewModel> viewModel = _adminSkills.GetSkills("", 1);
            return PartialView("~/Views/Admin/Skills/SkillDetails.cshtml", viewModel);
        }

        public string getUpdatedSkills(string SearchText, string PageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminSkillTableViewModel> viewModel = _adminSkills.GetSkills(SearchText, Convert.ToInt32(PageIndex));
            return JsonSerializer.Serialize(viewModel);
        }
        public IActionResult SaveSkill(string SkillId, string Name,string Status)
        {

            if (Name == null || Name == "") return Json("Name is Empty");
            string status = _adminSkills.SaveSkill(Convert.ToInt32(SkillId), Name, Status);
            return Json(status);
        }
        [HttpPost]
        public void DeleteSkill(string SkillId)
        {
            _adminSkills.DeleteSkill(Convert.ToInt32(SkillId));
        }

        /* ------------------------- Stories --------------------------------- */

        public IActionResult Stories()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                return View("Stories/Stories");
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
            
        }
        public IActionResult GetStoriesDetails()
        {
            IEnumerable<AdminStoryTableViewModel> viewModel = _adminStories.GetStoriesApplications("", 1);
            return PartialView("~/Views/Admin/Stories/StoriesDetails.cshtml", viewModel);
        }

        public string GetUpdateStories(string SearchText, string pageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminStoryTableViewModel> viewModel = _adminStories.GetStoriesApplications(SearchText, Convert.ToInt32(pageIndex));
            return JsonSerializer.Serialize(viewModel);
        }

        [HttpPost]
        public void UpdateStoryStatus(string StoryId, string Status)
        {
            _adminStories.UpdateStatus(Convert.ToInt64(StoryId), Status);
        }

        /* ------------------------- Banner Management --------------------------------- */

        public IActionResult BannerManagement()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                return View("Banner/BannerManagement");
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }

        }

        public IActionResult GetBannerDetails()
        {
            IEnumerable<AdminBannerViewModel> viewModel = _adminBanners.GetBannerDetails("", 1);
            return PartialView("~/Views/Admin/Banner/BannerDetails.cshtml", viewModel);
        }

        public string GetUpdatedBannerDetails(string SearchText, string PageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminBannerViewModel> viewModel = _adminBanners.GetBannerDetails(SearchText, Convert.ToInt32(PageIndex));
            return JsonSerializer.Serialize(viewModel);
        }
        public string GetBanner(long BannerId)
        {
            return JsonSerializer.Serialize(_adminBanners.GetBanner(BannerId));
        }

        [HttpPost]
        public string SaveBannerDetails(AdminAddEditBannerViewModel Model)
        {
            if (Model.Title!.Trim() == "") return "Enter Valid Title";

            if (Model.SortOrder == "") Model.SortOrder = "0";

            if (Model.Text!.Trim() == "") return "Enter Valid Text";

            if (Model.Image == null) return "Please Upload Image";
            string result =_adminBanners.SaveBannerDetails(Model,_webHostEnvironment.WebRootPath);
            return result;
        }

        [HttpPost]
        public void DeleteBanner(string BannerId)
        {
            _adminBanners.DeleteBanner(Convert.ToInt64(BannerId),_webHostEnvironment.WebRootPath);
        }

        /* ------------------------- Volunteering Management --------------------------------- */

        public IActionResult Volunteering()
        {
            if (HttpContext.Session.GetString("Role") == "Admin" && HttpContext.Session.GetString("UserId") != "")
            {
                return View("Volunteering/Volunteering");
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
        }

        public IActionResult GetVolunteeringDetails()
        {
            IEnumerable<AdminTimesheetViewModel> viewModel = _volunteerings.GetVolunteeringDetails("", 1);
            return PartialView("~/Views/Admin/Volunteering/VolunteeringDetailsTable.cshtml", viewModel);
        }

        public string GetUpdatedVolunteerings(string SearchText, string PageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            IEnumerable<AdminTimesheetViewModel> viewModel = _volunteerings.GetVolunteeringDetails(SearchText, Convert.ToInt32(PageIndex));

            return JsonSerializer.Serialize(viewModel);
        }
        public void UpdateVulunteeringStatus(string TimesheetId, string Status)
        {
            _volunteerings.UpdateStatus(Convert.ToInt64(TimesheetId), Status);
        }
    }
}
