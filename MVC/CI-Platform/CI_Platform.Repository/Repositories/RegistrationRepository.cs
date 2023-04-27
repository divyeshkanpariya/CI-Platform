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
    public class RegistrationRepository : Repository<User>, IRegistrationRepository
    {
        private readonly CiPlatformContext _db;
        private readonly IRepository<Admin> _Admins;

        public RegistrationRepository(CiPlatformContext db,IRepository<Admin> Admins) : base(db)
        {
            _db = db;
            _Admins = Admins;
        }

        public User NewUser(RegistrationViewModel data)
        {
            var user = new User
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                Password = data.Password,
            };
            return user;
        }

        public bool ExistAdmin(string EmailId)
        {
            return _Admins.ExistUser(admin => admin.Email == EmailId && admin.DeletedAt == null);
        }
    }
}
