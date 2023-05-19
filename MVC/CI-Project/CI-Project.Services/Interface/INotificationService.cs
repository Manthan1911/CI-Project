using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Services.Interface
{
    public interface INotificationService
    {
        public Task<IEnumerable<UserNotificationModel>> GetAllByUserId(long userId);

        public Task MarkAsRead(long userNotificationId);

        public Task ClearAll(long userId);

        public Task<NotificationSettingsModel> GetNotificationSettingsByUserId(long userId);

        public Task SaveNotificationSettings(NotificationSettingsModel notificationSettingsModel);

        public void SendNotificationToAllUsers(SendNotificationVm sendNotificationVm);
        public void SendNotificationToSpecificUser(SendNotificationVm sendNotificationVm);
        public IEnumerable<NotificationSetting?> GetAllNotificationSettings();

    }
}
