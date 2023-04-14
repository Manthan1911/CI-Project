using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IVolunteeringTimesheetRepository
    {
        public List<Timesheet> GetAllWithInclude();

    }
}
