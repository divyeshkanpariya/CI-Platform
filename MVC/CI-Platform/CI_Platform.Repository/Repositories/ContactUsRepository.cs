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
    public class ContactUsRepository : IContactUsRepository
    {
        private readonly IRepository<ContactU> _contactUs;

        public ContactUsRepository(IRepository<ContactU> contactUs)
        {
            _contactUs = contactUs;
        }
        public void SubmitContact(long UserId, string Subject, string Message)
        {
            ContactU newC = new ContactU()
            {
                UserId = UserId,
                Subject = Subject,
                Message = Message
            };
            _contactUs.AddNew(newC);
            _contactUs.Save();
        }
    }
}
