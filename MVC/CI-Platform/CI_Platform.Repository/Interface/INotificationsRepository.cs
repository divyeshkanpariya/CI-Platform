﻿using CI_Platform.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Repository.Interface
{
    public interface INotificationsRepository
    {
        public IEnumerable<Notification> getNotifications(long UserId);
    }
}
