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
    public class AdminVolunteeringRepository : IAdminVolunteering
    {
        private readonly CiPlatformContext _db;
        private readonly IRepository<User> _Users;
        private readonly IRepository<Mission> _Missions;
        private readonly IRepository<Timesheet> _Timesheets;
        private readonly IRepository<GoalMission> _MissionGoals;
        private readonly IRepository<UserSetNotification> _UserSetNotification;
        private readonly IRepository<Notification> _Notifications;

        public AdminVolunteeringRepository(CiPlatformContext db, 
            IRepository<User> users, 
            IRepository<Mission> missions, 
            IRepository<Timesheet> timesheets, 
            IRepository<GoalMission> missionGoals,
            IRepository<UserSetNotification> UserSetNotifications,
            IRepository<Notification> Notifications)
        {
            _db = db;
            _Users = users;
            _Missions = missions;
            _Timesheets = timesheets;
            _MissionGoals = missionGoals;
            _UserSetNotification = UserSetNotifications;
            _Notifications = Notifications;
        }

        public IEnumerable<AdminTimesheetViewModel> GetVolunteeringDetails(string SearchText, int PageIndex)
        {
            List<AdminTimesheetViewModel> adminTimesheetViewModels = new List<AdminTimesheetViewModel>();

            List<Timesheet> Timeshees = (from ts in _db.Timesheets
                                         join missions in _db.Missions on ts.MissionId equals missions.MissionId
                                         join user in _db.Users on ts.UserId equals user.UserId
                                         where ts.Status == "SUBMIT_FOR_APPROVAl" && ts.DeletedAt == null && (missions.Title.Contains(SearchText) || user.FirstName!.Contains(SearchText) || user.LastName!.Contains(SearchText))
                                         select ts).ToList();

            foreach (var timesheet in Timeshees)
            {
                AdminTimesheetViewModel newModel = new AdminTimesheetViewModel
                {
                    TimesheetId = timesheet.TimesheetId,
                    Time = timesheet.Time,
                    Action = timesheet.Action,
                    DateVolunteered = timesheet.DateVolunteered,
                };
                string UserName = "";
                if (_Users.GetFirstOrDefault(u => u.UserId == timesheet.UserId).FirstName != null) UserName += _Users.GetFirstOrDefault(u => u.UserId == timesheet.UserId).FirstName + " ";
                if (_Users.GetFirstOrDefault(u => u.UserId == timesheet.UserId).LastName != null) UserName += _Users.GetFirstOrDefault(u => u.UserId == timesheet.UserId).LastName;

                newModel.UserName = UserName;
                newModel.Mission = _Missions.GetFirstOrDefault(ms => ms.MissionId == timesheet.MissionId).Title;
                newModel.TotalTimesheets = Timeshees.Count();
                adminTimesheetViewModels.Add(newModel);
            }
            var pagesize = 9;

            adminTimesheetViewModels = adminTimesheetViewModels.Skip((PageIndex - 1) * pagesize).Take(pagesize).ToList();
            return adminTimesheetViewModels;
        }

        public void UpdateStatus(long TimesheetId, string Status)
        {
            Timesheet Vol = _Timesheets.GetFirstOrDefault(t => t.TimesheetId == TimesheetId);
            if(Status == "APPROVED")
            {
                if(_Missions.GetFirstOrDefault(u=> u.MissionId == Vol.MissionId).MissionType == "Go")
                {
                    GoalMission gm = _MissionGoals.GetFirstOrDefault(g => g.MissionId == Vol.MissionId);
                    gm.GoalArchived += Vol.Action;
                    gm.UpdatedAt = DateTime.Now;
                    _MissionGoals.Update(gm);
                    _MissionGoals.Save();
                }
            }
            Vol.Status = Status;
            Vol.UpdatedAt = DateTime.Now;
            _Timesheets.Update(Vol);
            _Timesheets.Save();
            if (_UserSetNotification.GetFirstOrDefault(us => us.UserId == Vol.UserId).Status == 1)
            {
                Notification NewNotification = new Notification();
                if (_Missions.GetFirstOrDefault(u => u.MissionId == Vol.MissionId).MissionType == "Go")
                {
                    NewNotification.NotificationTypeId = 4;
                }
                NewNotification.UserId = Vol.UserId;
                NewNotification.Text = "Volunteering Request has been Approved for this Mission - " + _Missions.GetFirstOrDefault(ms => ms.MissionId == Vol.MissionId).Title;
                NewNotification.Status = Status;

                _Notifications.AddNew(NewNotification);
                _Notifications.Save();

            }
        }
    }
}
