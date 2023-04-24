using System;
using System.Collections.Generic;

namespace CI_Project.Entities.DataModels;

public partial class Skill
{
    public int SkillId { get; set; }

    public string SkillName { get; set; } = null!;

    public byte Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<MissionSkills> MissionSkills { get; } = new List<MissionSkills>();

    public virtual ICollection<UserSkill> UserSkills { get; } = new List<UserSkill>();
}
