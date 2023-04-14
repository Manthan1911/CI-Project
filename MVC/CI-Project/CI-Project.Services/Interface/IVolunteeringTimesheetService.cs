using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Services.Interface
{
    public interface IVolunteeringTimesheetService
    {

        public List<MissionTimesheetTimeModel> GetAllTimeData(long userId);
        public List<MissionTimesheetGoalModel> GetAllGoalData(long userId);
        public MissionTimesheetTimeModel ConvertToTimeModel(Timesheet timesheetData);
        public MissionTimesheetGoalModel ConvertToGoalModel(Timesheet timesheetData);


    }
}
