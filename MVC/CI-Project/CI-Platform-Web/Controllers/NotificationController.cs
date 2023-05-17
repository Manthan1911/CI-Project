using CI_Project.Entities.ViewModels;
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
            var notificationMainModel = new NotificationMainModel();

            notificationMainModel.LatestNotifications = await _unitOfService.Notification.GetAllByUserId(userId);
            return PartialView("_NotificationListPartial", notificationMainModel);
        }

        public async Task<IActionResult> GetNotificationSettingsPartial(long userId)
        {
            var userNotificationSettingsVm = await _unitOfService.Notification.GetNotificationSettingsByUserId(userId);
            return PartialView("_NotificationSettingsPartial",userNotificationSettingsVm);
        }

        [HttpPut]
        public async Task UpdateNotificationSettings(NotificationSettingsModel newNotificationSettingsVm) =>    await _unitOfService.Notification.SaveNotificationSettings(newNotificationSettingsVm);
    }
}
    