using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class MissionCardViewModel
    {
        public IEnumerable<Mission> Missions { get; set; }

        public string path { get; set; }

        public int ? rating { get; set; }
    }
}
