﻿using CI_Platform.Models;
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
        private readonly CiPlatformContext _db;
        private readonly IMissionListingRepository _missionListingDb;
        private readonly IMissionCardRepository _MissionCard;
        private readonly IVolunteeringMissionRepository _VolunteeringMission;
        private readonly IFavouriteMission _FavouriteMissions;
        private readonly IRepository<City> _Cities;
        private readonly IStoryListingRepository _StoriyListingDb;
        private readonly IStoryCardRepository _StoryCard;
        public HomeController(CiPlatformContext db, 
            IMissionListingRepository missionListingDb,
            IMissionCardRepository MissionCard,
            IRepository<City> Cities,
            IVolunteeringMissionRepository volunteeringMission,
            IFavouriteMission FavouriteMissions,
            IStoryListingRepository StoryListingDb,
            IStoryCardRepository StoryCards)
        {
            _db = db;
            _missionListingDb = missionListingDb;
            _MissionCard = MissionCard;
            _Cities = Cities;
            _VolunteeringMission = volunteeringMission;
            _FavouriteMissions = FavouriteMissions;
            _StoriyListingDb = StoryListingDb;
            _StoryCard = StoryCards;
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
    }
}