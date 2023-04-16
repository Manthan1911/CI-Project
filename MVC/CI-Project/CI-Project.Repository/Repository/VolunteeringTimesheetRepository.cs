using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

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

        public List<Mission> GetAllAppliedMissionsOfUser(long userId)
        {
            var listOfAppliedMissionId = _db.MissionApplications.Where(ma => ma.UserId == userId).Select(ma => ma.MissionId).ToArray();
            List<Mission> appliedMissionList = _db.Missions.Where(m => listOfAppliedMissionId.Contains(m.MissionId)).ToList();
            return appliedMissionList;
        }

        public Mission? GetMission(long? missionId)
        {
            return _db.Missions.FirstOrDefault(m => m.MissionId == missionId);
        }

        public void SaveTimeData(Timesheet timesheetObj)
        {
            _db.Timesheets.Add(timesheetObj);
            _db.SaveChanges();
        }
    }
}
