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
    public class StoryDetailsRepository : IStoryDetailsRepository
    {
        private readonly IStoryCardRepository _StoryCardRepository;
        private readonly IRepository<Story> _StoryList;
        private readonly IRepository<StoryMedium> _StoryMediaList;
        private readonly IRepository<User> _Users;
        private readonly IRepository<StoryInvite> _StoryInviteList;

        public StoryDetailsRepository(IStoryCardRepository storyCardRepository,IRepository<Story> StoryList,IRepository<StoryMedium> StoryMediaList, IRepository<User> UserList,IRepository<StoryInvite> StoryInvite)
        {
            _StoryCardRepository = storyCardRepository;
            _StoryList = StoryList;
            _StoryMediaList = StoryMediaList;
            _Users = UserList;
            _StoryInviteList = StoryInvite;
        }

        public void AddStoryInvite(long fromUid, long toUid, long StoryId)
        {
            StoryInvite newInvite = new StoryInvite()
            {
                StoryId = StoryId,
                ToUserId = toUid,
                FromUserId = fromUid
            };
            _StoryInviteList.AddNew(newInvite);
            _StoryInviteList.Save();
        }

        public void addStoryView(long UserId,long StoryId)
        {
            var story = _StoryList.GetFirstOrDefault(u => u.StoryId == StoryId);
            if(story.UserId != UserId)
            {
                story.Views += 1;
            }
            _StoryList.Update(story);   
            _StoryList.Save();
        }

        public IEnumerable<StoryCardViewModel> GetAllData(long StoryId)
        {
            //StoryCardViewModel storyCard = new StoryCardViewModel();
            IEnumerable<Story> story = _StoryList.GetAll().Where(u => u.StoryId == StoryId);
            IEnumerable<StoryCardViewModel> currCard = _StoryCardRepository.FillData(story);
            IEnumerable<StoryMedium> MediaList = _StoryMediaList.GetAll().Where(u => u.StoryId == StoryId && u.Type != "URL");
            List<string> mediaL = new List<string>();
            if(MediaList.Count() > 0)
            {
                foreach (var item in MediaList)
                {
                    mediaL.Add(item.Path);
                }
            }
            else
            {
                mediaL.Add("/images/Default.jpg");
            }

            currCard.FirstOrDefault()!.StoryMediaList = mediaL;

            IEnumerable<StoryMedium> VideoList = _StoryMediaList.GetAll().Where(u => u.StoryId == StoryId && u.Type == "URL");
            List<string> videoUrlList = new List<string>();
            if(VideoList.Count() > 0)
            {
                foreach(var item in VideoList)
                {
                    videoUrlList.Add(item.Path);
                }
            }
            
            

            currCard.FirstOrDefault()!.StoryVideoPaths = videoUrlList;


            var StoryWriter = _Users.GetFirstOrDefault(u => u.UserId == story.FirstOrDefault()!.UserId);
            if(StoryWriter.Avatar != null)
            {
                currCard.FirstOrDefault()!.UserProfile = StoryWriter.Avatar;
            }
            else
            {
                currCard.FirstOrDefault()!.UserProfile = "/images/default-user-icon.jpg";
            }
            

            currCard.FirstOrDefault()!.FirstName = StoryWriter.FirstName;

            currCard.FirstOrDefault()!.LastName = StoryWriter.LastName;

            
            currCard.FirstOrDefault()!.WhyIVol = StoryWriter.WhyIVolunteer;

            List<List<string>> UserList = new List<List<string>>();

            foreach (var user in _Users.GetAll().Where(u => u.UserId != story.FirstOrDefault().UserId && u.DeletedAt == null))
            {
                List<string> newuser = new List<string>();
                newuser.Add(Convert.ToString(user.UserId));
                newuser.Add(user.FirstName!);
                newuser.Add(user.LastName!);
                newuser.Add(user.Email);
                UserList.Add(newuser);
            }
            currCard.FirstOrDefault().UsersList = UserList;

            return currCard;
        }
    }
}
