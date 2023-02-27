using System;
using System.Collections.Generic;

namespace CI_Project.Entities.DataModels;

public partial class MissionApplication
{
    public long MissionApplicationId { get; set; }

    public long? MissionId { get; set; }

    public long? UserId { get; set; }

    public DateTime AppliedAt { get; set; }

    public string ApprovalStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Mission? Mission { get; set; }

    public virtual User? User { get; set; }
}
