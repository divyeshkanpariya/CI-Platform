using System;
using System.Collections.Generic;

namespace CI_Platform.Models.Models;

public partial class Notification
{
    public long NotificationId { get; set; }

    public long NotificationTypeId { get; set; }

    public long UserId { get; set; }

    public string Text { get; set; } = null!;

    public long? MissionId { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual NotificationType NotificationType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
