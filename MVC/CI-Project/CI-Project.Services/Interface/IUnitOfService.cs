namespace CI_Project.Services.Interface
{
	public interface IUnitOfService
	{
		public IUserProfileService UserProfile { get; }
		public IVolunteeringTimesheetService VolunteeringTimesheet { get; }
		public IPasswordService Password { get; }
		public INotificationService Notification { get; }
	}
}