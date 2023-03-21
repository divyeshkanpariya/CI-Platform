using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class StoryCardViewModel : Story
    {
        public string UserProfile { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MissionTitle { get; set; }

        public string StoryMediaPath { get; set; }
    }
}
