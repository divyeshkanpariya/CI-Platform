using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminSkillRepository
    {
        public IEnumerable<AdminSkillTableViewModel> GetSkills(string SearchText, int PageIndex);

        public string SaveSkill(int SkillId, string Name, string Status);

        public void DeleteSkill(int SkillId);
    }
}
