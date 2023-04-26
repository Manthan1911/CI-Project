using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public interface IMissionMediaRepository
    {
        public List<MissionMedium> GetAllMissionMedia();
        public void AddMissionMedia(MissionMedium missionMedia);
        public void DeleteAllMissionMediaByMissionId(long missionId);
    }
}
