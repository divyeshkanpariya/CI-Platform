using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IContactUsRepository
    {
        public void SubmitContact(long UserId,string Subject, string Message);
    }
}
