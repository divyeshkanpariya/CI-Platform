using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private IRepository<City> _CityList;
        private IRepository<Country> _CountryList;

        public UserProfileRepository(IRepository<City> CityList, IRepository<Country> countryList)
        {
            _CityList = CityList;
            _CountryList = countryList;
        }
        public IEnumerable<City> GetAllCities( long CountryId)
        {
            var cities = _CityList.GetAll();
            if (CountryId == 0)
            {
                return cities;
            }
            else
            {
                cities = cities.Where(u => u.CountryId == CountryId);
                return cities;
            }
            
        }

        public IEnumerable<Country> GetAllCountries()
        {
            var countries = _CountryList.GetAll();
            return countries;
        }
    }
}
