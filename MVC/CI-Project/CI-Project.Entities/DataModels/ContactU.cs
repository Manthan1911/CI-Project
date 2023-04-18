using System;
using System.Collections.Generic;

namespace CI_Project.Entities.DataModels;

public partial class ContactU
{
    public long ContactUsId { get; set; }

    public long UserId { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public string? Reply { get; set; }

    public bool? Status { get; set; }

    public virtual User User { get; set; } = null!;
}
