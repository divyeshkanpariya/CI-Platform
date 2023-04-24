using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminStoryRepository
    {

        public IEnumerable<AdminStoryTableViewModel> GetStoriesApplications(string SearchText, int PageIndex);

        public void UpdateStatus(long StoryId, string Status);
    }
}
