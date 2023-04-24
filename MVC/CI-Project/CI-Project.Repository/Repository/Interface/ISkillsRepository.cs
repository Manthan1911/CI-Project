using CI_Project.Entities.DataModels;

namespace CI_Project.Repository.Repository.Interface
{
	public interface ISkillsRepository
	{
		public List<Skill> getAllSkills();
		public Skill getSkillById(int skillId);

		public void AddSkill(Skill skill);

		public void UpdateSkill(Skill skill);

		public void DeleteSkill(Skill skill);
	}
}
