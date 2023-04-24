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
    public class AdminMissionApplicationsRepository : IAdminMissionApplicationsRepository
    {
        private CiPlatformContext _db;
        private readonly IRepository<MissionApplication> _MissionApplications;
        private readonly IRepository<Mission> _Missions;
        private readonly IRepository<User> _Users;

        public AdminMissionApplicationsRepository(CiPlatformContext db,
            IRepository<MissionApplication> missionApplications,
            IRepository<Mission> missions,
            IRepository<User> users
        ){
            _db = db;
            _MissionApplications = missionApplications;
            _Missions = missions;
            _Users = users;
        }

        public IEnumerable<AdminMissionAppicationTableViewModel> GetMissionApplications(string SearchText,int PageIndex)
        {
            List<AdminMissionAppicationTableViewModel> Applications = new List<AdminMissionAppicationTableViewModel>();
            IEnumerable<MissionApplication> missionApplications = (from apps in _db.MissionApplications
                                                                   join missions in _db.Missions on apps.MissionId equals missions.MissionId
                                                                   join user in _db.Users on apps.UserId equals user.UserId
                                                                   where apps.ApprovalStatus == "PENDING" && (missions.Title.Contains(SearchText) || user.FirstName.Contains(SearchText) || user.LastName.Contains(SearchText))
                                                                   select apps).ToList();

            foreach(MissionApplication missionApplication in missionApplications)
            {

                AdminMissionAppicationTableViewModel newapp = new AdminMissionAppicationTableViewModel()
                {
                    MissionApplicationId = missionApplication.MissionApplicationId,
                    MissionTitle = _Missions.GetFirstOrDefault(m => m.MissionId == missionApplication.MissionId).Title,
                    MissionId = missionApplication.MissionId,
                    UserId = missionApplication.UserId,
                    AppliedDate = missionApplication.AppliedAt,
                    ApplicationCount = missionApplications.Count()
                    
                };
                string UserName = "";
                if (_Users.GetFirstOrDefault(u => u.UserId == missionApplication.UserId).FirstName != null) UserName += _Users.GetFirstOrDefault(u => u.UserId == missionApplication.UserId).FirstName +" ";
                if (_Users.GetFirstOrDefault(u => u.UserId == missionApplication.UserId).LastName != null) UserName += _Users.GetFirstOrDefault(u => u.UserId == missionApplication.UserId).LastName;

                newapp.UserName = UserName;

                Applications.Add(newapp);
            }

            var pagesize = 9;
            if (PageIndex != null)
            {
                if (PageIndex == null)
                {
                    PageIndex = 1;
                }
                Applications = Applications.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            }

            return Applications;
        }

        public void UpdateStatus( long AppId, string Status)
        {
            if(_MissionApplications.ExistUser(ma => ma.MissionApplicationId == AppId))
            {
                MissionApplication missionApplication = _MissionApplications.GetFirstOrDefault(ma => ma.MissionApplicationId == AppId);

                if (missionApplication != null)
                {
                    if(Status != "DELETE") missionApplication.ApprovalStatus = Status;
                    missionApplication.UpdatedAt = DateTime.Now;
                    if(Status == "DELETE") missionApplication.DeletedAt = DateTime.Now;
                    _MissionApplications.Update(missionApplication);
                    _MissionApplications.Save();
                }
            }
            
        }
    }
}
