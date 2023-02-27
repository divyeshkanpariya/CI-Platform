using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class AuthController : Controller
    {
        private readonly CiPlatformContext  _db;
        public AuthController(CiPlatformContext db)
        {
            _db = db;
        }
        public IActionResult Login()
        {

            return View();

        }
        [HttpPost]
        public IActionResult Login(LoginViewModel data)
        {
            if (ModelState.IsValid)
            {
                var userFromDB = _db.Users.Any(u => u.Email == data.Email && u.Password == data.Password);
                if (userFromDB)
                {
                    return RedirectToAction("home", "MissionListing");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email or Password");
                }
                
            }
            return View(data);
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel data)
        {
            if(ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName= data.FirstName,
                    LastName= data.LastName,
                    Email= data.Email,
                    PhoneNumber= data.PhoneNumber,
                    Password= data.Password,
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(data);
                
        }
        public IActionResult Lost_Password()
        {
            return View();
        }
        public IActionResult Reset_Password()
        {
            return View();
        }
    }
}
