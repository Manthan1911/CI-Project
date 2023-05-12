using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;


namespace CI_Project.Repository.Repository
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        private readonly CIProjectDbContext _db;
        public NotificationRepository(CIProjectDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
