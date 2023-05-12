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
    }
}
