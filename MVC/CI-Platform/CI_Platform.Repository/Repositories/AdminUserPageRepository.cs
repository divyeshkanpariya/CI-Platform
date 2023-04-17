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
                    where u.FirstName.Contains(searchText) || u.LastName.Contains(searchText) || u.Email.Contains(searchText)
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
    }
}
