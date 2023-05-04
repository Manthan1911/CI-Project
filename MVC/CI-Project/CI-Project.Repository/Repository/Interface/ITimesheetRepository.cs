using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface ITimesheetRepository
	{
		public List<Timesheet> GetAllTimesheet();

		public void UpdateTimesheet(Timesheet timesheet);

		public Timesheet GetTimesheetById(long id);
	}
}
