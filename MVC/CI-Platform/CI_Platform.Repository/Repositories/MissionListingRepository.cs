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

        public MissionListingViewModel GetAllData()
        {
/*            IQueryable<City> citys = _CityList;
            IQueryable<Country> countryList = _CountryList;*/

            MissionListingViewModel viewModel = new MissionListingViewModel()
            {
                Cities = _CityList.GetAll(),
                Countries = _CountryList.GetAll(),
            };
            return viewModel;

            
        }



    }
}
