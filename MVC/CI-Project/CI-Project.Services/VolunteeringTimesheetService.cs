using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository;
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
				DateVolunteered = timesheetData.DateVolunteered,
				UserId = timesheetData.UserId,
				Notes = timesheetData?.Notes,
				ApprovalStatus = timesheetData?.Status,
				Minutes = timesheetData.Time.Value.Minutes,
				Hours = timesheetData.Time.Value.Hours,
				TimesheetId=timesheetData.TimesheetId,
			};

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
				TimesheetId = timesheetData.TimesheetId,
				MissionId = timesheetData.MissionId,
				MissionName = timesheetData.Mission.Title,
				UserId = timesheetData.UserId,
				Action = timesheetData.Action,
				Notes = timesheetData?.Notes,
				DateVolunteered = timesheetData?.DateVolunteered,
				ApprovalStatus = timesheetData?.Status,
			};
		}

		public List<Mission> GetMissionList(long userId,string type)
		{
			List<Mission> listOfAppliedMissionsOfUser = _unitOfWork.VolunteeringTimesheet.GetAllAppliedMissionsOfUser(userId,type);
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
				Time = new TimeSpan((int)missionTimesheetTimeModel.Hours, (int)missionTimesheetTimeModel.Minutes, 0),
				DateVolunteered = (DateTime)missionTimesheetTimeModel.DateVolunteered,
				Notes = missionTimesheetTimeModel.Notes,
				Status = "PENDING",
				CreatedAt = DateTime.Now,
			};

			_unitOfWork.VolunteeringTimesheet.Insert(timesheetObj);
			_unitOfWork.Save();
		}

		public void SaveGoalData(MissionTimesheetGoalModel missionTimesheetGoalModel)
		{
			Timesheet goalObj = new()
			{
				UserId=(long)missionTimesheetGoalModel.UserId,
				MissionId=(long)missionTimesheetGoalModel.MissionId,
				Action= missionTimesheetGoalModel.Action,
				DateVolunteered=(DateTime)missionTimesheetGoalModel.DateVolunteered,
				Notes=missionTimesheetGoalModel.Notes,
				Status="PENDING",
				CreatedAt = DateTime.Now,
			};
			_unitOfWork.VolunteeringTimesheet.Insert(goalObj);
			_unitOfWork.Save();
		}

		public MissionTimesheetTimeModel GetParticularTimeBasedData(long timesheetId)
		{
			Timesheet timesheetObj = _unitOfWork.VolunteeringTimesheet.GetAllWithInclude().FirstOrDefault(timesheet => timesheet.TimesheetId == timesheetId);
			return ConvertToTimeModel(timesheetObj);
		}

		public void EditTimeData(MissionTimesheetTimeModel missionTimesheetTimeModel)
		{
			Timesheet timesheetObj = _unitOfWork.VolunteeringTimesheet.GetAllWithInclude().FirstOrDefault(timesheet => timesheet.TimesheetId == missionTimesheetTimeModel.TimesheetId);
			timesheetObj.MissionId = missionTimesheetTimeModel.MissionId;
			timesheetObj.DateVolunteered = (DateTime)missionTimesheetTimeModel.DateVolunteered;
			timesheetObj.Notes= missionTimesheetTimeModel.Notes;
			timesheetObj.UpdatedAt= DateTime.Now;
			timesheetObj.Time = new TimeSpan((int)missionTimesheetTimeModel.Hours,(int)missionTimesheetTimeModel.Minutes,0);

			_unitOfWork.VolunteeringTimesheet.Update(timesheetObj);
			_unitOfWork.Save();
		}

		public void EditGoalData(MissionTimesheetGoalModel missionTimesheetGoalModel)
		{
			Timesheet timesheetObj = _unitOfWork.VolunteeringTimesheet.GetAllWithInclude().FirstOrDefault(timesheet => timesheet.TimesheetId == missionTimesheetGoalModel.TimesheetId);
			timesheetObj.MissionId = (long)missionTimesheetGoalModel.MissionId;
			timesheetObj.DateVolunteered = (DateTime)missionTimesheetGoalModel.DateVolunteered;
			timesheetObj.Notes= missionTimesheetGoalModel.Notes;
			timesheetObj.UpdatedAt= DateTime.Now;
			timesheetObj.Action = missionTimesheetGoalModel.Action;

			_unitOfWork.VolunteeringTimesheet.Update(timesheetObj);
			_unitOfWork.Save();
		}

		public MissionTimesheetGoalModel GetParticularGoalBasedData(long timesheetId)
		{
			Timesheet timesheetObj = _unitOfWork.VolunteeringTimesheet.GetAllWithInclude().FirstOrDefault(timesheet => timesheet.TimesheetId == timesheetId);
			return ConvertToGoalModel(timesheetObj);
		}

		public Timesheet GetTimesheetData(long timesheetId)
		{
			return _unitOfWork.VolunteeringTimesheet.GetFirstOrDefault(timesheet => timesheet.TimesheetId == timesheetId);
		}

		public void DeleteTimesheetData(long timesheetId)
		{
			Timesheet timesheetObj = GetTimesheetData(timesheetId);
			_unitOfWork.VolunteeringTimesheet.Delete(timesheetObj);
			_unitOfWork.Save();
		}
	}
}
