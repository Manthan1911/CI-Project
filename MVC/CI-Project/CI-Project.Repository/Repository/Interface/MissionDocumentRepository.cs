using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
    public class MissionDocumentRepository : IMissionDocument
    {
        private readonly CIProjectDbContext _db;

        public MissionDocumentRepository(CIProjectDbContext db)
        {
            _db = db;
        }

        public void AddMissionDocument(MissionDocument missionDocumentObj)
        {
            _db.Add(missionDocumentObj);
            _db.SaveChanges();
        }

		public void DeleteAllMissionDocumentsByMissionId(long missionId)
		{
            List<MissionDocument> missionDocuments = _db.MissionDocuments.Where(missionDocument => missionDocument.MissionId == missionId).ToList();

            _db.RemoveRange(missionDocuments);
            _db.SaveChanges();
		}

		public List<MissionDocument> GetAllMissionDocumentsd()
        {
            return _db.MissionDocuments.ToList();
        }
    }
}
