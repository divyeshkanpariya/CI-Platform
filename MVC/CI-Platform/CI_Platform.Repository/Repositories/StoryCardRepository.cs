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
    public class StoryCardRepository : IStoryCardRepository
    {
        public IEnumerable<StoryCardViewModel> FillData(IEnumerable<Story> Stories)
        {
            List<StoryCardViewModel> StoryCard = new List<StoryCardViewModel>();

            foreach (Story story in Stories)
            {
                StoryCardViewModel view = new StoryCardViewModel();

                view.StoryId = story.StoryId;
                view.Title = story.Title;
                view.Description = story.Description;
                view.UserId = story.UserId;
                view.MissionId = story.MissionId;
                view.Status = story.Status;
                view.PublishedAt = story.PublishedAt;
                view.CreatedAt = story.CreatedAt;
                view.UpdatedAt = story.UpdatedAt;
                view.DeletedAt = story.DeletedAt;
                view.Views = story.Views;

                StoryCard.Add(view);
            }
            IEnumerable<StoryCardViewModel> fin = StoryCard;
            return fin;
        }
    }
}
