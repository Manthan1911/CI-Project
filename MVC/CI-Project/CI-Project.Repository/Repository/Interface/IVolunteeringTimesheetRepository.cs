using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IVolunteeringTimesheetRepository
    {
        public List<Timesheet> GetAllWithInclude();
        public List<Mission> GetAllAppliedMissionsOfUser(long userId);

        public Mission? GetMission(long? missionId);
        public void SaveTimeData(Timesheet timesheetObj);

	}
}
