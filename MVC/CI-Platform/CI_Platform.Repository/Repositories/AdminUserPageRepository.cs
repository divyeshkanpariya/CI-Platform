using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class AdminUserPageRepository : IAdminUserPageRepositoty
    {
        private readonly CiPlatformContext _db;
        private readonly IRepository<User> _UserList;
        

        public AdminUserPageRepository(CiPlatformContext db,IRepository<User> UserList)
        {
            _db = db;
            _UserList = UserList;
        }
        public IEnumerable<AdminUserTableViewModel> UsersList(string searchText, int pageIndex)
        {
            List<AdminUserTableViewModel> UsersList = new List<AdminUserTableViewModel>();
            var Users = (from u in _db.Users
                    where (u.FirstName.Contains(searchText) || u.LastName.Contains(searchText) || u.Email.Contains(searchText))
                    && (u.DeletedAt == null)
                    select new
                    {
                        u.UserId,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.EmployeeId,
                        u.Department,
                        u.Status
                    }).ToList();

            foreach (var User in Users)
            {
                AdminUserTableViewModel ViewModel = new AdminUserTableViewModel()
                {
                    UserId = User.UserId,
                    FirstName = User.FirstName,
                    LastName = User.LastName,
                    Email = User.Email,
                    EmployeeId = User.EmployeeId,
                    Department = User.Department,
                    Status = User.Status,
                    TotalUsers = Users.Count(),
                };
                UsersList.Add(ViewModel);
            }
            var pagesize = 9;
            int PageIndex = pageIndex;
            if (PageIndex != null)
            {
                if (PageIndex == null)
                {
                    PageIndex = 1;
                }
                UsersList = UsersList.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            }

            return UsersList;
        }


        public AdminAddUserViewModel getUserDetails(long UserId)
        {
            User Userdetails = _UserList.GetFirstOrDefault(x => x.UserId == UserId);
            AdminAddUserViewModel ViewModel = new AdminAddUserViewModel()
            {
                UserId = UserId.ToString(),
                Name = Userdetails.FirstName,
                Surname = Userdetails.LastName,
                Email = Userdetails.Email,
                EmployeeId = Userdetails.EmployeeId,
                Department = Userdetails.Department,
                ProfileText = Userdetails.ProfileText,
                City = Userdetails.CityId.ToString(),
                Country = Userdetails.CountryId.ToString(),
                Status = Convert.ToChar(Userdetails.Status),
            };
            return ViewModel;
        }

        public List<string> getUserLocation(string Email)
        {
            List<string> locations = new List<string>();
            if (Email != null)
            {
                User user = _UserList.GetFirstOrDefault(user => user.Email == Email);
                if (user != null)
                {
                    locations.Add(user.CountryId.ToString());
                    locations.Add(user.CityId.ToString());
                }
            }
            return locations;
        }

        public void SaveUserDetails(AdminAddUserViewModel model)
        {
            if(_UserList.ExistUser(user => user.UserId == Convert.ToInt64(model.UserId)))
            {
                User user = _UserList.GetFirstOrDefault(user => user.UserId == Convert.ToInt64(model.UserId));
                user.Email = model.Email.Trim().Replace(" ","");
                user.CountryId = Convert.ToInt64(model.Country);
                user.CityId = Convert.ToInt64(model.City);
                user.FirstName = model.Name;
                user.LastName = model.Surname;
                user.EmployeeId = model.EmployeeId;
                user.Department = model.Department;
                user.ProfileText = model.ProfileText;
                user.Status = model.Status.ToString();
                user.UpdatedAt = DateTime.Now;
                _UserList.Update(user);
                _UserList.Save();

            }
            else
            {
                User newUser = new User()
                {
                    Email = model.Email,
                    FirstName = model.Name,
                    LastName = model.Surname,
                    Department = model.Department,
                    EmployeeId = model.EmployeeId,
                    ProfileText = model.ProfileText,
                    CityId = Convert.ToInt64(model.City),
                    CountryId = Convert.ToInt64(model.Country),
                    Status = model.Status.ToString(),
                    Password = Guid.NewGuid().ToString(),
                };

                _UserList.AddNew(newUser);
                _UserList.Save();
            }
        }

        public void DeleteUser(long UserId)
        {
            if(_UserList.ExistUser(u => u.UserId == UserId))
            {
                User user = _UserList.GetFirstOrDefault(u => u.UserId == UserId);
                user.DeletedAt = DateTime.Now;
                _UserList.Update(user);
                _UserList.Save();
            }
        }

        public bool ExistUser(string Email)
        {
            return _UserList.ExistUser(u => u.Email == Email && u.DeletedAt == null);
        }
    }
}
