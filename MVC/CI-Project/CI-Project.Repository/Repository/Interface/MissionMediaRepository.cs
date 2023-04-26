using CI_Project.Entities.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CI_Project.Repository.Repository.Interface
{
    public class MissionMediaRepository : IMissionMediaRepository
    {

        private readonly CIProjectDbContext _db;

        public MissionMediaRepository(CIProjectDbContext db)
        {
            _db = db;
        }

        public void AddMissionMedia(MissionMedium missionMedia)
        {
            _db.MissionMedia.Add(missionMedia);
            _db.SaveChanges();
        }

        public void DeleteAllMissionMediaByMissionId(long missionId)
        {
            List<MissionMedium> missionMedia = GetAllMissionMedia().Where(missionMedia => missionMedia.MissionId == missionId).ToList();
            _db.RemoveRange(missionMedia);
            _db.SaveChanges();
        }

        public List<MissionMedium> GetAllMissionMedia()
        {
            return _db.MissionMedia
                    .Include(missionMdeia => missionMdeia.Mission)
                    .ToList();
        }
    }
}