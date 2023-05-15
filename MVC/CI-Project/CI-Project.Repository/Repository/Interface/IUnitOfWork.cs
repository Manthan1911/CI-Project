namespace CI_Project.Repository.Repository.Interface
{
	public interface IUnitOfWork
	{
		IUserProfileRepository UserProfile { get; }

		IVolunteeringTimesheetRepository VolunteeringTimesheet { get; }
		
		INotificationRepository Notification { get; }

		IUserNotificationRepository UserNotification { get; }

		ILastCheckRepository LastCheck { get; }

		INotificationSettingRepository NotificationSetting { get; }

		void Save();
	}
}
