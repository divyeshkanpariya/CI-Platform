using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CI_Platform.Repository.Repositories
{
    public class MissionListingRepository : IMissionListingRepository
    {
        private readonly CiPlatformContext _db;
        private readonly IRepository<City> _CityList;
        private readonly IRepository<Country> _CountryList;
        private readonly IRepository<MissionTheme> _ThemeList;
        private readonly IRepository<Skill> _SkillList;
        private readonly IRepository<Mission> _Missions;
        private readonly IRepository<MissionMedium> _MissionMedia;
        private readonly IRepository<MissionRating> _MissionRatingList;
        private readonly IMissionCardRepository _MissionCard;


        public MissionListingRepository(CiPlatformContext db,IRepository<City> cityList, IRepository<Country> countryList, IRepository<MissionTheme> ThemeList, IRepository<Skill> SkillList, IRepository<Mission> MissionList, IRepository<MissionMedium> MissionMedia, IRepository<MissionMedium> missionMedia,IMissionCardRepository MissionCard,IRepository<MissionRating> missionRating)
        {
            _db = db;
            _CityList = cityList;
            _CountryList = countryList;
            _ThemeList = ThemeList;
            _SkillList = SkillList;
            _Missions = MissionList;
            _MissionMedia = missionMedia;
            _MissionCard = MissionCard;
            _MissionRatingList = missionRating;
        }
          
        
        public MissionListingViewModel GetAllData()
        {
            

            MissionListingViewModel viewModel = new MissionListingViewModel();

            viewModel.MissionCards = _MissionCard.FillData(_Missions.GetAll());

            //MissionCardViewModel cardmodel = new MissionCardViewModel();
            //IEnumerable<Mission> missions = _Missions.GetAll();

            //IEnumerable<MissionCardViewModel> miss = _MissionCard.FillData(missions);

            viewModel.Cities = _CityList.GetAll();
            viewModel.Countries = _CountryList.GetAll();
            viewModel.MissionThemes = _ThemeList.GetAll();
            viewModel.Skills = _SkillList.GetAll();
            viewModel.Missions = _Missions.GetAll();
            viewModel.MissionMedia = _MissionMedia.GetAll();

            foreach(var mission in viewModel.MissionCards)
            {
                mission.City = _CityList.GetAll().Where(u => u.CityId == mission.CityId).FirstOrDefault().Name;

                mission.Country = _CountryList.GetAll().Where(u=>u.CountryId == mission.CountryId).FirstOrDefault().Name;

                mission.Theme = _ThemeList.GetAll().Where(u => u.MissionThemeId == mission.ThemeId).FirstOrDefault().Title;
                bool path = _MissionMedia.GetAll().Any(u => u.MissionId == mission.MissionId);


                if (path)
                {
                    mission.Path = _MissionMedia.GetAll().Where(u => u.MissionId == mission.MissionId).FirstOrDefault().MediaPath;
                }else
                {
                    mission.Path = "https://localhost:7172/images/Grow-Trees-On-the-path-to-environment-sustainability.png";
                }
                

               

               bool ms = _MissionRatingList.GetAll().Any(u=>u.MissionId == mission.MissionId);
                
                if (ms)
                {
                    //    
                    //    Console.WriteLine(ms);
                    //    
                    var x = _MissionRatingList.GetAll().Where(u => u.MissionId == mission.MissionId).Average(r => r.Rating);
                    mission.Ratings = Convert.ToInt16(x);
                    //    mission.Ratings = Convert.ToInt16(rat);
                }
                else
                {
                    mission.Ratings = 0;
                }
                if(mission.StartDate != null && mission.EndDate != null){
                    if (DateTime.Compare((DateTime)mission.StartDate, DateTime.Now) < 0 && DateTime.Compare((DateTime)mission.EndDate, DateTime.Now) >= 0)
                    {
                        mission.IsOngoingActivity = true;
                    }
                    else
                    {
                        mission.IsOngoingActivity = false;
                    }
                }
                

            }
                
            
            return viewModel;

            
        }
        



        public IEnumerable<City> getCityByCountry(long CountriesId)
        {
            var cities = _CityList.GetAll().Where(u=>u.CountryId== CountriesId);
            return cities;
        }
    }
}
