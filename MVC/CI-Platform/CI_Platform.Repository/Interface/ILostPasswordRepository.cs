using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface ILostPasswordRepository : IRepository<PasswordReset>
    {
        PasswordReset newToken(Lost_passwordViewModel data, string token);
    }
}
