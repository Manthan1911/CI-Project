using CI_Project.Repository.Repository.Interface;
using CI_Project.Services.Interface;

namespace CI_Project.Services
{
    public class UnitOfService : IUnitOfService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            UserProfile = new UserProfileService(_unitOfWork);
            VolunteeringTimesheet = new VolunteeringTimesheetService(_unitOfWork);
            Password = new PasswordService(_unitOfWork);
            Notification = new NotificationService(_unitOfWork);
        }
        public IUserProfileService UserProfile { get; }
        public IVolunteeringTimesheetService VolunteeringTimesheet { get; }
        public IPasswordService Password { get; }
        public INotificationService Notification { get; }

    }
}
