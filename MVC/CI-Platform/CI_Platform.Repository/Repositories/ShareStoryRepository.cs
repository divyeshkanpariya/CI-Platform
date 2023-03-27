using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class ShareStoryRepository : IShareStoryRepository
    {
        private readonly IRepository<Mission> _Missions;
        private readonly IRepository<MissionApplication> _MissionApplications;
        private readonly IRepository<Story> _Storys;
        private readonly IRepository<StoryMedium> _StoryMediaList;


        public ShareStoryRepository(IRepository<Mission> Missions,
            IRepository<MissionApplication> MissionApplications,
            IRepository<Story> storys,
            IRepository<StoryMedium> StoryMediaList)
        {
            _Missions = Missions;
            _MissionApplications = MissionApplications;
            _Storys = storys;
            _StoryMediaList = StoryMediaList;
        }

        public List<List<string>> GetMissions(long UserId)
        {
            List<long> missionIds = new List<long>();

            IEnumerable<MissionApplication> missionApplications = _MissionApplications.GetAll().Where(u=>UserId == UserId);

            foreach (MissionApplication application in missionApplications)
            {
                missionIds.Add(application.MissionId);
            }
            IEnumerable<Mission> missions = _Missions.GetAll().Where(u => missionIds.Contains(u.MissionId));

            List<List<string>> selM = new List<List<string>>();
            foreach (var user in missions)
            {

                List<string> newU = new List<string>()
                        {

                            Convert.ToString(user.MissionId),
                            user.Title
                        };
                selM.Add(newU);
            }
            return selM;
        }

        public long UploadStory(ShareYourStoryViewModel ShareStoryModel,long UserId)
        {
            long MissionId = Convert.ToInt64(ShareStoryModel.Mission);
            Story newStory = new Story
            {
                UserId = UserId,
                MissionId = MissionId,
                Title = ShareStoryModel.StoryTitle,
                Description = ShareStoryModel.StoryDescription,
                Status = "PENDING",
                PublishedAt = Convert.ToDateTime(ShareStoryModel.Date)

            };
            _Storys.AddNew(newStory);
            _Storys.Save();
            return _Storys.GetAll().Where(u=> u.UserId == UserId && u.MissionId == MissionId).LastOrDefault().StoryId;
        }

        public void UploadMedia(long StoryId, string Type, string Path)
        {
            StoryMedium newMedia = new StoryMedium
            {
                StoryId = StoryId,
                Type = Type,
                Path = Path
            };
            _StoryMediaList.AddNew(newMedia);
            _StoryMediaList.Save();
        }

        public List<List<string>> GetStoryDetails(long MissionId,long UserId)
        {
            var story = _Storys.GetFirstOrDefault(u=> u.MissionId==MissionId && u.UserId == UserId);
            if(story == null)
            {
                return new List<List<string>>();
            }
            var storyMedia = _StoryMediaList.GetAll().Where(u=>u.StoryId == story.StoryId);
            List<List<string>> storyDetails = new List<List<string>>();
            List<string> title = new List<string>() { story.Title };
            List<string> Date = new List<string>() { Convert.ToString(story.PublishedAt).Split(" ")[0] };
            List<string> Description = new List<string>() {story.Description };
            List<string> VideoUrls = new List<string>();
            List<string> Photos = new List<string>();
            foreach(var item in storyMedia)
            {
                if(item.Type == "URL")
                {
                    VideoUrls.Add(item.Path);
                }
                else
                {
                    Photos.Add(item.Path);
                }
            }
            storyDetails.Add(title);
            storyDetails.Add(Date);
            storyDetails.Add(Description);
            storyDetails.Add(VideoUrls);
            storyDetails.Add(Photos);
            return storyDetails;

        }
    }
}
