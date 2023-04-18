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

        public AdminController(IAdminUserPageRepositoty adminUserPageRepo,
            IAdminCMSPageRepository adminCMSPage,
            IAdminMissionPageRepository adminMissionPageRepo)
        {
            _adminUserPageRepo = adminUserPageRepo;
            _adminCMSPageRepo = adminCMSPage;
            _adminMissionPageRepo = adminMissionPageRepo;
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

        }
    }
}
