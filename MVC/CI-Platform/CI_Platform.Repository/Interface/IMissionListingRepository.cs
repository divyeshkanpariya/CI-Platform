using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IMissionListingRepository
    {
        public MissionListingViewModel GetAllData(string CountryIDs);

        public IEnumerable<City> getCityByCountry(long CountriesId);
    }
}
