using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Models.ViewModels
{
    public class AdminSkillTableViewModel
    {

        public long SkillId { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public int SkillCount { get; set; }
    }
}
