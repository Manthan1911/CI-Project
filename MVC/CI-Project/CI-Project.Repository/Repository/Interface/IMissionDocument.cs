using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public  interface IMissionDocument
    {
        public List<MissionDocument> GetAllMissionDocumentsd();
        public void AddMissionDocument(MissionDocument missionDocumentObj);
    }
}
