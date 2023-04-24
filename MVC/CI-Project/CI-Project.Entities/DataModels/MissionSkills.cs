using System;
using System.Collections.Generic;

namespace CI_Project.Entities.DataModels;

public partial class MissionSkills
{
    public long MissionSkillId { get; set; }

    public int SkillId { get; set; }

    public long? MissionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual Skill Skill { get; set; } = null!;
}
