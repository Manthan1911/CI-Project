using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
	public class BannerRepository : IBannerRepository
	{

		private readonly CIProjectDbContext _db;

		public BannerRepository(CIProjectDbContext db)
		{
			_db= db;
		}

		public void AddBanner(Banner banner)
		{
			_db.Banners.Add(banner);
			_db.SaveChanges();
		}

		public void DeleteBanner(Banner banner)
		{
			_db.Banners.Remove(banner);
			_db.SaveChanges();
		}

		public List<Banner> GetAllBanner()
		{
			return _db.Banners.ToList();
		}

		public Banner? GetBannerById(long bannerId)
		{
			return _db.Banners.FirstOrDefault(banner => banner.BannerId == bannerId);
		}

		public void UpdateBanner(Banner banner)
		{
			_db.Banners.Update(banner);
			_db.SaveChanges();
		}
	}
}
