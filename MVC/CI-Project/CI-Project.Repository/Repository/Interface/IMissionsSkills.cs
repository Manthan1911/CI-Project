using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface IMissionsSkills
	{
		public List<MissionSkills> GetAllMissionSkills();

		public void AddMissionSkill(MissionSkills missionSkill);
	}
}
