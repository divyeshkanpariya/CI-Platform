using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminStoryTableViewModel
    {
        public long StoryId { get; set; }

        public string UserId { get; set;}

        public string MissionId { get; set;}

        public string Title { get; set; }

        public string FullName { get; set; }

        public string MissionTitle { get; set; }

        public int StoryCount { get; set; }
    }
}
