using CI_Project.Entities.Enums;

namespace CI_Project.Entities.ViewModels
{
    public class SendNotificationVm
    {
        public long? UserId { get; set; }

        public string NotificationText { get; set; } = string.Empty;
        
        public NotificationType NotificationType { get; set; }

        public string? Avtar { get; set; }

        public string? ToUsers { get; set; }
        
        public string SettingTypeName { get; set; } = string.Empty ;

        public string? Link { get; set; }
    }
}
