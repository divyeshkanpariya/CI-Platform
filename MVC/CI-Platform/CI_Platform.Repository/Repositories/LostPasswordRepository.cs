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
    public class LostPasswordRepository : Repository<PasswordReset>,ILostPasswordRepository
    {
        private readonly CiPlatformContext _db;
        public LostPasswordRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public PasswordReset newToken(Lost_passwordViewModel data,string token)
        {
            var reset_pwd = new PasswordReset
            {
                Email = data.Email,
                Token = token,
            };
            return reset_pwd;
        }

       
    }
}
