using CI_Project.Entities.DataModels;
using CI_Project.Entities.Enums;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;

namespace CI_Project.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ClearAll(long userId)
        {
            await _unitOfWork.UserNotification.ClearAll(userId);
        }

        public async Task<IEnumerable<UserNotificationModel>> GetAllByUserId(long userId)
        {
            var userNotificataions = await _unitOfWork.UserNotification.GetAllByUserId(userId);
            return userNotificataions.Select(ConvertUserNotificationToVm);
        }

        public async Task MarkAsRead(long userNotificationId)
        {
            await _unitOfWork.UserNotification.MarkAsRead(userNotificationId);
        }

        public async Task<NotificationSettingsModel> GetNotificationSettingsByUserId(long userId)
        {
            NotificationSetting? notificationSettings = await _unitOfWork.NotificationSetting.GetNotificationSettingsByUserId(userId);
            if (notificationSettings is null) throw new Exception("Notification setting is null!: " + userId);
            
            NotificationSettingsModel notificationSettingsModel = ConvertNotificationSettingToVm(notificationSettings);
            return notificationSettingsModel;
        }

        public UserNotificationModel ConvertUserNotificationToVm(UserNotification userNotificationObj)
        {
            return new UserNotificationModel()
            {
                UserNotificationId = userNotificationObj.UserNotificationId,
                UserId = userNotificationObj.UserId,
                NotificationId = userNotificationObj.NotificationId,
                IsRead = userNotificationObj.IsRead,
                NotificationVm = ConvertUserNotificationToVm(userNotificationObj.Notification),
                CreatedAt = userNotificationObj.CreatedAt,
                UpdatedAt = userNotificationObj.UpdatedAt,
            };
        }

        public NotificationModel ConvertUserNotificationToVm(Notification notification)
        {
            return new NotificationModel()
            {
                NotificationId = notification.NotificationId,
                NotificationText = notification.NotificationText,
                NotificationType = (NotificationType)notification.NotificationType,
                UserAvtar = notification.UserAvtar,
            };
        }

        public NotificationSettingsModel ConvertNotificationSettingToVm(NotificationSetting notificationSetting)
        {
            return new NotificationSettingsModel()
            {
                UserId = notificationSetting.UserId,
                VolunteeringGoal = (bool)notificationSetting.VolunteeringGoal,
                VolunteeringHour = (bool)notificationSetting.VolunteeringHour,
                Mail = (bool)notificationSetting.Mail,
                MissionApplication = (bool)notificationSetting.MissionApplication,
                MyStory = (bool)notificationSetting.MyStory,
                NewMessage = (bool)notificationSetting.NewMessage,
                NewMission = (bool)notificationSetting.NewMission,
                News = (bool)notificationSetting.News,
                RecommendMission = (bool)notificationSetting.RecommendMission,
                RecommendStory = (bool)notificationSetting.RecommendStory,
            };
        }

        public async Task SaveNotificationSettings(NotificationSettingsModel notificationSettingsModel)
        {
            NotificationSetting? notificationSettings = await _unitOfWork.NotificationSetting.GetNotificationSettingsByUserId(notificationSettingsModel.UserId);
            if (notificationSettings is null) throw new Exception("Notification setting is null!: " + notificationSettingsModel.UserId);

            notificationSettings.RecommendMission = notificationSettingsModel.RecommendMission;
            notificationSettings.RecommendStory = notificationSettingsModel.RecommendStory;
            notificationSettings.VolunteeringHour = notificationSettingsModel.VolunteeringHour;
            notificationSettings.VolunteeringGoal = notificationSettingsModel.VolunteeringGoal;
            notificationSettings.MissionApplication = notificationSettingsModel.MissionApplication;
            notificationSettings.MyStory = notificationSettingsModel.MyStory;
            notificationSettings.NewMessage = notificationSettingsModel.NewMessage;
            notificationSettings.Mail = notificationSettingsModel.Mail;
            notificationSettings.NewMission = notificationSettingsModel.NewMission;
            notificationSettings.News = notificationSettingsModel.News;

            _unitOfWork.Save();
        }
    }
}
