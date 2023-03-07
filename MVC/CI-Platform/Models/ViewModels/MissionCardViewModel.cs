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
    }
}
