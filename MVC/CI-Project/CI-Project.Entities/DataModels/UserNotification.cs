using System;
using System.Collections.Generic;

namespace CI_Project.Entities.DataModels;

public partial class UserNotification
{
    public long UserNotificationId { get; set; }

    public long? UserId { get; set; }

    public long? NotificationId { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Notification? Notification { get; set; }

    public virtual User? User { get; set; }
}
