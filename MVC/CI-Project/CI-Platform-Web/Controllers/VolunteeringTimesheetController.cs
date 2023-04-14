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
    }
}
