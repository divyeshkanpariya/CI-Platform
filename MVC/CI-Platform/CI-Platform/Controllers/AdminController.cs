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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IAdminUserPageRepositoty adminUserPageRepo,
            IAdminCMSPageRepository adminCMSPage,
            IWebHostEnvironment webHostEnvironment,
            IAdminMissionPageRepository adminMissionPageRepo,
            IAdminStoryRepository adminStoryRepo,
            IAdminSkillRepository adminSkills,
            IAdminMissionApplicationsRepository adminMissionApplications)
        {
            _adminUserPageRepo = adminUserPageRepo;
            _adminCMSPageRepo = adminCMSPage;
            _adminMissionPageRepo = adminMissionPageRepo;
            _webHostEnvironment = webHostEnvironment;
            _adminMissionApps = adminMissionApplications;
            _adminSkills = adminSkills;
            _adminStories = adminStoryRepo;
        }
        /* ------------------------- User ----------------------- */

        public IActionResult User()
        {
            IEnumerable<AdminUserTableViewModel> viewModel = _adminUserPageRepo.UsersList("",1);
            return View("User/User",viewModel);
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
            AdminAddUserViewModel viewModel = new AdminAddUserViewModel();
            return PartialView("~/Views/Admin/User/AddNewUser.cshtml", viewModel);
        }

        public IActionResult EditUser(long UserId)
        {
            
            AdminAddUserViewModel viewModel = _adminUserPageRepo.getUserDetails(UserId);
            return PartialView("~/Views/Admin/User/AddNewUser.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult SaveUserDetails(AdminAddUserViewModel viewModel)
        {
            _adminUserPageRepo.SaveUserDetails(viewModel);
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
            IEnumerable<AdminCmsPageViewModel> viewModel = _adminCMSPageRepo.getCmsPages("", 1);

            return View("CMS/CMSPage",viewModel);
        }

        public IActionResult GetCmsPages()
        {

            IEnumerable<AdminCmsPageViewModel> viewModel = _adminCMSPageRepo.getCmsPages("", 1);
            return PartialView("~/Views/Admin/CMS/CMSDetails.cshtml", viewModel);
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
                TempData["Description"] = "Description can not be Empty";
                return RedirectToAction("CMSPage", "Admin");
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
            return View("Mission/Mission");
        }
        public IActionResult GetMissions()
        {
            IEnumerable <AdminMissionTableViewModel> viewModel= _adminMissionPageRepo.GetAdminMissions("", 1);
            return PartialView("~/Views/Admin/Mission/MissionDetails.cshtml", viewModel);
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
            if(formData.Availability == "0") return View("Mission/Mission");
            string x = _webHostEnvironment.WebRootPath;
            _adminMissionPageRepo.SaveMissionDetails(formData, _webHostEnvironment.WebRootPath);
            return View("Mission/Mission");
        }
        public string GetMissionLoc(string Mid)
        {
            AdminAddEditMissionViewModel viewModel = _adminMissionPageRepo.GetMissionDetails(Convert.ToInt64(Mid), _webHostEnvironment.WebRootPath);
            List<string> result = _adminMissionPageRepo.GetMissionLoc(Convert.ToInt64(Mid));
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
            return View("Mission Application/MissionApplications");
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

        public IActionResult MissionThemes()
        {
            return View("Mission Theme/MissionThemes");
        }
        /* ------------------------- Skills --------------------------------- */
        public IActionResult Skills()
        {
            return View("Skills/Skills");
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
        public IActionResult Stories()
        {
            //IEnumerable<AdminStoryTableViewModel> viewModel = _adminStories.GetStoriesApplications("", 1);
            return View("Stories/Stories");
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
    }
}
