﻿using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Http;
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
        private IRepository<User> _Users;

        public UserProfileRepository(IRepository<City> CityList, IRepository<Country> countryList, IRepository<User> Users)
        {
            _CityList = CityList;
            _CountryList = countryList;
            _Users = Users;
        }

        public void AddUserData(UserProfileViewModel viewModel, long UserId,string ProfilePath)
        {
            var CurrUser = _Users.GetFirstOrDefault(x => x.UserId == UserId);
            if(viewModel.ProfileImage != null)
            {
                string oldProfile = "/Users/pci117/source/repos/divyeshkanpariya/CI-Platform/MVC/CI-Platform/CI-Platform/wwwroot" + CurrUser.Avatar;
                if (File.Exists(oldProfile))
                {
                    File.Delete(oldProfile);
                }
                CurrUser.Avatar = ProfilePath;
            }
            CurrUser.FirstName = viewModel.Name;
            CurrUser.LastName = viewModel.Surname;
            CurrUser.ProfileText = viewModel.MyProfileText;
            CurrUser.CountryId = Convert.ToInt64(viewModel.Country);
            CurrUser.WhyIVolunteer = viewModel.WhyIVol;
            CurrUser.CityId = Convert.ToInt64(viewModel.City);
            CurrUser.EmployeeId = viewModel.EmployeeId;
            CurrUser.Title = viewModel.Title;
            CurrUser.Department = viewModel.Department;
            CurrUser.LinkedInUrl = viewModel.LinkedinURL;

            //if (viewModel.City != null) CurrUser.CityId = Convert.ToInt64(viewModel.City);
            //if (viewModel.EmployeeId != null) CurrUser.EmployeeId = viewModel.EmployeeId;
            //if (viewModel.Title != null) CurrUser.Title = viewModel.Title;
            
            //if (viewModel.Department != null) CurrUser.Department = viewModel.Department;
            //if (viewModel.LinkedinURL != null) CurrUser.LinkedInUrl = viewModel.LinkedinURL;
            
            _Users.Update(CurrUser);
            _Users.Save();
            
        }

        public string ChangePassword(long UserId, string OldPwd, string NewPwd)
        {
            var user = _Users.GetFirstOrDefault(u => u.UserId == UserId);
            if(user.Password != OldPwd)
            {
                return "Password Not Matched";
            }
            user.Password = NewPwd;
            _Users.Update(user);
            _Users.Save();
            return "";
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

        public IEnumerable<UserProfileViewModel> GetUserData(long UserId)
        {
            List<UserProfileViewModel> model = new List<UserProfileViewModel>();
            var user = _Users.GetFirstOrDefault(u => u.UserId == UserId);

            UserProfileViewModel userData = new UserProfileViewModel()
            { 
                Name = user.FirstName,
                Surname = user.LastName,
                EmployeeId = user.EmployeeId,
                Title = user.Title,
                Department = user.Department,
                MyProfileText = user.ProfileText,
                WhyIVol = user.WhyIVolunteer,
                City = Convert.ToString(user.CityId),
                Country = Convert.ToString(user.CountryId),
                LinkedinURL = user.LinkedInUrl

            };

            model.Add(userData);

            return model;
        }
    }
}
