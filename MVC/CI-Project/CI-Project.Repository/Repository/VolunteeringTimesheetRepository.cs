using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
    public class VolunteeringTimesheetRepository : Repository<Timesheet>, IVolunteeringTimesheetRepository
    {
        private CIProjectDbContext _db;

        public VolunteeringTimesheetRepository(CIProjectDbContext db) : base(db)
        {
            _db = db;
        }

        public List<Timesheet> GetAllWithInclude()
        {
            return table.Include(t => t.User).Include(t => t.Mission).ToList();
        }


    }
}
