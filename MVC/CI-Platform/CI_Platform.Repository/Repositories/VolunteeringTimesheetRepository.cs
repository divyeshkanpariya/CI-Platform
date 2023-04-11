using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class VolunteeringTimesheetRepository : IVolunteeringTimesheet
    {
        private readonly IRepository<MissionApplication> _missionApplications;
        private readonly IRepository<Mission> _Missions;
        private readonly IRepository<Timesheet> _TimeSheet;
        private readonly IRepository<GoalMission> _MissionGoals;

        public VolunteeringTimesheetRepository(IRepository<MissionApplication> missionApplications,
            IRepository<Mission> missionList,
            IRepository<Timesheet> timeSheet, 
            IRepository<GoalMission> missionGoals)
        {
            _missionApplications = missionApplications;
            _Missions = missionList;
            _TimeSheet = timeSheet;
            _MissionGoals = missionGoals;
        }
        public List<List<string>> Missions(long UserId)
        {
            List<List<string>> opList = new List<List<string>>();
            var missionApps = _missionApplications.GetAll().Where(u => u.UserId == UserId && u.ApprovalStatus == "APPROVE");
            foreach (var missionApp in missionApps)
            {
                var mission = _Missions.GetFirstOrDefault(u => missionApp.MissionId == u.MissionId);
                List<string> x = new List<string>();
                x.Add(Convert.ToString(mission.MissionId));
                x.Add(mission.Title);
                x.Add(mission.MissionType);
                if(mission.StartDate != null)
                {
                    x.Add(mission.StartDate.ToString().Split(" ")[0]);
                }
                else
                {
                    x.Add("");
                }

                opList.Add(x);
            }

            return opList;
        }

        public void SaveVolDetails(string Type, long UserId, long MissionId, DateTime Date, int Hours, int Minutes, int Action, string Message)
        {
            
            Timesheet newEntry = new Timesheet()
            {
                UserId = UserId,
                MissionId = MissionId,
                DateVolunteered = Date,
                Notes = Message,
                Status = "SUBMIT_FOR_APPROVAL"
            };
            if(Type == "Time")
            {
                newEntry.Time = new TimeSpan(Hours,Minutes,0);
                
            }
            else
            {
                newEntry.Action = Action;
                var ms = _MissionGoals.GetFirstOrDefault(u => u.MissionId == MissionId);
                ms.GoalArchived += Action;
                _MissionGoals.Update(ms);
                _MissionGoals.Save();

            }
            _TimeSheet.AddNew(newEntry);
            _TimeSheet.Save();


        }
    }
}
