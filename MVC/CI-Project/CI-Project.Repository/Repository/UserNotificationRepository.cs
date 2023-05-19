using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
    public class UserNotificationRepository : Repository<UserNotification>, IUserNotificationRepository
    {
        private readonly CIProjectDbContext _db;
        public UserNotificationRepository(CIProjectDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task ClearAll(long userId)
        {
            string query = "DELETE FROM user_notification user_id = {0}";
            await _db.Database.ExecuteSqlRawAsync(query, userId);
        }

        public async Task<IEnumerable<UserNotification>> GetAllByUserId(long userId)
        {
            return await table.Include(userNotifcation => userNotifcation.Notification).Where(userNotification => userNotification.UserId == userId).OrderByDescending(userNotification => userNotification.CreatedAt).ToListAsync();
        }

        public async Task MarkAsRead(long userNotificationId)
        {
            string query = "UPDATE user_notification SET is_read = 1 WHERE user_notification_id = {0}";
            await _db.Database.ExecuteSqlRawAsync(query, userNotificationId);
        }
    }
}
