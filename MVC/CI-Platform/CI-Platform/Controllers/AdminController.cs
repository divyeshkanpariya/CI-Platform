using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class AdminController : Controller
    {
        private IAdminUserPageRepositoty _adminUserPageRepo;

        public AdminController(IAdminUserPageRepositoty adminUserPageRepo)
        {
            _adminUserPageRepo = adminUserPageRepo;
        }
        public IActionResult User()
        {
            IEnumerable<AdminUserTableViewModel> viewModel = _adminUserPageRepo.UsersList();
            return View(viewModel);
        }
    }
}
