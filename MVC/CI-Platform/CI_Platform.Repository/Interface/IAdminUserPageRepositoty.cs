using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminUserPageRepositoty
    {
        public IEnumerable<AdminUserTableViewModel> UsersList(string searchtext, int pageIndex);

        public AdminAddUserViewModel getUserDetails(long UserId);

        public List<string> getUserLocation(string Email);

        public void SaveUserDetails(AdminAddUserViewModel model);

        public void DeleteUser(long UserId);
    }
}
