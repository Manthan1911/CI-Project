namespace CI_Project.Repository.Repository.Interface
{
	public interface IUnitOfWork
	{

		IUserRepository User { get; } 
		
		IHomeRepository Home { get; }

		IStoryRepository Story { get; }

		IUserProfileRepository UserProfile { get; }
		
		IVolunteeringMissionRepository VolunteeringMission { get; }

		IVolunteeringTimesheetRepository VolunteeringTimesheet { get;}
	}
}
