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
        private readonly IRepository<Admin> _admins;

        public LoginRepository(IRepository<User> userRepo,IRepository<Admin> admins)
        {
            _userRepository = userRepo;
            _admins = admins;
        }
        public List<string> GetLoginDetails(string EmailId,string Password)
        {
            List<string> LoginDetails = new List<string>();
            if(_admins.ExistUser(u => u.Email == EmailId && u.Password == Password))
            {
                Admin admin = _admins.GetFirstOrDefault(u => u.Email == EmailId && u.Password == Password);
                LoginDetails.Add("Admin");
                LoginDetails.Add(admin.AdminId.ToString());
                LoginDetails.Add(admin.Email);
                string Name = "";
                if(admin.FirstName != null) Name += admin.FirstName + " ";
                if(admin.LastName != null) Name += admin.LastName;
                LoginDetails.Add(Name);
            }else if(_userRepository.ExistUser(u => u.Email == EmailId && u.Password == Password && u.DeletedAt == null))
            {
                User user = _userRepository.GetFirstOrDefault(u => u.Email == EmailId && u.Password == Password && u.DeletedAt == null);
                LoginDetails.Add("User");
                LoginDetails.Add(user.UserId.ToString());
                LoginDetails.Add(user.Email);
                string Name = "";
                if (user.FirstName != null) Name += user.FirstName + " ";
                if (user.LastName != null) Name += user.LastName;
                LoginDetails.Add(Name);
                if (user.Avatar != null) LoginDetails.Add(user.Avatar);
                else LoginDetails.Add("");
            }
            return LoginDetails;
        }
        public string getUserAvatar(string email)
        {
            string avatar = "/images/default-user-icon.jpg";
            if(_userRepository.GetFirstOrDefault(u => u.Email == email).Avatar != null)
            {
                avatar = _userRepository.GetFirstOrDefault(u => u.Email == email).Avatar;
            }
            
            return avatar;
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

        public string getUserEmail(long UserId)
        {
            return _userRepository.GetFirstOrDefault(u => u.UserId == UserId).Email;
        }
    }
}
