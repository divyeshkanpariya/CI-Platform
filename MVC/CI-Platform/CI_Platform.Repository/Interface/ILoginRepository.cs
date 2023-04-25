using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface ILoginRepository
    {
        public List<string> GetLoginDetails(string EmailId, string Password);

        public long getUserId(string email);

        public string getUserName(string email);

        public string getUserAvatar(string email);

        public string getUserEmail(long userId);
    }
}
