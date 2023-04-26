using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository
{
    public class GoalMissionRepositor : IGoalMissionRepository
    {

        private readonly CIProjectDbContext _db;

        public GoalMissionRepositor(CIProjectDbContext db)
        {
            _db = db;
        }

        public void AddGoalMission(GoalMission goalMissionObj)
        {
            _db.GoalMissions.Add(goalMissionObj);
            _db.SaveChanges();
        }

        public void UpdateGoalMission(GoalMission goalMissionObj)
        {
            _db.GoalMissions.Update(goalMissionObj);
            _db.SaveChanges();
        }

        public List<GoalMission> GetAllGoalMissionWithInclude()
        {
            return _db.GoalMissions.Include(goalMission => goalMission.Mission).ToList();
        }
    }
}
