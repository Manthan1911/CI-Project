using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
    public class CmsRepository : ICmsRepository
    {
        private readonly CIProjectDbContext _db;

        public CmsRepository(CIProjectDbContext db)
        {
            _db = db;
        }

        public void addCmsPage(CmsPage cmsPage)
        {
            _db.CmsPages.Add(cmsPage);
            _db.SaveChanges();
        }

        public void deleteCmsPage(CmsPage cmsPage)
        {
            _db.Remove(cmsPage);
            _db.SaveChanges();
        }

        public CmsPage findCmsPageById(long cmsPageId)
        {
            return _db.CmsPages.FirstOrDefault(cms => cms.CmsPageId == cmsPageId);
        }

        public List<CmsPage> GetAll()
        {
            return _db.CmsPages.ToList();
        }

        public List<CmsModel> GetConvertedAll()
        {
            List<CmsPage> cmsObj = GetAll();
            List<CmsModel> cmsVm = new List<CmsModel>();
            cmsObj.ForEach((cms) =>
            {
                cmsVm.Add(new CmsModel()
                {
                    Title = cms.Title,
                    Description = cms.Description,
                    CmsPageId = cms.CmsPageId,
                    Status = cms.Status,
                    Slug = cms.Slug,
                    CreatedAt = cms.CreatedAt,
                    UpdatedAt = cms.UpdatedAt,
                    DeletedAt = cms.DeletedAt,
                });
            });
            return cmsVm;

        }

        public void updateCmsPage(CmsPage cmsPage)
        {
            _db.Update(cmsPage);
            _db.SaveChanges();
        }
    }
}
