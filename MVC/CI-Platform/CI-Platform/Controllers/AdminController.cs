using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult User()
        {
            return View();
        }
    }
}
