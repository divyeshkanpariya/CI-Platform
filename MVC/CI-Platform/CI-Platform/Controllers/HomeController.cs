using CI_Platform.Models;
using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using CI_Platform.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CI_Platform.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IMissionListingRepository _missionListingDb;
        private readonly IMissionCardRepository _MissionCard;
        private readonly IVolunteeringMissionRepository _VolunteeringMission;
        private readonly IFavouriteMission _FavouriteMissions;
        private readonly IRepository<City> _Cities;
        private readonly IStoryListingRepository _StoriyListingDb;
        private readonly IStoryCardRepository _StoryCard;
        private readonly IShareStoryRepository _ShareStory;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IStoryDetailsRepository _StoryDetails;
        private readonly IRepository<Story> _StoryList;
        private readonly IRepository<Mission> _Missions;
        public HomeController(
            IMissionListingRepository missionListingDb,
            IMissionCardRepository MissionCard,
            IRepository<City> Cities,
            IVolunteeringMissionRepository volunteeringMission,
            IFavouriteMission FavouriteMissions,
            IStoryListingRepository StoryListingDb,
            IStoryCardRepository StoryCards,
            IShareStoryRepository ShareStory,
            IWebHostEnvironment webHostEnvironment,
            IStoryDetailsRepository StoryDetails,
            IRepository<Story> StoryList,
            IRepository<Mission> Missions)
        {
            
            _missionListingDb = missionListingDb;
            _MissionCard = MissionCard;
            _Cities = Cities;
            _VolunteeringMission = volunteeringMission;
            _FavouriteMissions = FavouriteMissions;
            _StoriyListingDb = StoryListingDb;
            _StoryCard = StoryCards;
            _ShareStory = ShareStory;
            _WebHostEnvironment = webHostEnvironment;
            _StoryDetails = StoryDetails;
            _StoryList = StoryList;
            _Missions = Missions;
        }
        public IActionResult MissionListing()
        {
            if(HttpContext.Session.GetString("UserName") != null)
            {
                string CountryIDs = "";
                string CityIDs = "";
                string ThemeIDs = "";
                string SkillIDs = "";
                string SortBy = "1";
                string SearchText = "";
                string PageIndex = "";
                string UserId = HttpContext.Session.GetString("UserId");
                var data = _missionListingDb.GetAllData(CountryIDs, CityIDs, ThemeIDs, SkillIDs, SortBy,SearchText,UserId,PageIndex);
                return View(data);
            }else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
            
        }
       
        [HttpPost]
        public IActionResult GetCards(string CountryIDs,string CityIDs,string ThemeIDs,string SkillIDs,string SortBy,string SearchText,string PageIndex)
        {
            if(SearchText == null)
            {
                SearchText = "";
            }
            if(CityIDs == null)
            {
                var citys = _Cities.GetAll();
                string citystr = "";
                foreach(var city in citys)
                {
                    citystr += city.CityId.ToString();
                    citystr += ",";
                }
                citystr = citystr.Substring(0, citystr.Length - 2);
                CityIDs = citystr;
            }
            string UserId = HttpContext.Session.GetString("UserId");
            var data = _missionListingDb.GetAllData(CountryIDs, CityIDs, ThemeIDs, SkillIDs, SortBy,SearchText,UserId,PageIndex);
            
            return PartialView("Cards", data);
        }
        public IActionResult AddToFavourite(long MissionId)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            _FavouriteMissions.AddToFavourite(MissionId,UserId);
            return Json(MissionId);
        }

        public IActionResult getAllCities()
        {
            IEnumerable<City> cities = _Cities.GetAll();
            Console.WriteLine(cities);
            return Json(cities);
        }

        public IActionResult getCityByCountry(long countryId)
        {
            var cities = _missionListingDb.getCityByCountry(countryId);
            return Json(cities);
        }


        public IActionResult VolunteeringMission(long mid)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                MissionListingViewModel data = _VolunteeringMission.GetAllMissionData(mid,userId);
                return View(data);
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
            
        }
        public IActionResult RelatedMissions(string MissionId)
        {
            long userId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            long Mid = Convert.ToInt64(MissionId);
            MissionListingViewModel relatedMissions = _VolunteeringMission.GetRelatedMissions(Mid,userId);
            return PartialView("RelatedMissions",relatedMissions);
        }
        public void SendInvitation(long EmailTo,long MissionId)
        {
            long senderId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            string senderName = HttpContext.Session.GetString("UserName");
            string url = Url.ActionLink("VolunteeringMission", "Home", new { mid = MissionId });
            var MissionTitle = _Missions.GetFirstOrDefault(u => u.MissionId == MissionId).Title;
            string body = "Greetings of the Day !! <br><br>        " + senderName + " Invited you to join '" + MissionTitle + "' Mission. <br><br>" + url;

            _VolunteeringMission.SendInvitation(EmailTo, senderId, MissionId, body);

            _VolunteeringMission.addInvitation(senderId, EmailTo, MissionId);
        }

        public void RateMission(int Rating,string MissionId)
        {
            long senderId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            long mid = Convert.ToInt64(MissionId);
            _VolunteeringMission.RateMission(Rating, mid, senderId);
        }

        public void PostComment(string MissionId,string CommentText)
        {
            long Mid = Convert.ToInt64(MissionId);
            long uid = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            _VolunteeringMission.PostComment(Mid,CommentText,uid);
        }
        public void ApplyMission(string MissionId)
        {
            long Mid = Convert.ToInt64(MissionId);
            long uid = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            _VolunteeringMission.ApplyMission(Mid, uid);
        }

        public IActionResult StoryListing()
        {
            string CountryIDs = "";
            string CityIDs = "";
            string ThemeIDs = "";
            string SkillIDs = "";
            string SortBy = "1";
            string SearchText = "";
            string PageIndex = "";
            string UserId = HttpContext.Session.GetString("UserId");
            var data = _StoriyListingDb.GetAllData(CountryIDs, CityIDs, ThemeIDs, SkillIDs, SearchText, UserId, PageIndex);
            return View(data);
            
        }
        [HttpPost]
        public IActionResult GetStoryCards(string CountryIDs, string CityIDs, string ThemeIDs, string SkillIDs, string SearchText, string PageIndex)
        {
            if (SearchText == null)
            {
                SearchText = "";
            }
            if (CityIDs == null)
            {
                var citys = _Cities.GetAll();
                string citystr = "";
                foreach (var city in citys)
                {
                    citystr += city.CityId.ToString();
                    citystr += ",";
                }
                citystr = citystr.Substring(0, citystr.Length - 2);
                CityIDs = citystr;
            }
            string UserId = HttpContext.Session.GetString("UserId");
            var data = _StoriyListingDb.GetAllData(CountryIDs, CityIDs, ThemeIDs, SkillIDs, SearchText, UserId, PageIndex);

            return PartialView("StoryCards", data);
        }
        public IActionResult ShareYourStory()
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            ShareYourStoryViewModel shareYourStoryViewModel = new ShareYourStoryViewModel();
            if (_ShareStory.GetMissions(UserId) != null)
            {
                return View(shareYourStoryViewModel);
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
        }
        [HttpGet]
        public IActionResult GetMissionsOfUser()
      {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            
            List<List<string>> recentUsers = _ShareStory.GetMissions(UserId);
            
            return Json(recentUsers);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShareYourStory(IFormCollection formData)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            
            ShareYourStoryViewModel viewModel = new ShareYourStoryViewModel()
            {
                Mission = formData["Mission"],
                Date = Convert.ToDateTime(formData["Date"]),
                StoryTitle = formData["StoryTitle"],
                StoryDescription = formData["StoryDescription"],
                StoryVideoUrl = formData["StoryVideoUrl"],
                Photos = formData.Files,
            };

            if (ModelState.IsValid)
            {

                UpdateMedia(viewModel, "PENDING");
                return RedirectToAction("StoryListing");

            }
            else
            {
                return View();
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult SaveStoryDraft(IFormCollection formData)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            ShareYourStoryViewModel viewModel = new ShareYourStoryViewModel()
            {
                Mission = formData["Mission"],
                Date = Convert.ToDateTime(formData["Date"]),
                StoryTitle = formData["StoryTitle"],
                StoryDescription = formData["StoryDescription"],
                StoryVideoUrl = formData["StoryVideoUrl"],
                Photos = formData.Files,
            };

            if (ModelState.IsValid)
            {
                UpdateMedia(viewModel,"DRAFT");

                return RedirectToAction("StoryListing");

            }
            else
            {
                return View();
            }
            return View();
        }
        [HttpPost]
        public IActionResult GetStoryDetails(string MissionId)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            List<List<string>> list = _ShareStory.GetStoryDetails(Convert.ToInt64(MissionId), UserId);
            return Json(list);
        }
        public void UpdateMedia(ShareYourStoryViewModel viewModel, string status)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            long StoryId = _ShareStory.UploadStory(viewModel, UserId, status);
            if (_StoryList.ExistUser(u => u.MissionId == Convert.ToInt64(viewModel.Mission) && u.UserId == UserId))
            {
                _ShareStory.DeleteMedia(StoryId);
            }
            foreach (var file in viewModel.Photos)
            {
                string folder = "Uploads/Story/";
                string ext = file.ContentType.ToLower().Substring(file.ContentType.LastIndexOf("/") + 1);
                folder += Convert.ToString(StoryId) + "-" + Convert.ToString(UserId) + "-" + Guid.NewGuid().ToString() + "." + ext;
                string serverFolder = Path.Combine(_WebHostEnvironment.WebRootPath, folder);
                file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                string path = "/" + folder;
                string FileType = Convert.ToString(file.ContentType);
                _ShareStory.UploadMedia(StoryId, FileType, path);
            }

            string[] Urls = viewModel.StoryVideoUrl.Split('\r');
            int x = 0;
            foreach (string Url in Urls)
            {
                string finUrl;
                if (x++ != 0)
                {
                    finUrl = Url.Remove(0, 1);
                }
                else
                {
                    finUrl = Url;
                }
                if(finUrl != "")
                {
                    _ShareStory.UploadMedia(StoryId, "URL", finUrl);

                }
            }
        }

        public IActionResult StoryDetails(string sid)
        {
            if (HttpContext.Session.GetString("UserId") != null)
            {
                long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                long StoryId = Convert.ToInt64(sid);
                _StoryDetails.addStoryView(UserId, StoryId);
                IEnumerable<StoryCardViewModel> storyCard = _StoryDetails.GetAllData(StoryId);
                return View(storyCard);
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
            
        }
        [HttpPost]
        public IActionResult SendStoryInvite(string SendTo, string StoryId)
        {
            long EmailTo = Convert.ToInt64(SendTo);
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            long Sid = Convert.ToInt64(StoryId);
            string UserName = HttpContext.Session.GetString("UserName");
            long Mid = _StoryList.GetFirstOrDefault(u => u.StoryId == Sid).MissionId;
            string MissionTitle = _Missions.GetFirstOrDefault(u => u.MissionId == Mid).Title;
            
            string url = Url.ActionLink("StoryDetails", "Home", new { sid = Sid });
            
            string body = "Greetings of the Day !! <br><br>        " + UserName + " Invited you to join '" + MissionTitle + "' Mission if you have not joined yet and watch this Story. <br><br>" + url;
            //_VolunteeringMission.SendInvitation(EmailTo, UserId, Sid, body);
            _StoryDetails.AddStoryInvite(UserId, EmailTo, Sid);
            return Json(SendTo);
        }

    }

    
}