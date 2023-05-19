using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly CIProjectDbContext _db;
        public NotificationRepository(CIProjectDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<User?> SendNotificationToAllUsers(SendNotificationVm sendNotificationVm)
        {
            try
            {
                //_db.Users.FromSqlInterpolated($"exec sp_SendNotificationToAllUsers @user_id={sendNotificationVm.UserId},@notification_text={sendNotificationVm.NotificationText},@notification_type={sendNotificationVm.NotificationType},@user_avtar={sendNotificationVm.Avtar},@to_users={sendNotificationVm.ToUsers},@created_at={DateTime.Now},@setting_type_name={sendNotificationVm.SettingTypeName}");
                var users = _db.Users.FromSqlRaw("EXEC sp_SendNotificationToAllUsers @user_id, @notification_text, @notification_type, @user_avtar, @to_users, @created_at, @setting_type_name",
                    new SqlParameter("@user_id", sendNotificationVm.UserId ?? 0),
                    new SqlParameter("@notification_text", sendNotificationVm.NotificationText),
                    new SqlParameter("@notification_type", sendNotificationVm.NotificationType),
                    new SqlParameter("@user_avtar", sendNotificationVm.Avtar ?? (Object)DBNull.Value),
                    new SqlParameter("@to_users", sendNotificationVm.ToUsers ?? (Object)DBNull.Value),
                    new SqlParameter("@created_at", DateTime.Now),
                    new SqlParameter("@setting_type_name", sendNotificationVm.SettingTypeName));

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public User? SendNotificationToSpecificUser(SendNotificationVm sendNotificationVm)
        {
            throw new NotImplementedException();
        }
    }
}
