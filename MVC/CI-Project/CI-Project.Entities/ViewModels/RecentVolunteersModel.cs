using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
	public class RecentVolunteersModel
	{
		public List<MissionApplication>? recentVolunteers { get; set; }

		public int? recentVolunteersCount { get; set; } 

		public int? indexOfFirstVolunteerOfCurrPage { get; set; }

		public int? indexOfLastVolunteerOfCurrPage { get; set; }
	}
}
