using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
    public class NotificationSettingsRepository : Repository<NotificationSetting>, INotificationSettingRepository
    {
        private readonly CIProjectDbContext _db;
        public NotificationSettingsRepository(CIProjectDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<NotificationSetting?> GetNotificationSettingsByUserId(long userId) =>
            await table.FirstOrDefaultAsync(notificationSetting => notificationSetting.UserId == userId);


        public async Task<NotificationSetting?> GetNotificationSettingsByUserIdUsingSp(long userId)
        {

            var notificationSetting = await _db.NotificationSettings.FromSqlInterpolated($"exec sp_getnotificationsettingbyuserid @user_id={userId}").ToListAsync();
            return notificationSetting.FirstOrDefault();

        }
    }
}
