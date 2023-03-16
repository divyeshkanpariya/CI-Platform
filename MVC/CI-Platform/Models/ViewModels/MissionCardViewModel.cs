using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class MissionCardViewModel : Mission
    {
        public string City { get; set; }

        public string Country { get; set; }

        public string Theme { get; set; }

        public string Path { get; set; }

        public short ? Ratings { get; set; }
        
        public bool? IsOngoingActivity { get; set; } = false;

        public bool? IsSeatDataFound { get; set; } = false;

        public byte? IsLimitedSeats { get; set; }

        public int? TotalSeats { get; set; }

        public int? SeatsFilled { get; set; }

        public int? SeatsLeft { get; set; }

        public string? GoalObjectiveText { get; set; }

        public int GoalValue { get; set; }

        public int? GoalArchived { get; set; }

        public float? prArchivement { get; set; }

        public bool? IsFavourite { get; set; } = false;

        public List<string> MissionSkills { get; set; }

        public List<string> MissionMediaPaths { get; set; }
    }
}
