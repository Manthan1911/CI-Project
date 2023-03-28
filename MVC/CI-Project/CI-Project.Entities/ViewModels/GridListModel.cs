using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
	public class GridListModel
	{
		public List<MissionModel> missionModels { get; set; }

		public long userId { get; set; }
	}
}
