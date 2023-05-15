using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
    public class NotificationSettingsRepository : Repository<NotificationSetting>, INotificationSettingRepository
    {

        public NotificationSettingsRepository(CIProjectDbContext db) : base(db)
        {
        }

        public async Task<NotificationSetting?> GetNotificationSettingsByUserId(long userId) =>
            await table.FirstOrDefaultAsync(notificationSetting => notificationSetting.UserId == userId);
          
    }
}
