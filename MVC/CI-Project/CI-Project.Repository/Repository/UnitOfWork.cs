using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CIProjectDbContext _db;

        public UnitOfWork(CIProjectDbContext db)
        {
            _db = db;
            UserProfile = new UserProfileRepository(_db);
            VolunteeringTimesheet = new VolunteeringTimesheetRepository(_db);
            Notification = new NotificationRepository(_db);
            UserNotification = new UserNotificationRepository(_db);
            LastCheck = new LastCheckRepository(_db);
            NotificationSetting = new NotificationSettingsRepository(_db);
        }


        public IUserProfileRepository UserProfile { get; }

        public IVolunteeringTimesheetRepository VolunteeringTimesheet { get; }

        public INotificationRepository Notification { get; }

        public IUserNotificationRepository UserNotification { get; }

        public ILastCheckRepository LastCheck { get; }

        public INotificationSettingRepository NotificationSetting { get; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
