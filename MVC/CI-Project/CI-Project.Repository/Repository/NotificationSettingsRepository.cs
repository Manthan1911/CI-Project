using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
    public class NotificationSettingsRepository : Repository<NotificationSetting>, NotificationSettingRepository
    {
        public NotificationSettingsRepository(CIProjectDbContext db) : base(db)
        {
        }
    }
}
