using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IUserNotificationRepository : IRepository<UserNotification>
    {
        public Task<IEnumerable<UserNotification>> GetAllByUserId(long userId);

        public Task MarkAsRead(long userNotificationId);

        public Task ClearAll(long userId);
    }
}
