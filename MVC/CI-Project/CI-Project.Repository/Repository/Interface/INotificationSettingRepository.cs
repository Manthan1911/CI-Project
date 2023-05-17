using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface INotificationSettingRepository:IRepository<NotificationSetting>
    {
        public Task<NotificationSetting?> GetNotificationSettingsByUserId(long userId);
        public Task<NotificationSetting?> GetNotificationSettingsByUserIdUsingSp(long userId);

    }
}
