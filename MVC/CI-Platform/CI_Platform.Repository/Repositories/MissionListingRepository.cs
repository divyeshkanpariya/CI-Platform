using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        private MissionListingRepository _CardRepo;


        public MissionListingRepository(CiPlatformContext db,IRepository<City> cityList, IRepository<Country> countryList, IRepository<MissionTheme> ThemeList, IRepository<Skill> SkillList, IRepository<Mission> MissionList, IRepository<MissionMedium> MissionMedia, IRepository<MissionMedium> missionMedia,MissionCardRepository cardRepo)
        {
            _db = db;
            _CityList = cityList;
            _CountryList = countryList;
            _ThemeList = ThemeList;
            _SkillList = SkillList;
            _Missions = MissionList;
            _MissionMedia = missionMedia;
            /*_CardRepo = cardRepo;*/
        }
          
        
        public MissionListingViewModel GetAllData()
        {

            MissionListingViewModel viewModel = new MissionListingViewModel()
            {
                Cities = _CityList.GetAll(),
                Countries = _CountryList.GetAll(),
                MissionThemes = _ThemeList.GetAll(),
                Skills = _SkillList.GetAll(),
                Missions = _Missions.GetAll(),
                MissionMedia = _MissionMedia.GetAll(),
            };
            return viewModel;

            
        }
       



        public IEnumerable<City> getCityByCountry(long CountriesId)
        {
            var cities = _CityList.GetAll().Where(u=>u.CountryId== CountriesId);
            return cities;
        }
    }
}
