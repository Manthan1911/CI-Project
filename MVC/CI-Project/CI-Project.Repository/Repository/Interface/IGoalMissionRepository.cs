using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IGoalMissionRepository
    {
        public List<GoalMission> GetAllGoalMissionWithInclude();
        public void AddGoalMission(GoalMission goalMissionObj);
        public void UpdateGoalMission(GoalMission goalMissionObj);
    }
}
