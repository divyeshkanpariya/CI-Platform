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
        public HomeController(CiPlatformContext db, IMissionListingRepository missionListingDb,IMissionCardRepository MissionCard)
        {
            _db = db;
            _missionListingDb = missionListingDb;
            _MissionCard = MissionCard;
        }
        public IActionResult MissionListing()
        {
            if(HttpContext.Session.GetString("UserName") != null)
            {
                var data = _missionListingDb.GetAllData();
                return View(data);
            }else
            {
                TempData["SuccessMessage"] = "Login is Required";

                return RedirectToAction("Login", "Auth");
            }
            
        }
        public IActionResult getCityByCountry(long countryId)
        {
            var cities = _missionListingDb.getCityByCountry(countryId);
            return Json(cities);
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