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
            if(EmailId  != null)
            {
                if(_admins.ExistUser(u => u.Email == EmailId && u.DeletedAt == null))
                {
                    Admin admin = _admins.GetFirstOrDefault(u => u.Email == EmailId && u.Password == Password && u.DeletedAt == null);
                    if(admin != null)
                    {
                        LoginDetails.Add("Admin");
                        LoginDetails.Add(admin.AdminId.ToString());
                        LoginDetails.Add(admin.Email);
                        string Name = "";
                        if (admin.FirstName != null) Name += admin.FirstName + " ";
                        if (admin.LastName != null) Name += admin.LastName;
                        LoginDetails.Add(Name);
                    }
                }
                else if (_userRepository.ExistUser(u => u.Email == EmailId && u.DeletedAt == null))
                {
                    User user = _userRepository.GetFirstOrDefault(u => u.Email == EmailId && u.DeletedAt == null);
                    if (BCrypt.Net.BCrypt.Verify(Password, user.Password))
                    {
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
                }
            }
            

            
            return LoginDetails;
        }
        public string getUserAvatar(string email)
        {
            string avatar = "/images/default-user-icon.jpg";
            if(_userRepository.GetFirstOrDefault(u => u.Email == email).Avatar != null)
            {
                avatar = _userRepository.GetFirstOrDefault(u => u.Email == email).Avatar!;
            }
            
            return avatar;
        }

        public long getUserId(string email)
        {
            User users = _userRepository.GetFirstOrDefault(u => u.Email == email);

            long id = users.UserId;
            return id;

        }

        public string getUserName(string email)
        {
            User users = _userRepository.GetFirstOrDefault(u => u.Email == email);
            string name = "";
            if(users.FirstName != null)
                name += users.FirstName;
            if (users.LastName != null)
                name += " " + users.LastName;

            return name;


        }

        public string getUserEmail(long UserId)
        {
            return _userRepository.GetFirstOrDefault(u => u.UserId == UserId).Email;
        }
    }
}
