using CI_Platform.Models.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class MissionListingViewModel
    {
        public IEnumerable<City> Cities { get; set; }

        public IEnumerable<Country> Countries { get; set; }

        public IEnumerable<MissionTheme> MissionThemes  { get; set; }

        public IEnumerable<Skill> Skills { get; set; }

        public IEnumerable<Mission> Missions { get; set; }

        public IEnumerable<MissionMedium> MissionMedia { get; set; }

        public IEnumerable<MissionCardViewModel> MissionCards { get; set; }

        public IEnumerable<MissionSeat> MissionSeats { get; set; }

        public IEnumerable<GoalMission> Goals { get; set; }

    }
}
