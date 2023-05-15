using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
    public class UserNotificationModel
    {
        public long UserNotificationId { get; set; }

        public long? UserId { get; set; }

        public long? NotificationId { get; set; }

        public bool? IsRead { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual NotificationModel? NotificationVm { get; set; }

        public virtual User? User { get; set; }

    }
}
