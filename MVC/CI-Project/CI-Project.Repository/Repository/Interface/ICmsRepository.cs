using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface ICmsRepository
	{
		public List<CmsPage> GetAll();
		public List<CmsModel> GetConvertedAll();
		public void addCmsPage(CmsPage cmsPage);
		public void deleteCmsPage(CmsPage cmsPage);
		public void updateCmsPage(CmsPage cmsPage);
		public CmsPage findCmsPageById(long cmsPageId);


	}
}
