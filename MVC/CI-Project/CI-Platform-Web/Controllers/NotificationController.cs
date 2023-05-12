using CI_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IUnitOfService _unitOfService;
        public NotificationController(IUnitOfService unitOfService)
        {
            _unitOfService = unitOfService;
        }

        public async Task<IActionResult> GetAllNotificationsOfUser(long userId)
        {
            var userNotifications = await _unitOfService.Notification.GetAllByUserId(userId);

            return PartialView("_NotificationListPartial",userNotifications);
        }
    }
}
    