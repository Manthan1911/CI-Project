using CI_Project.Entities.ViewModels;
using CI_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
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
            missionTimesheetTimeModel.Missions = _unitOfService.VolunteeringTimesheet.GetMissionList(userId);
            return PartialView("_AddTimeModal",missionTimesheetTimeModel);
        }

		public IActionResult GetAddGoalModalPartial(long userId)
		{
            MissionTimesheetGoalModel missionTimesheetGoalModel = new();
			return PartialView("_AddGoalModal", missionTimesheetGoalModel);
		}

        [HttpPost]
        public IActionResult SaveTimeData(MissionTimesheetTimeModel missionTimesheetTimeModel)
        {
            try
            {
				var isDateValid = _unitOfService.VolunteeringTimesheet.IsDateValidToSaveTimesheetEntry(DateTime.Now, missionTimesheetTimeModel.MissionId);

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
	}
}
