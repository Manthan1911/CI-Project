using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
	public class UnitOfWork:IUnitOfWork
	{
		private readonly CIProjectDbContext _db;

		public UnitOfWork(CIProjectDbContext db)
		{
			_db = db;
			User = new UserRepository(_db);
			Home = new HomeRepository(_db);
			Story= new StoryRepository(_db);
			UserProfile=new UserProfileRepository(_db);
			VolunteeringMission = new VolunteeringMissionRepository(Home,_db);
			VolunteeringTimesheet = new VolunteeringTimesheetRepository(_db);
		}

		public IHomeRepository Home { get; }

		public IStoryRepository Story { get; }

		public IUserProfileRepository UserProfile { get; }

		public IUserRepository User { get; }

		public IVolunteeringMissionRepository VolunteeringMission { get; }
		public IVolunteeringTimesheetRepository VolunteeringTimesheet { get; }
	}
}
