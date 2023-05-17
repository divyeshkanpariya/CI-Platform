using CI_Platform.Models.Models;
using CI_Platform.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Repositories
{
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly CiPlatformContext _db;
        private readonly IRepository<Notification> _notificationsRepository;
        private readonly IRepository<UserSetNotification> _userSetNotifications;

        public NotificationsRepository(CiPlatformContext db,
            IRepository<Notification> notifications,
            IRepository<UserSetNotification> userSetNotifications)
        {
            _db = db;
            _notificationsRepository = notifications;
            _userSetNotifications = userSetNotifications;
        }
        public IEnumerable<Notification> getNotifications(long UserId)
        {
            List<Notification> notifications = (from n in _db.Notifications
                                                join userChoice in _db.UserSetNotifications 
                                                on n.UserId equals userChoice.UserId
                                                where n.UserId == UserId
                                                select n).ToList();

            return notifications;
        }
    }
}
