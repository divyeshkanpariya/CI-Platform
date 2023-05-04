using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform.Controllers
{
    public class StoryController : Controller
    {
        private readonly IRepository<City> _Cities;
        private readonly IStoryListingRepository _StoriyListingDb;
        private readonly IStoryCardRepository _StoryCard;
        private readonly IShareStoryRepository _ShareStory;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IStoryDetailsRepository _StoryDetails;
        private readonly IRepository<Story> _StoryList;
        private readonly IRepository<Mission> _Missions;
        public StoryController(
            IRepository<City> Cities,
            IStoryListingRepository StoryListingDb,
            IStoryCardRepository StoryCards,
            IShareStoryRepository ShareStory,
            IWebHostEnvironment webHostEnvironment,
            IStoryDetailsRepository StoryDetails,
            IRepository<Story> StoryList,
            IRepository<Mission> Missions)
        {

            _Cities = Cities;
            _StoriyListingDb = StoryListingDb;
            _StoryCard = StoryCards;
            _ShareStory = ShareStory;
            _WebHostEnvironment = webHostEnvironment;
            _StoryDetails = StoryDetails;
            _StoryList = StoryList;
            _Missions = Missions;
        }
        public IActionResult StoryListing()
        {
            if (HttpContext.Session.GetString("Role") == "User" && HttpContext.Session.GetString("UserId") != "")
            {
                string CountryIDs = "";
                string CityIDs = "";
                string ThemeIDs = "";
                string SkillIDs = "";
                string SearchText = "";
                string PageIndex = "";
                string UserId = HttpContext.Session.GetString("UserId")!;
                var data = _StoriyListingDb.GetAllData(CountryIDs, CityIDs, ThemeIDs, SkillIDs, SearchText, UserId, PageIndex);
                return View(data);
            }
            else
            {
                TempData["SuccessMessage"] = "Login is Required";
                return RedirectToAction("Login", "Auth");
            }

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
            string UserId = HttpContext.Session.GetString("UserId")!;
            var data = _StoriyListingDb.GetAllData(CountryIDs, CityIDs, ThemeIDs, SkillIDs, SearchText, UserId, PageIndex);

            return PartialView("StoryCards", data);
        }

        public IActionResult ShareYourStory()
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            ShareYourStoryViewModel shareYourStoryViewModel = new ShareYourStoryViewModel();
            if (HttpContext.Session.GetString("Role") == "User" && HttpContext.Session.GetString("UserId") != "")
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

            if(formData["Mission"] == "Select Your Mission")
            {
                ModelState.AddModelError("Mission", "Please Select Mission");
                return RedirectToAction("ShareYourStory", "Story");
            }
            if(formData["StoryTitle"] == "")
            {
                ModelState.AddModelError("StoryTitle", "Please Add Story Title");
                return RedirectToAction("ShareYourStory", "Story");
            }
            if (formData["Date"] == "")
            {
                ModelState.AddModelError("Date", "Please Select Valid Date");
                return RedirectToAction("ShareYourStory", "Story");
            }
            if (formData["StoryDescription"] == "")
            {
                ModelState.AddModelError("StoryDescription", "Please Add Some Text");
                return RedirectToAction("ShareYourStory", "Story");
            }
            if(formData.Files.Count == 0)
            {
                ModelState.AddModelError("Photos", "Please Add Photos");
                return RedirectToAction("ShareYourStory", "Story");
            }

            ShareYourStoryViewModel viewModel = new ShareYourStoryViewModel()
            {
                Mission = formData["Mission"]!,
                Date = Convert.ToDateTime(formData["Date"]),
                StoryTitle = formData["StoryTitle"]!,
                StoryDescription = formData["StoryDescription"]!,
                StoryVideoUrl = formData["StoryVideoUrl"]!,
                Photos = formData.Files,
            };

            //if (ModelState.IsValid)
            //{

                UploadStory(viewModel, "PENDING");
                TempData["SuccessMessage"] = "Story Submitted Successfully !!!";
                return RedirectToAction("StoryListing");

            
        }

        [HttpPost]
        public IActionResult SaveStoryDraft(IFormCollection formData)
        {
            if (formData["StoryTitle"] == "")
            {
                ModelState.AddModelError("StoryTitle", "Please Add Story Title");
                return RedirectToAction("ShareYourStory", "Story");
            }
            if (formData["Date"] == "")
            {
                ModelState.AddModelError("Date", "Please Select Valid Date");
                return RedirectToAction("ShareYourStory", "Story");
            }
            if (formData["StoryDescription"] == "")
            {
                ModelState.AddModelError("StoryDescription", "Please Add Some Text");
                return RedirectToAction("ShareYourStory","Story");
            }
            if (formData.Files.Count == 0)
            {
                ModelState.AddModelError("Photos", "Please Add Photos");
                return RedirectToAction("ShareYourStory", "Story");
            }
            foreach(var file in formData.Files)
            {
                if (file.ContentType.Substring(0,5) != "image")
                {
                    ModelState.AddModelError("Photos", "File must be Image");
                }
            }
            ShareYourStoryViewModel viewModel = new ShareYourStoryViewModel()
            {
                
                Mission = formData["Mission"]!,
                Date = Convert.ToDateTime(formData["Date"]),
                StoryTitle = formData["StoryTitle"]!,
                StoryDescription = formData["StoryDescription"]!,
                StoryVideoUrl = formData["StoryVideoUrl"]!,
                Photos = formData.Files,
            };

           
                UploadStory(viewModel, "DRAFT");
                TempData["SuccessMessage"] = "Story Saved Successfully !!!";
                return RedirectToAction("StoryListing");

        }
        [HttpPost]
        public IActionResult GetStoryDetails(string MissionId)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));

            List<List<string>> list = _ShareStory.GetStoryDetails(Convert.ToInt64(MissionId), UserId);


            return Json(list);
        }
        public void UploadStory(ShareYourStoryViewModel viewModel, string status)
        {
            long UserId = Convert.ToInt64(HttpContext.Session.GetString("UserId"));
            long StoryId = _ShareStory.UploadStory(viewModel, UserId, status);
            if (_StoryList.ExistUser(u => u.MissionId == Convert.ToInt64(viewModel.Mission) && u.UserId == UserId))
            {
                _ShareStory.DeleteMedia(StoryId);
            }
            foreach (var file in viewModel.Photos!)
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

            string[] Urls = viewModel.StoryVideoUrl!.Split('\r');
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
                if (finUrl != "")
                {
                    _ShareStory.UploadMedia(StoryId, "URL", finUrl);

                }
            }
        }

        public IActionResult StoryDetails(string sid)
        {
            if (HttpContext.Session.GetString("Role") == "User" && HttpContext.Session.GetString("UserId") != "")
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
            string UserName = HttpContext.Session.GetString("UserName")!;
            long Mid = _StoryList.GetFirstOrDefault(u => u.StoryId == Sid).MissionId;
            string MissionTitle = _Missions.GetFirstOrDefault(u => u.MissionId == Mid).Title;

            string url = Url.ActionLink("StoryDetails", "Story", new { sid = Sid })!;

            string body = "Greetings of the Day !! <br><br>        " + UserName + " Invited you to join '" + MissionTitle + "' Mission if you have not joined yet and watch this Story. <br><br>" + url;
            _StoryDetails.AddStoryInvite(UserId, EmailTo, Sid);
            return Json(SendTo);
        }
        
        public IActionResult PreviewStory(string sid)
        {
            long StoryId = Convert.ToInt64(sid);
            IEnumerable<StoryCardViewModel> storyCard = _StoryDetails.GetAllData(StoryId);
            return View(storyCard);
        }
    }
}
