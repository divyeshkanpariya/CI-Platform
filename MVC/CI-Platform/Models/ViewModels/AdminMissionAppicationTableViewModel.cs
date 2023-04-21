using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminMissionAppicationTableViewModel
    {
        public long MissionApplicationId { get; set; }
        public string MissionTitle { get; set; }

        public long MissionId { get; set; }

        public long UserId { get; set; }

        public DateTime AppliedDate { get; set; }

        public string UserName { get; set; }

        public int ApplicationCount { get; set; }
    }
}
