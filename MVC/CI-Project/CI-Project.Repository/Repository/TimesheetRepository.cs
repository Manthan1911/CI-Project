using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
	public class TimesheetRepository:ITimesheetRepository
	{
		private readonly CIProjectDbContext _db;

		public TimesheetRepository(CIProjectDbContext db)
		{
			_db = db;
		}

		public List<Timesheet> GetAllTimesheet()
		{
			return _db.Timesheets
				.Include(timesheet => timesheet.User)
				.Include(timesheet => timesheet.Mission)
				.ToList();
		}

        public Timesheet GetTimesheetById(long id)
        {
			return _db.Timesheets.FirstOrDefault(timesheet => timesheet.TimesheetId == id);
        }

        public void UpdateTimesheet(Timesheet timesheet)
		{
			_db.Timesheets.Update(timesheet);
			_db.SaveChanges();
		}

	}
}
