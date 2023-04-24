using System.ComponentModel.DataAnnotations;

namespace CI_Project.Entities.ViewModels
{
    public class SkillModel
    {
        public int SkillId { get; set; }

		[Required]
        public string SkillName { get; set; } = null!;

		public byte Status { get; set; }

		public bool isSkillInUse { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }
	}
}
