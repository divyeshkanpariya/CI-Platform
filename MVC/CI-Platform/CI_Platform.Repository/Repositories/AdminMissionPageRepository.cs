using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class AdminMissionPageRepository : IAdminMissionPageRepository
    {

        private readonly CiPlatformContext _db;
        private readonly IRepository<Mission> _Missions;
        public AdminMissionPageRepository(CiPlatformContext db,
            IRepository<Mission> missionList)
        {
            _db = db;
            _Missions = missionList;
        }

        public void DeleteMission(long missionId)
        {
            if(_Missions.ExistUser(ms => ms.MissionId == missionId))
            {
                Mission mission = _Missions.GetFirstOrDefault(ms => ms.MissionId == missionId);
                if(mission != null)
                {
                    mission.DeletedAt = DateTime.Now;
                    _Missions.Update(mission);
                    _Missions.Save();
                }
            }
        }

        public IEnumerable<AdminMissionTableViewModel> GetAdminMissions(string searchText, int pageIndex)
        {
            List<AdminMissionTableViewModel> MissionList = new List<AdminMissionTableViewModel>();
            
            List<Mission> Missions = (from mis in _db.Missions
                                      where mis.Title.Contains(searchText) && mis.DeletedAt == null
                                      select mis).ToList();
            if(Missions.Count > 0)
            {
                foreach (Mission mis in Missions)
                {
                    AdminMissionTableViewModel newmodel = new AdminMissionTableViewModel
                    {
                        Title = mis.Title,
                        MissionType = mis.MissionType,
                        MissionId = mis.MissionId,
                        StartDate = mis.StartDate,
                        EndDate = mis.EndDate,
                        TotalMissions = Missions.Count(),
                    };
                    MissionList.Add(newmodel);
                }
            }
            var pagesize = 8;
            int PageIndex = pageIndex;
            if (PageIndex != null)
            {
                if (PageIndex == null)
                {
                    PageIndex = 1;
                }
                MissionList = MissionList.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            }
            return MissionList;
        }
    }
}
