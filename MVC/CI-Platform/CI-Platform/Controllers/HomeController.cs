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
        public HomeController(
            IMissionListingRepository missionListingDb,
            IMissionCardRepository MissionCard,
            IRepository<City> Cities,
            IVolunteeringMissionRepository volunteeringMission,
            IFavouriteMission FavouriteMissions,
            IStoryListingRepository StoryListingDb,
            IStoryCardRepository StoryCards,
            IShareStoryRepository ShareStory,
            IWebHostEnvironment webHostEnvironment)
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

            _VolunteeringMission.SendInvitation(EmailTo, senderId, MissionId, senderName, url);


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
        public IActionResult ShareYourStory(ShareYourStoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
                long StoryId = _ShareStory.UploadStory(viewModel, UserId);

                foreach (var file in viewModel.Photos)
                {
                    string folder = "Uploads/Story/";
                    folder += Convert.ToString(StoryId) + "-" + Guid.NewGuid().ToString() + file.FileName;
                    string serverFolder = Path.Combine(_WebHostEnvironment.WebRootPath, folder);
                    file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    string path = "/" + folder;
                    string FileType = Convert.ToString(file.ContentType);
                    _ShareStory.UploadMedia(StoryId, FileType, path);
                }
                
                string[] Urls = viewModel.StoryVideoUrl.Split('\r');
                int x=0;
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

                    _ShareStory.UploadMedia(StoryId, "URL", finUrl);
                }
                return RedirectToAction("StoryListing");
                
            }
            else
            {
                return View(viewModel);
            }
        }
        
        [HttpPost]
        public IActionResult SaveStoryDraft(ShareYourStoryViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("StoryListing");
            }
            return RedirectToAction("StoryListing");
        }
        [HttpPost]
        public IActionResult GetStoryDetails(string MissionId)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            List<List<string>> list = _ShareStory.GetStoryDetails(Convert.ToInt64(MissionId), UserId);
            return Json(list);
        }
        //[HttpPost]

        //public IActionResult SaveStoryDetails(string MissionId, string StoryTitle, string Date, string StoryDesc, string VideoURL,string[] Images,string Status)
        //{


        //    return Json(MissionId);


        //}


    }

    
}