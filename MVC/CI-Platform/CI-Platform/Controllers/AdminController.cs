using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CI_Platform.Controllers
{
    public class AdminController : Controller
    {
        private IAdminUserPageRepositoty _adminUserPageRepo;
        private IAdminCMSPageRepository _adminCMSPageRepo;

        public AdminController(IAdminUserPageRepositoty adminUserPageRepo,
            IAdminCMSPageRepository adminCMSPage)
        {
            _adminUserPageRepo = adminUserPageRepo;
            _adminCMSPageRepo = adminCMSPage;
        }
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
            return RedirectToAction("User", "Admin");
        }


        public IActionResult CMSPage()
        {
            IEnumerable<AdminCmsPageViewModel> viewModel = _adminCMSPageRepo.getCmsPages("", 1);

            return View("CMS/CMSPage",viewModel);
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
    }   
}
