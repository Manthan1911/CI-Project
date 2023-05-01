using System;
using System.Collections.Generic;

namespace CI_Project.Entities.DataModels;

public partial class Banner
{
    public long BannerId { get; set; }

    public string? MediaName { get; set; }

    public string? MediaType { get; set; }

    public string? MediaPath { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
