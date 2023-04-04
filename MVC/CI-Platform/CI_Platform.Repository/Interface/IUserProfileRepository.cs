using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IUserProfileRepository
    {
        public IEnumerable<City> GetAllCities(long CountryId);

        public IEnumerable<Country> GetAllCountries();

        public void AddUserData(UserProfileViewModel viewModel, long UserId, string ProfilePath);
    }
}
