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

        public RegistrationRepository(CiPlatformContext db) : base(db)
        {
            _db = db;
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
    }
}
