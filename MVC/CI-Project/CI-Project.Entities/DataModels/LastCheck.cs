using System;
using System.Collections.Generic;

namespace CI_Project.Entities.DataModels;

public partial class LastCheck
{
    public long UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
