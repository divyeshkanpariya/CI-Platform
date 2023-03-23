using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CI_Platform.Models.ViewModels
{
    public class ShareYourStoryViewModel
    {
        public string Mission { get; set; }
        public string StoryTitle { get; set; }

        public DateOnly Date { get; set; }

        public string StoryDescription { get; set;}

        public string StoryVideoUrl { get; set;}

        public string Photos { get; set;}
         
        public IEnumerable<Mission> MissionList { get; set; }
    }
}
