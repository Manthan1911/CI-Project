using CI_Project.Entities.DataModels;
using CI_Project.Entities.ViewModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface IBannerRepository
	{
		public List<Banner> GetAllBanner();
		public Banner? GetBannerById(long bannerId);
		public void UpdateBanner(Banner banner);
		public void AddBanner(Banner banner);
		public void DeleteBanner(Banner banner);

		public List<BannerModel> GetBannerModelList();

		public BannerModel ConvertBannerToBannerModel(Banner banner);
	}
}
