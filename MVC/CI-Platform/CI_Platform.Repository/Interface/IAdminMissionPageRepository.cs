using CI_Platform.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface IAdminMissionPageRepository
    {
        public IEnumerable<AdminMissionTableViewModel> GetAdminMissions(string searchText, int pageIndex);

        public void DeleteMission(long missionId);
    }
}
