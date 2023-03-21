using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IStoryListingRepository
    {
        public StoryListingViewModel GetAllData(string CountryIDs, string CityIDs, string ThemeIDs, string SkillIDs,string SearchText, string UserId, string PageIndex);

        public IEnumerable<City> getCityByCountry(long CountriesId);
    }
}
