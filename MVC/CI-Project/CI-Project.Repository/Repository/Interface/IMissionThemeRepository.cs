using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface IMissionThemeRepository
	{
		public List<MissionTheme> GetAllThemes();
		public void AddTheme(MissionTheme theme);
		public void UpdateTheme(MissionTheme theme);

		public void DeleteTheme(MissionTheme theme);

		public MissionTheme GetThemeById(long themeId);

	}
}
