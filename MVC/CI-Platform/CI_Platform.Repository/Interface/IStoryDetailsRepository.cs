using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IStoryDetailsRepository
    {

        public IEnumerable<StoryCardViewModel> GetAllData(long StoryId);

        public void AddStoryInvite(long fromUid, long toUid,long StoryId);
        
    }
}
