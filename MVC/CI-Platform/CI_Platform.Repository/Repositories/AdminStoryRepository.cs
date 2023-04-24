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
    public class AdminStoryRepository : IAdminStoryRepository
    {
        private CiPlatformContext _db;
        private readonly IRepository<Story> _Stories;
        private readonly IRepository<Mission> _Missions;
        private readonly IRepository<User> _Users;

        public AdminStoryRepository(CiPlatformContext db,
            IRepository<Story> stories,
            IRepository<Mission> missions,
            IRepository<User> users
        )
        {
            _db = db;
            _Stories = stories;
            _Missions = missions;
            _Users = users;
        }

        public IEnumerable<AdminStoryTableViewModel> GetStoriesApplications(string SearchText, int PageIndex)
        {
            List<AdminStoryTableViewModel> Applications = new List<AdminStoryTableViewModel>();
            IEnumerable<Story> storyApplications = (from story in _db.Stories
                                                                   join missions in _db.Missions on story.MissionId equals missions.MissionId
                                                                   join user in _db.Users on story.UserId equals user.UserId
                                                                   where story.Status == "PENDING" && (story.Title.Contains(SearchText)|| missions.Title.Contains(SearchText) || user.FirstName.Contains(SearchText) || user.LastName.Contains(SearchText))
                                                                   select story).ToList();

            foreach (Story storyApplication in storyApplications)
            {
                AdminStoryTableViewModel newstory = new AdminStoryTableViewModel
                {
                    StoryId = storyApplication.StoryId,
                    Title = storyApplication.Title,
                    MissionId = storyApplication.MissionId.ToString(),
                    UserId = storyApplication.UserId.ToString(),
                    MissionTitle = _Missions.GetFirstOrDefault(m => m.MissionId == storyApplication.MissionId).Title,
                    StoryCount = storyApplications.Count(),
                };
                string UserName = "";
                if (_Users.GetFirstOrDefault(u => u.UserId == storyApplication.UserId).FirstName != null) UserName += _Users.GetFirstOrDefault(u => u.UserId == storyApplication.UserId).FirstName + " ";
                if (_Users.GetFirstOrDefault(u => u.UserId == storyApplication.UserId).LastName != null) UserName += _Users.GetFirstOrDefault(u => u.UserId == storyApplication.UserId).LastName;

                newstory.FullName = UserName;

                Applications.Add(newstory);
            }
            var pagesize = 2;
            if (PageIndex != null)
            {
                if (PageIndex == null)
                {
                    PageIndex = 1;
                }
                Applications = Applications.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            }

            return Applications;
        }

        public void UpdateStatus(long StoryId, string Status)
        {
            if (_Stories.ExistUser(ma => ma.StoryId == StoryId))
            {
                Story storyApplication = _Stories.GetFirstOrDefault(ma => ma.StoryId == StoryId);

                if (storyApplication != null)
                {
                    storyApplication.Status = Status;
                    storyApplication.UpdatedAt = DateTime.Now;
                    _Stories.Update(storyApplication);
                    _Stories.Save();
                }
            }

        }
    }
}
