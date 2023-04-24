using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
	public class CmsRepository:ICmsRepository
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

		public void updateCmsPage(CmsPage cmsPage)
		{
			_db.Update(cmsPage);
			_db.SaveChanges();
		}
	}
}
