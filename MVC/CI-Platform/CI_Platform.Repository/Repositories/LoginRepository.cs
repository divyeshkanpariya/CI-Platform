using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IRepository<User> _userRepository;

        public LoginRepository(IRepository<User> userRepo)
        {
            _userRepository = userRepo;
        }

        public long getUserId(string email)
        {
            IEnumerable<User> users = _userRepository.GetAll();

            long id = users.FirstOrDefault(u => u.Email == email).UserId;
            return id;

        }

        public string getUserName(string email)
        {
            IEnumerable<User> users = _userRepository.GetAll();
            string name = "";
            name += users.FirstOrDefault(u=> u.Email == email).FirstName;
            name += " " + users.FirstOrDefault(u => u.Email == email).LastName;

            return name;


        }
    }
}
