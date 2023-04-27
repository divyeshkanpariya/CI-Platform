using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IRegistrationRepository : IRepository<User>
    {
        User NewUser(RegistrationViewModel data);

        public bool ExistAdmin(string EmailId);
    }
}
