using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CI_Platform.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public readonly IPrivacyPolicyRepository _privacyPolicyRepo;
        public readonly IVolunteeringTimesheet _volunteeringTimesheetRepo;

        public UserProfileController(IUserProfileRepository userProfileRepository,
            IWebHostEnvironment webHostEnvironment,
            IPrivacyPolicyRepository privacyPolicyRepository,
            IVolunteeringTimesheet volunteeringTimesheet)
        {
            _userProfileRepo = userProfileRepository;
            _WebHostEnvironment = webHostEnvironment;
            _privacyPolicyRepo = privacyPolicyRepository;
            _volunteeringTimesheetRepo = volunteeringTimesheet;
        }

        public IActionResult UserEditProfile()
        {
            //UserProfileViewModel model = new UserProfileViewModel();
            if (HttpContext.Session.GetString("Role") == "User" && HttpContext.Session.GetString("UserId") != "")
            {
                return View();
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
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
            if (HttpContext.Session.GetString("UserId") != null)
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
                if (formData["MySkills"] == "")
                {
                    ModelState.AddModelError("MySkills", "Please Select Skills");
                    return View();
                }

                UserProfileViewModel userProfileViewModel = new UserProfileViewModel();
                string path = "";
                if (formData.Files.Count != 0)
                {
                    if (formData.Files[0].ContentType.Substring(0,5) != "image")
                    {
                        ModelState.AddModelError("ProfileImage", "Profile Photo must be Image");
                        return View();
                    }
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
                userProfileViewModel.Name = formData["Name"]!;
                userProfileViewModel.Surname = formData["Surname"]!;
                userProfileViewModel.MyProfileText = formData["MyProfileText"]!;

                if (formData["Country"] != "0") userProfileViewModel.Country = formData["Country"]!;
                if (formData["EmployeeId"] != "") userProfileViewModel.EmployeeId = formData["EmployeeId"]!;
                if (formData["Manager"] != "") userProfileViewModel.Manager = formData["Manager"]!;
                if (formData["Title"] != "") userProfileViewModel.Title = formData["Title"];
                if (formData["Department"] != "") userProfileViewModel.Department = formData["Department"];
                if (formData["WhyIVol"] != "") userProfileViewModel.WhyIVol = formData["WhyIVol"];
                if (formData["LinkedinURL"] != "") userProfileViewModel.LinkedinURL = formData["LinkedinURL"];
                if (formData["City"] != "0") userProfileViewModel.City = formData["City"]!;
                if (formData["Availability"] != "Select Your Availability") userProfileViewModel.Availability = formData["Availability"]!;
                if (formData["MySkills"] != "") userProfileViewModel.MySkills = formData["MySkills"]!;

                _userProfileRepo.AddUserData(userProfileViewModel, UserId, path);
                TempData["SuccessMessage"] = "Profile Updated Successfully";
                return View();
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
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
            List<string> arr = new List<string>();
            if (OldPwd == "" || OldPwd == null)
            {
                arr.Add("Please Enter Old Password");
                return Json(arr);
            }
            if(NewPwd == "" || NewPwd == null)
            {
                arr.Add("Please Enter New Password");
                return Json(arr);
            }
            if(OldPwd == NewPwd)
            {
                arr.Add("Old Password and New Password Should not Same");
                return Json(arr);
            }
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            string result = _userProfileRepo.ChangePassword(UserId, OldPwd, NewPwd);
            arr.Add(result);
            return Json(arr);
        }

        public IActionResult VolunteeringTimesheet()
        {
            //long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            if(HttpContext.Session.GetString("Role") == "User" && HttpContext.Session.GetString("UserId") != "")
            {
                return View();
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";
                return RedirectToAction("Login", "Auth");
            }
            
        }

        public IActionResult GetMissions()
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            List<List<string>> list = _volunteeringTimesheetRepo.Missions(UserId);
            return Json(list);
        }

        public IActionResult getVolunteeringdetails()
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            return Json(_volunteeringTimesheetRepo.getVolDetailsByUser(UserId));
        }

        [HttpPost]
        public void SaveVolunteeringDetails(string Type,string MissionId,string Date, string Hours, string Minutes, string Action, string Message, string Status)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            _volunteeringTimesheetRepo.SaveVolDetails(Type,UserId, Convert.ToInt64(MissionId), Convert.ToDateTime(Date), Convert.ToInt32(Hours), Convert.ToInt32(Minutes), Convert.ToInt32(Action), Message,Status);
        }
        [HttpPost]
        public void DeleteVolField(string TimesheetId)
        {
            _volunteeringTimesheetRepo.DeleteVol(Convert.ToInt64(TimesheetId));
        }
    }

}
