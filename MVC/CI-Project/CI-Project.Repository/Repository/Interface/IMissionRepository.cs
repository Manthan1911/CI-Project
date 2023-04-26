using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IMissionRepository
    {
        public List<Mission> GetAllMissionsWithInclude();
        public void AddMission(Mission mission);
        public void UpdateMission(Mission mission);

    }
}
