namespace CI_Project.Entities.ViewModels
{
    public class NotificationMainModel
    {

        public IEnumerable<UserNotificationModel>? LatestNotifications { get; set; }
        
        public IEnumerable<UserNotificationModel>? OlderNotifications { get; set; }

    }
}
