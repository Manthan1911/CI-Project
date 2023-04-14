namespace CI_Project.Services.Interface
{
	public interface IUnitOfService
	{
		public IHomeService Home { get; }
		public IStoryService Story { get; }
		public IUserService User { get; }
		public IUserProfileService UserProfile { get; }
		public IVolunteeringMissionService VolunteeringMission { get; }

		public IVolunteeringTimesheetService VolunteeringTimesheet { get; }
		public IPasswordService Password { get; }
	}
}
