using CI_Platform_Web.Utilities;
using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
	[Authenticate]
    public class VolunteeringTimesheetController : Controller
    {
        private readonly IUnitOfService _unitOfService;

        public VolunteeringTimesheetController(IUnitOfService unitOfService)
        {
            _unitOfService = unitOfService;
        }

        public IActionResult Index(long? userId)
        {
            var userEmailId = HttpContext.Session.GetString("UserEmail");
            if (userEmailId == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            if (userId == null)
            {
                return View("PageNotFound", "Authentication");
            }

            return View();
        }

        public IActionResult GetTimeBasedPartialView(long userId)
        {
            List<MissionTimesheetTimeModel> missionTimesheetTimeVm = _unitOfService.VolunteeringTimesheet.GetAllTimeData(userId);
            return PartialView("_TimesheetTimeBasedPartial", missionTimesheetTimeVm);
        }

        public IActionResult GetGoalBasedPartialView(long userId)
        {
            List<MissionTimesheetGoalModel> missionTimesheetGoalVm = _unitOfService.VolunteeringTimesheet.GetAllGoalData(userId);
            return PartialView("_TimesheetGoalBasedPartial", missionTimesheetGoalVm);
        }

        public IActionResult GetAddTimeModalPartial(long userId)
        {
            MissionTimesheetTimeModel missionTimesheetTimeModel = new();
            missionTimesheetTimeModel.Missions = _unitOfService.VolunteeringTimesheet.GetMissionList(userId,"time");
            return PartialView("_AddTimeModal",missionTimesheetTimeModel);
        }

		public IActionResult GetAddGoalModalPartial(long userId)
		{
            MissionTimesheetGoalModel missionTimesheetGoalModel = new();
			missionTimesheetGoalModel.Missions = _unitOfService.VolunteeringTimesheet.GetMissionList(userId, "goal");
			return PartialView("_AddGoalModal", missionTimesheetGoalModel);
		}

        [HttpPost]
        public IActionResult SaveTimeData(MissionTimesheetTimeModel missionTimesheetTimeModel)
        {
            try
            {
				var isDateValid = _unitOfService.VolunteeringTimesheet.IsDateValidToSaveTimesheetEntry((DateTime)missionTimesheetTimeModel.DateVolunteered, missionTimesheetTimeModel.MissionId);

				if (!isDateValid)
				{
					return NoContent();
				}

				_unitOfService.VolunteeringTimesheet.SaveTimeData(missionTimesheetTimeModel);
			}
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok(200);
		}

		public IActionResult SaveGoalData(MissionTimesheetGoalModel missionTimesheetGoalModel)
		{
			try
			{
				var isDateValid = _unitOfService.VolunteeringTimesheet.IsDateValidToSaveTimesheetEntry((DateTime)missionTimesheetGoalModel.DateVolunteered, missionTimesheetGoalModel.MissionId);

				if (!isDateValid)
				{
					return NoContent();
				}
				_unitOfService.VolunteeringTimesheet.SaveGoalData(missionTimesheetGoalModel);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return Ok(200);
		}

		// -------------------(Time Based) Edit Timesheet-------------------
        public IActionResult GetTimeBasedEditPartial(long userId,long timesheetId)
        {
            MissionTimesheetTimeModel? timeObj = _unitOfService.VolunteeringTimesheet.GetParticularTimeBasedData(timesheetId);
            timeObj.Missions = _unitOfService.VolunteeringTimesheet.GetMissionList(userId, "time");
			if (timeObj == null)
            {
                return NoContent();
            }
            return PartialView("_EditTimeModal",timeObj);
        }

        public IActionResult EditTimeData(MissionTimesheetTimeModel missionTimesheetTimeModel)
        {
			try
			{
				var isDateValid = _unitOfService.VolunteeringTimesheet.IsDateValidToSaveTimesheetEntry((DateTime)missionTimesheetTimeModel.DateVolunteered, missionTimesheetTimeModel.MissionId);

				if (!isDateValid)
				{
					return NoContent();
				}

				_unitOfService.VolunteeringTimesheet.EditTimeData(missionTimesheetTimeModel);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return Ok(200);
		}

		// -------------------(Goal Based) Edit Timesheet-------------------
		public IActionResult GetGoalBasedEditPartial(long userId, long timesheetId)
		{
			MissionTimesheetGoalModel? goalObj = _unitOfService.VolunteeringTimesheet.GetParticularGoalBasedData(timesheetId);
			goalObj.Missions = _unitOfService.VolunteeringTimesheet.GetMissionList(userId, "goal");
			if (goalObj == null)
			{
				return NoContent();
			}
			return PartialView("_EditGoalModal", goalObj);
		}

		public IActionResult EditGoalData(MissionTimesheetGoalModel missionTimesheetGoalModel)
		{
			try
			{
				var isDateValid = _unitOfService.VolunteeringTimesheet.IsDateValidToSaveTimesheetEntry((DateTime)missionTimesheetGoalModel.DateVolunteered, missionTimesheetGoalModel.MissionId);

				if (!isDateValid)
				{
					return NoContent();
				}

				_unitOfService.VolunteeringTimesheet.EditGoalData(missionTimesheetGoalModel);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return Ok(200);
		}


		// ------------------- Delete Timesheet-------------------
		public IActionResult DeleteTimeData(long timesheetId)
		{
			try
			{
				_unitOfService.VolunteeringTimesheet.DeleteTimesheetData(timesheetId);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return Ok(200);
		}
	}
}
