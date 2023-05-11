using System;
using System.Collections.Generic;

namespace CI_Project.Entities.DataModels;

public partial class Notification
{
    public long NotificationId { get; set; }

    public string? NotificationText { get; set; }

    public int? NotificationType { get; set; }

    public string? UserAvtar { get; set; }

    public virtual ICollection<UserNotification> UserNotifications { get; } = new List<UserNotification>();
}
