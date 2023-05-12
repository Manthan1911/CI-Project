namespace CI_Project.Entities.ViewModels
{
    public class NotificationMainModel
    {

        public IEnumerable<UserNotificationModel>? UserNotificatios { get; set; }

        public IEnumerable<NotificationSettingsModel>? UserNotificationSettings { get; set; }
    }
}
