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
            var missionApps = _missionApplications.GetAll().Where(u => u.UserId == UserId && u.ApprovalStatus == "APPROVE" && u.DeletedAt == null);
            foreach (var missionApp in missionApps)
            {
                var mission = _Missions.GetFirstOrDefault(u => missionApp.MissionId == u.MissionId);
                if (mission.StartDate == null || mission.StartDate <= DateTime.Now)
                {
                    List<string> x = new List<string>();
                    x.Add(Convert.ToString(mission.MissionId));
                    x.Add(mission.Title);
                    x.Add(mission.MissionType);
                    if (mission.StartDate != null) x.Add(mission.StartDate.ToString().Split(" ")[0]);
                    else x.Add("");
                    if (mission.EndDate != null) x.Add(mission.EndDate.ToString().Split(" ")[0]);
                    else x.Add("");
                    opList.Add(x);
                }
                

                
            }

            return opList;
        }

        public List<List<string>> getVolDetailsByUser(long UserId)
        {
            List<List<string>> target = new List<List<string>>();

            
            
            var timesheet = _TimeSheet.GetAll().Where(u =>u.UserId == UserId && u.DeletedAt == null);

            foreach (var timesheetApp in timesheet)
            {
                List<string> vol = new List<string>();
                vol.Add(Convert.ToString(timesheetApp.TimesheetId));
                vol.Add(Convert.ToString(timesheetApp.MissionId));
                vol.Add(_Missions.GetFirstOrDefault(u =>u.MissionId == timesheetApp.MissionId).Title);
                vol.Add(_Missions.GetFirstOrDefault(u =>u.MissionId == timesheetApp.MissionId).MissionType);
                vol.Add(timesheetApp.DateVolunteered.ToString().Split(" ")[0]);
                if (timesheetApp.Time == null)
                {
                    vol.Add("");
                    vol.Add("");
                }
                else
                {
                    vol.Add(timesheetApp.Time.ToString().Split(":")[0]);
                    vol.Add(timesheetApp.Time.ToString().Split(":")[1]);
                }
                if (timesheetApp.Action == null) vol.Add("");
                else vol.Add(timesheetApp.Action.ToString());

                vol.Add(timesheetApp.Notes);

                target.Add(vol);
            }

            return target;
        }

        public void SaveVolDetails(string Type, long UserId, long MissionId, DateTime Date, int Hours, int Minutes, int Action, string Message, string Status)
        {
            if(_TimeSheet.ExistUser(u => u.UserId == UserId && u.MissionId == MissionId && u.DateVolunteered == Date && u.DeletedAt == null))
            {
                Timesheet field = _TimeSheet.GetFirstOrDefault(u => u.UserId == UserId && u.MissionId == MissionId && u.DateVolunteered == Date && u.DeletedAt == null);
                if (field != null)
                {
                    field.DateVolunteered = Date;
                    field.Notes = Message;
                    field.Status = "SUBMIT_FOR_APPROVAL";
                    field.UpdatedAt = DateTime.Now;
                    if (Type == "Time")
                    {
                        field.Time = new TimeSpan(Hours, Minutes, 0);

                    }
                    else
                    {
                        field.Action = Action;
                        var ms = _MissionGoals.GetFirstOrDefault(u => u.MissionId == MissionId);
                        ms.GoalArchived += ms.GoalArchived + Action - field.Action;
                        _MissionGoals.Update(ms);
                        _MissionGoals.Save();

                    }
                    _TimeSheet.Update(field);
                    _TimeSheet.Save();
                }
                

            }
            else if(Status == "" || Status ==null)
            {
                Timesheet newEntry = new Timesheet()
                {
                    UserId = UserId,
                    MissionId = MissionId,
                    DateVolunteered = Date,
                    Notes = Message,
                    Status = "SUBMIT_FOR_APPROVAL"
                };
                if (Type == "Time")
                {
                    newEntry.Time = new TimeSpan(Hours, Minutes, 0);

                }
                else
                {
                    newEntry.Action = Action;
                    var ms = _MissionGoals.GetFirstOrDefault(u => u.MissionId == MissionId);
                    ms.GoalArchived += Action;
                    ms.UpdatedAt = DateTime.Now;
                    _MissionGoals.Update(ms);
                    _MissionGoals.Save();

                }
                _TimeSheet.AddNew(newEntry);
                _TimeSheet.Save();
            }
            else
            {
                long timesheetId = Convert.ToInt64(Status);
                var raw = _TimeSheet.GetFirstOrDefault(u => u.TimesheetId == timesheetId);

                if (raw != null)
                {
                    raw.DateVolunteered = Date;
                    raw.Notes = Message;
                    raw.Status = "SUBMIT_FOR_APPROVAL";
                    raw.UpdatedAt = DateTime.Now;
                    if (Type == "Time")
                    {
                        raw.Time = new TimeSpan(Hours, Minutes, 0);

                    }
                    else
                    {

                        var ms = _MissionGoals.GetFirstOrDefault(u => u.MissionId == MissionId);
                        ms.GoalArchived = ms.GoalArchived + Action - raw.Action;
                        raw.Action = Action;
                        ms.UpdatedAt = DateTime.Now;
                        _MissionGoals.Update(ms);
                    }
                    _TimeSheet.Update(raw);
                    _TimeSheet.Save();
                }
            }
        }

        public void DeleteVol(long TimesheetId)
        {
            var field = _TimeSheet.GetFirstOrDefault(u => u.TimesheetId == TimesheetId);

            if(_MissionGoals.ExistUser(u=> u.MissionId == field.MissionId))
            {
                var goalMs = _MissionGoals.GetFirstOrDefault(u => u.MissionId == field.MissionId);
                goalMs.GoalArchived -= field.Action;
                goalMs.UpdatedAt = DateTime.Now;
            }
            field.UpdatedAt = DateTime.Now;
            field.DeletedAt = DateTime.Now;
            _TimeSheet.Update(field);
            _TimeSheet.Save();
        }
    }
}
