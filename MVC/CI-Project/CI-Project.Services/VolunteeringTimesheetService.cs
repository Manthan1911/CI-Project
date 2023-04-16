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

			foreach (var currTimeSheetData in _unitOfWork.VolunteeringTimesheet.GetAllWithInclude().Where(t => t.UserId == userId && t.Mission.MissionType.ToLower().Equals("time")).ToList())
			{
				missionTimesheetTimeModel.Add(ConvertToTimeModel(currTimeSheetData));
			}

			return missionTimesheetTimeModel;

		}

		public MissionTimesheetTimeModel ConvertToTimeModel(Timesheet timesheetData)
		{
			MissionTimesheetTimeModel mts = new MissionTimesheetTimeModel()

			{
				MissionId = timesheetData.MissionId,
				MissionName = timesheetData.Mission.Title,
				UserId = timesheetData.UserId,
				Notes = timesheetData?.Notes,
				DateVolunteered = timesheetData?.DateVolunteered,
				ApprovalStatus = timesheetData?.Status,
			};

			if (timesheetData?.Time != null)
			{
				mts.Hours = timesheetData.Time.Value.Hours;
				mts.Minutes = timesheetData.Time.Value.Minutes;
			}
			return mts;
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
				MissionName = timesheetData.Mission.Title,
				UserId = timesheetData.UserId,
				Action = timesheetData.Action,
				Notes = timesheetData?.Notes,
				DateVolunteered = timesheetData?.DateVolunteered,
				ApprovalStatus = timesheetData?.Status,
			};
		}

		public List<Mission> GetMissionList(long userId)
		{
			List<Mission> listOfAppliedMissionsOfUser = _unitOfWork.VolunteeringTimesheet.GetAllAppliedMissionsOfUser(userId);
			return listOfAppliedMissionsOfUser;
		}

		public bool IsDateValidToSaveTimesheetEntry(DateTime date, long? missionId)
		{
			var mission = _unitOfWork.VolunteeringTimesheet.GetMission(missionId);
			if (mission != null)
			{
				if (date > mission?.StartDate && date < mission.EndDate)
				{
					return true;
				}
			}
			return false;
		}

		public void SaveTimeData(MissionTimesheetTimeModel missionTimesheetTimeModel)
		{
			Timesheet timesheetObj = new()
			{
				UserId = missionTimesheetTimeModel.UserId,
				MissionId = missionTimesheetTimeModel.MissionId,
				Time = new TimeSpan(missionTimesheetTimeModel.Hours, missionTimesheetTimeModel.Minutes, 0),
				//DateVolunteered = missionTimesheetTimeModel.DateVolunteered,
				Notes = missionTimesheetTimeModel.Notes,
				Status = "PENDING",
				CreatedAt = DateTime.Now,
			};

			_unitOfWork.VolunteeringTimesheet.SaveTimeData(timesheetObj);
		}
	}
}
