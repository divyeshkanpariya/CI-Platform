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

        public string getUserAvatar(string email)
        {
            string avatar = "/images/volunteer1.png";
            if(_userRepository.GetFirstOrDefault(u => u.Email == email).Avatar != null)
            {
                avatar = _userRepository.GetFirstOrDefault(u => u.Email == email).Avatar;
            }
            return _userRepository.GetFirstOrDefault(u => u.Email == email).Avatar;
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
