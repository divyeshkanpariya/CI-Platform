﻿using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CI_Platform.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public UserProfileController(IUserProfileRepository userProfileRepository,IWebHostEnvironment webHostEnvironment)
        {
            _userProfileRepo = userProfileRepository;
            _WebHostEnvironment = webHostEnvironment;

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
        public string GetSkills()
        {
            var skillls = _userProfileRepo.GetAllSkills();
            return JsonSerializer.Serialize(skillls);
        }
        [HttpPost]
        public IActionResult UserEditProfile(IFormCollection formData)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            if (UserId != null)
            {
                if (formData == null)
                {
                    return View();
                }
                if (formData["Country"] == "0")
                {
                    ModelState.AddModelError("Country", "Please Select Country");
                    return View();
                }
                if (formData["City"] == "0")
                {
                    ModelState.AddModelError("City", "Please Select Country");
                    return View();
                }


                UserProfileViewModel userProfileViewModel = new UserProfileViewModel();
                string path = "";
                if (formData.Files.Count != 0)
                {
                    userProfileViewModel.ProfileImage = formData.Files[0];

                    var file = formData.Files[0];
                    string folder = "Uploads/ProfilePhotos/";
                    string ext = file.ContentType.ToLower().Substring(file.ContentType.LastIndexOf("/") + 1);
                    folder += Convert.ToString(UserId) + "-" + Guid.NewGuid().ToString() + "." + ext;
                    HttpContext.Session.SetString("UserAvatar", "/" + folder);
                    string serverFolder = Path.Combine(_WebHostEnvironment.WebRootPath, folder);
                    file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    path += "/" + folder;
                }
                userProfileViewModel.Name = formData["Name"];
                userProfileViewModel.Surname = formData["Surname"];
                userProfileViewModel.MyProfileText = formData["MyProfileText"];

                if (formData["Country"] != "0") userProfileViewModel.Country = formData["Country"];
                if (formData["EmployeeId"] != "") userProfileViewModel.EmployeeId = formData["EmployeeId"];
                if (formData["Manager"] != "") userProfileViewModel.Manager = formData["Manager"];
                if (formData["Title"] != "") userProfileViewModel.Title = formData["Title"];
                if (formData["Department"] != "") userProfileViewModel.Department = formData["Department"];
                if (formData["WhyIVol"] != "") userProfileViewModel.WhyIVol = formData["WhyIVol"];
                if (formData["LinkedinURL"] != "") userProfileViewModel.LinkedinURL = formData["LinkedinURL"];
                if (formData["City"] != "0") userProfileViewModel.City = formData["City"];
                if (formData["Availability"] != "Select Your Availability") userProfileViewModel.Availability = formData["Availability"];
                if (formData["MySkills"] != "") userProfileViewModel.MySkills = formData["MySkills"];

                _userProfileRepo.AddUserData(userProfileViewModel, UserId, path);
                return View();
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
            return View();
        }

        public string GetUserData()
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            
            IEnumerable<UserProfileViewModel> data = _userProfileRepo.GetUserData(UserId);

            return JsonSerializer.Serialize(data);
        }

        [HttpPost]
        public IActionResult ChangePassword(string OldPwd,string NewPwd, string ConfirmPwd)
        {
            
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            string result = _userProfileRepo.ChangePassword(UserId, OldPwd, NewPwd);
            List<string> arr = new List<string> { result };
            return Json(arr);
        }
    }
}