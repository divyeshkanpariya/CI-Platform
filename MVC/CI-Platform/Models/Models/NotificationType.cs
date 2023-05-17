using System;
using System.Collections.Generic;

namespace CI_Platform.Models.Models;

public partial class NotificationType
{
    public long NotificationTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<UserSetNotification> UserSetNotifications { get; } = new List<UserSetNotification>();
}
