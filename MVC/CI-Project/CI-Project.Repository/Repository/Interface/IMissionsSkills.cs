using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface IMissionsSkill
	{
		public List<MissionSkill> GetAllMissionSkills();

		public void AddMissionSkill(MissionSkill missionSkill);

		public void DeleteListOfMissionSkills(List<MissionSkill> missionSkills);
	}
}
