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
        private readonly IRepository<User> _UserList;
        

        public AdminUserPageRepository(IRepository<User> UserList)
        {
            _UserList = UserList;
        }
        public IEnumerable<AdminUserTableViewModel> UsersList()
        {
            List<AdminUserTableViewModel> UsersList = new List<AdminUserTableViewModel>();
            //List<AdminUserTableViewModel> UsersList = (from u in _UserList
            //                                           select new
            //                                           {
            //                                               u.UserId,
            //                                               u.FirstName,
            //                                               u.LastName,
            //                                               u.Email,
            //                                               u.EmployeeId,
            //                                               u.Department,
            //                                               u.Status

            //                                           }).ToList();

            foreach (var User in _UserList.GetAll())
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
                };
                UsersList.Add(ViewModel);
            }
            var pagesize = 6;
            string PageIndex = "1";
            if (PageIndex != null)
            {
                if (PageIndex == "")
                {
                    PageIndex = "1";
                }
                UsersList = UsersList.Skip((Convert.ToInt16(PageIndex) - 1) * pagesize).Take(pagesize).ToList();
            }

            return UsersList;
        }
    }
}
