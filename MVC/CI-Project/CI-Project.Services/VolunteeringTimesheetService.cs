using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;

namespace CI_Project.Services
{
	public class VolunteeringTimesheetService : IVolunteeringTimesheetService
	{

		private readonly IUnitOfWork _unitOfWork;
		public VolunteeringTimesheetService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<MissionTimesheetTimeModel> GetAllTimeData(long userId)
		{

			List<MissionTimesheetTimeModel> missionTimesheetTimeModel = new();

			foreach(var currTimeSheetData in _unitOfWork.VolunteeringTimesheet.GetAllWithInclude().Where(t => t.UserId == userId && t.Mission.MissionType.ToLower().Equals("time")).ToList())
			{
				missionTimesheetTimeModel.Add(ConvertToTimeModel(currTimeSheetData));
			}

			return missionTimesheetTimeModel;

		}

		public MissionTimesheetTimeModel ConvertToTimeModel(Timesheet timesheetData)
		{
			return new MissionTimesheetTimeModel()
			{
				MissionId = timesheetData.MissionId,
				UserId = timesheetData.UserId,
				//Hours = timesheetData.Time.Value.Hours,
				//Minutes = timesheetData.Time.Value.Minutes,
				Notes = timesheetData?.Notes,
				DateVolunteered = timesheetData?.DateVolunteered,
				ApprovalStatus = timesheetData?.Status,
			};
		}

		public List<MissionTimesheetGoalModel> GetAllGoalData(long userId)
		{
			List<MissionTimesheetGoalModel> missionTimesheetGoalModel = new();

			foreach (var currTimeSheetData in _unitOfWork.VolunteeringTimesheet.GetAllWithInclude().Where(t => t.UserId == userId && t.Mission.MissionType.ToLower().Equals("goal")).ToList())
			{
				missionTimesheetGoalModel.Add(ConvertToGoalModel(currTimeSheetData));
			}

			return missionTimesheetGoalModel;
		}

		public MissionTimesheetGoalModel ConvertToGoalModel(Timesheet timesheetData)
		{
			return new MissionTimesheetGoalModel()
			{
				MissionId = timesheetData.MissionId,
				UserId = timesheetData.UserId,
				Action= timesheetData.Action,
				Notes = timesheetData?.Notes,
				DateVolunteered = timesheetData?.DateVolunteered,
				ApprovalStatus = timesheetData?.Status,
			};
		}
	}
}
