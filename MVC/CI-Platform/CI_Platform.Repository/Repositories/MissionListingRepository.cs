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
        
        private readonly IRepository<City> _CityList;
        private readonly IRepository<Country> _CountryList;
        private readonly IRepository<MissionTheme> _ThemeList;
        private readonly IRepository<Skill> _SkillList;


        public MissionListingRepository(IRepository<City> cityList,IRepository<Country> countryList,IRepository<MissionTheme> ThemeList,IRepository<Skill> SkillList)
        {
            
            _CityList = cityList;
            _CountryList = countryList;
            _ThemeList = ThemeList;
            _SkillList = SkillList;
        }
        public MissionListingViewModel GetAllData()
        {
            /*            IQueryable<City> citys = _CityList;
                        IQueryable<Country> countryList = _CountryList;*/
            Console.WriteLine(_CityList.GetAll());
            MissionListingViewModel viewModel = new MissionListingViewModel()
            {
                Cities = _CityList.GetAll(),
                Countries = _CountryList.GetAll(),
                MissionThemes = _ThemeList.GetAll(),
                Skills = _SkillList.GetAll(),
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
