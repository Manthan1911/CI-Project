using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
    public class LastCheckRepository : Repository<LastCheck>, ILastCheckRepository
    {
        public LastCheckRepository(CIProjectDbContext db) : base(db)
        {
        }
    }
}
