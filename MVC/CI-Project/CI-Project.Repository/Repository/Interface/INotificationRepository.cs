using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface INotificationRepository:IRepository<Notification>
    {
        public IEnumerable<User?> SendNotificationToAllUsers(SendNotificationVm sendNotificationVm);
        public User? SendNotificationToSpecificUser(SendNotificationVm sendNotificationVm);
    }
}
