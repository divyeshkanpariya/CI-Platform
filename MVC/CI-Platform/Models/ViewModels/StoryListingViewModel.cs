using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class StoryListingViewModel
    {
        public IEnumerable<City> Cities { get; set; }

        public IEnumerable<Country> Countries { get; set; }

        public IEnumerable<MissionTheme> MissionThemes { get; set; }

        public IEnumerable<Skill> Skills { get; set; }

        public IEnumerable<StoryCardViewModel> StoryCards { get; set; }
    }
}
