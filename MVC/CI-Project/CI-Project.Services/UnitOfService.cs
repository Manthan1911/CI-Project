using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;

namespace CI_Project.Services
{
	public class UnitOfService : IUnitOfService
	{
		private readonly IUnitOfWork _unitOfWork;
		public UnitOfService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			Home = new HomeService(_unitOfWork);
			Story = new StoryService(_unitOfWork);
			User = new UserService(_unitOfWork);
			UserProfile = new UserProfileService(_unitOfWork);
			VolunteeringMission = new VolunteeringMissionService(_unitOfWork);
			VolunteeringTimesheet = new VolunteeringTimesheetService(_unitOfWork);
			Password = new PasswordService(_unitOfWork);
		}

		public IHomeService Home { get; }
		public IStoryService Story { get; }
		public IUserService User { get; }
		public IUserProfileService UserProfile { get; }
		public IVolunteeringMissionService VolunteeringMission { get; }
		public IVolunteeringTimesheetService VolunteeringTimesheet { get; }
		public IPasswordService Password { get; }

	}
}
