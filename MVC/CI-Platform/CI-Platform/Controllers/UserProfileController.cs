using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CI_Platform.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepo;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepo = userProfileRepository;
        }

        public IActionResult UserEditProfile()
        {
            //UserProfileViewModel model = new UserProfileViewModel();
            return View();
        }

        [HttpPost]
        public string GetCities(string CountryId)
        {
            var CId = Convert.ToInt64(CountryId);
            var cities =  _userProfileRepo.GetAllCities(CId);
            return JsonSerializer.Serialize(cities);
        }
        public string GetCountries()
        {
            var countries = _userProfileRepo.GetAllCountries();
            return JsonSerializer.Serialize(countries);
        }
        [HttpPost]
        public IActionResult UserEditProfile(IFormCollection formData)
        {
            if(formData == null)
            {
                return View();
            }
            if(formData["Country"] == "0")
            {
                ModelState.AddModelError("Name", "Name is Required");
                return View();
            }
            return View();
        }
    }
}
