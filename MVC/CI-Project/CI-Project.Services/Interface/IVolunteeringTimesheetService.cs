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
        public List<Mission> GetMissionList(long userId);
        public bool IsDateValidToSaveTimesheetEntry(DateTime date, long? missionId);
        public void SaveTimeData(MissionTimesheetTimeModel missionTimesheetTimeModel);


	}
}
