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
        private readonly IRepository<MissionTheme> _MissionThemes;
        public AdminMissionPageRepository(CiPlatformContext db,
            IRepository<Mission> missionList,
            IRepository<MissionTheme> missionThemes)
        {
            _db = db;
            _Missions = missionList;
            _MissionThemes = missionThemes;
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

        public AdminAddEditMissionViewModel  GetMissionDetails(long MissionId)
        {
            AdminAddEditMissionViewModel model = new AdminAddEditMissionViewModel();
            if(_Missions.ExistUser(ms => ms.MissionId == MissionId))
            {
                Mission mission = _Missions.GetFirstOrDefault(m => m.MissionId == MissionId);
                model.MissionId = MissionId;
                model.CityId = mission.CityId.ToString();
                model.CountryId = mission.CountryId.ToString();
                model.ThemeId = mission.ThemeId.ToString();
                model.Title = mission.Title;
                model.ShortDescription = mission.ShortDescription;
                model.Description = mission.Description;
                model.EndDate = mission.EndDate;
                model.StartDate = mission.StartDate;
                model.MissionType = mission.MissionType;
                model.OrganizationName = mission.OrganizationName;
                model.OrganizationDetail = mission.OrganizationDetail;
                model.Availability = mission.Availability;
                model.Status = mission.Status;
            }
            return model;
            
        }

        public List<string> GetMissionLoc(long MissionId)
        {
            List<string> missionLoc = new List<string>();
            if(MissionId != null && MissionId != 0)
            {
                if(_Missions.ExistUser(ms => ms.MissionId == MissionId))
                {
                    Mission mission = _Missions.GetFirstOrDefault(ms => ms.MissionId == MissionId);
                    missionLoc.Add(mission.CountryId.ToString());
                    missionLoc.Add(mission.CityId.ToString());
                    missionLoc.Add(mission.ThemeId.ToString());
                    missionLoc.Add(mission.MissionType);
                    missionLoc.Add(mission.Availability);
                }
                
            }
            return missionLoc;
        }
        public IEnumerable<MissionTheme> getMissionThemes()
        {
            return _MissionThemes.GetAll().Where(mt => mt.Status == "1");
        }
    }
}
