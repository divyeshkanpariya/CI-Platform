using CI_Platform.Models;
using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly CiPlatformContext _db;
        public HomeController(CiPlatformContext db)
        {
            _db = db;
        }
        public IActionResult MissionListing(MissionListingViewModel data)
        {
               
            return View(data);
        }
        public IActionResult VolunteeringMission()
        {
            return View();
        }
        public IActionResult StoryListing()
        {
            return View();
        }
    }
}