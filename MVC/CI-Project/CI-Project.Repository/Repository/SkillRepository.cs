using CI_Project.Entities.DataModels;
using CI_Project.Repository.Repository.Interface;

namespace CI_Project.Repository.Repository
{
	public class SkillRepository : ISkillsRepository
	{
		private readonly CIProjectDbContext _db;

		public SkillRepository(CIProjectDbContext db)
		{
			_db = db;
		}

		public void AddSkill(Skill skill)
		{
			_db.Skills.Add(skill);
			_db.SaveChanges();
		}

		public void DeleteSkill(Skill skill)
		{
			_db.Skills.Remove(skill);
			_db.SaveChanges();
		}

		public List<Skill> getAllSkills()
		{
			return _db.Skills.ToList();
		}

		public Skill getSkillById(int skillId)
		{
			return _db.Skills.FirstOrDefault(skill => skill.SkillId == skillId);
		}

		public void UpdateSkill(Skill skill)
		{
			_db.Skills.Update(skill);
			_db.SaveChanges();
		}
	}
}
