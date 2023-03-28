using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IShareStoryRepository
    {
        public List<List<string>> GetMissions(long UserId);

        public long UploadStory(ShareYourStoryViewModel ShareStoryModel,long UserId, string status);

        public void UploadMedia(long StoryId, string Type, string Path);

        public List<List<string>> GetStoryDetails(long MissionId,long UserId);

        public void DeleteMedia(long StoryId);
    }
}
