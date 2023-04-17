using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IVolunteeringTimesheetRepository:IRepository<Timesheet>
    {
        public List<Timesheet> GetAllWithInclude();
        public List<Mission> GetAllAppliedMissionsOfUser(long userId, string type);
		public Mission? GetMission(long? missionId);

	}
}
