using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
	public class MissionThemeRepository : IMissionThemeRepository
	{
		private readonly CIProjectDbContext _db;

		public MissionThemeRepository(CIProjectDbContext db)
		{
			_db = db;
		}

		public void AddTheme(MissionTheme theme)
		{
			_db.Add(theme);
			_db.SaveChanges();
		}

		public void DeleteTheme(MissionTheme theme)
		{
			_db.Remove(theme);	
			_db.SaveChanges();
		}

		public List<MissionTheme> GetAllThemes()
		{
			return _db.MissionThemes.ToList();
		}

		public void UpdateTheme(MissionTheme theme)
		{
			_db.MissionThemes.Update(theme);
			_db.SaveChanges();
		}

		public MissionTheme GetThemeById(long themeId)
		{
			return _db.MissionThemes.FirstOrDefault(missionTheme => missionTheme.MissionThemeId == themeId);
		}
	}
}
