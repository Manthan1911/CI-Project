using CI_Project.Entities.DataModels;

namespace CI_Project.Entities.ViewModels
{
	public class UserModel
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string PhoneNo { get; set; }

		public string EmailId { get; set; }

		public long UserId { get; set; }

		public virtual ICollection<MissionInvite>? missionInvitesTo { get; set; }
		public virtual ICollection<MissionInvite>? missionInvitesFrom { get; set; }
	}
}
