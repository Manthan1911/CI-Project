using CI_Project.Entities.DataModels;
using CI_Project.Entities.Enums;

namespace CI_Project.Entities.ViewModels
{
    public class NotificationModel
    {
        public long NotificationId { get; set; }

        public string? NotificationText { get; set; }

        public NotificationType? NotificationType { get; set; }

        public string? UserAvtar { get; set; }

        public virtual ICollection<UserNotification> UserNotifications { get; } = new List<UserNotification>();
    }
}
