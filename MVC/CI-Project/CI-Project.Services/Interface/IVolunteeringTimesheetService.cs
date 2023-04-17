using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using Org.BouncyCastle.Bcpg;

namespace CI_Project.Services.Interface
{
    public interface IVolunteeringTimesheetService
    {

		public List<MissionTimesheetTimeModel> GetAllTimeData(long userId);
		public List<MissionTimesheetGoalModel> GetAllGoalData(long userId);
        public MissionTimesheetTimeModel ConvertToTimeModel(Timesheet timesheetData);
        public MissionTimesheetGoalModel ConvertToGoalModel(Timesheet timesheetData);
		
		public MissionTimesheetTimeModel GetParticularTimeBasedData(long timesheetId);
		public MissionTimesheetGoalModel GetParticularGoalBasedData(long timesheetId);

		public Timesheet GetTimesheetData(long timesheetId);
		public List<Mission> GetMissionList(long userId, string type);

		public bool IsDateValidToSaveTimesheetEntry(DateTime date, long? missionId);
        public void SaveTimeData(MissionTimesheetTimeModel missionTimesheetTimeModel);
		public void SaveGoalData(MissionTimesheetGoalModel missionTimesheetGoalModel);

		public void EditTimeData(MissionTimesheetTimeModel missionTimesheetTimeModel);
		public void EditGoalData(MissionTimesheetGoalModel missionTimesheetGoalModel);

		public void DeleteTimesheetData(long timesheetId);

	}
}
