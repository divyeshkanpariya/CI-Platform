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
    public class ResetPasswordRepository : Repository<PasswordReset>,IResetPasswordRepository

    {
        private readonly CiPlatformContext _db;
        public ResetPasswordRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
        }

        public bool IsTokenValid(ResetPasswordViewModel data, string token)
        {
            var query = dbSet.Where(u=> u.Email == data.Email && u.Token == token).OrderBy(u=>u.Id).LastOrDefault();
            
            if (query == null)
            {
                return false;
            }
            else
            {
                if (query.CreatedAt.AddMinutes(10) >= DateTime.Now) return true;
                else return false;
            }
        }
    }
}
