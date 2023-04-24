namespace CI_Project.Entities.ViewModels
{
    public class ThemeModel
    {
        public long MissionThemeId { get; set; }

        public string MissionThemeTitle { get; set; } = null!;

		public byte Status { get; set; }

		public bool IsThemeAlreadyInUse { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }
	}
}
